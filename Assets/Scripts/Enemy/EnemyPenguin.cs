using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPenguin : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 20.0f)] float m_turnSpeed = 4.0f;
    [SerializeField] [Range(1.0f, 200.0f)] float m_throwSpeed = 50.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpResistance = 2.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_fallMultiplier = 2.0f;
    [SerializeField] [Range(1.0f, 200.0f)] float m_jumpForce = 10.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpRateRange = 2.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_attackRateRange = 2.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_lifetime = 5.0f;
    [SerializeField] Player m_player = null;
    [SerializeField] EnemyIntroData m_introData = null;
    [SerializeField] Transform m_throwLocation = null;
    [SerializeField] bool m_playIntro = false;
    [SerializeField] Transform m_groundTouchPoint = null;
    [SerializeField] LayerMask m_groundMask;

    public bool OnGround { get { return m_onGround; } }
    public float Health { get; set; }
    public bool Alive { get { return Health > 0.0f; } }

    Animator m_animator;
    Rigidbody m_rigidbody;
    float m_jumpTime = 0.0f;
    float m_currentJumpRate = 3.0f;
    float m_attackTime = 0.0f;
    float m_currentAttackRate = 3.0f;
    bool m_onGround;

    public bool m_playedIntro = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();

        Health = 140.0f;
    }

    void Update()
    {
        Collider[] c = Physics.OverlapSphere(m_groundTouchPoint.position, 0.6f, m_groundMask);
        m_onGround = c.Length > 0 ? true : false;

        Vector3 direction = m_player.transform.position - transform.position;
        if (CanMove() && direction.magnitude <= 30.0f)
        {
            m_jumpTime += Time.deltaTime;
            if (CanJump() && m_jumpTime >= m_currentJumpRate)
            {
                Vector3 velocity = m_rigidbody.velocity;
                velocity.y = 0.0f;
                m_rigidbody.velocity = velocity;
                float force = m_jumpForce;
                m_rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

                m_jumpTime = 0.0f;
                m_currentJumpRate = Random.Range(3.0f - m_jumpRateRange, 3.0f + m_jumpRateRange);
            }

            m_attackTime += Time.deltaTime;
            if (m_attackTime >= m_currentAttackRate)
            {
                m_animator.SetTrigger("Attack");
                m_attackTime = 0.0f;
                m_currentAttackRate = Random.Range(3.0f - m_attackRateRange, 3.0f + m_attackRateRange);
            }
            //if (Input.GetButtonDown("Throw"))
            //{
            //    m_animator.SetTrigger("Throw");
            //}

            if (!Alive)
            {
                Destroy(gameObject);
            }
        }

        if (m_playIntro)
        {
            Intro.Instance.PlayIntro(m_introData);
            m_playIntro = false;
        }
    }

    private void FixedUpdate()
    {
        if (CanMove())
        {
            //Vector3 velocity = Vector3.zero;
            //velocity.z = Input.GetAxis("Vertical");
            //velocity.x = Input.GetAxis("Horizontal");
            //velocity = velocity * m_speed * m_speedScale * Time.deltaTime;
            //velocity = OnGround ? velocity : velocity * 0.6f;

            Vector3 lookDir = m_player.transform.position + Vector3.up * 1.0f - transform.position;
            if (lookDir != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);
            }

            if (m_rigidbody.velocity.y < 0.0f && !OnGround)
            {
                m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallMultiplier - 1.0f) * Time.deltaTime;
            }
            else if (m_rigidbody.velocity.y > 0.0f && !OnGround)
            {
                m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
            }

            m_animator.SetFloat("WalkSpeed", m_rigidbody.velocity.magnitude * 0.15f);
            m_animator.SetFloat("yVelocity", m_rigidbody.velocity.y);
            m_animator.SetBool("IsWalking", m_rigidbody.velocity.magnitude > 0.05f && OnGround);
            m_animator.SetBool("OnGround", OnGround);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direciton = transform.position - m_player.transform.position;
            m_rigidbody.AddForce(direciton.normalized * 65.0f, ForceMode.Impulse);

            Health -= 40.0f;
        }
    }

    private bool CanJump()
    {
        return OnGround;
    }

    private bool CanMove()
    {
        return !Intro.Instance.IsPlaying && !UpgradeShop.Instance.InMenus;
    }

    private void ThrowIceball()
    {
        //GameObject ball = Instantiate(m_iceball, m_throwLocation.position, transform.rotation, m_iceballContainer);
        GameObject ball = IceBallPool.Instance.Get();
        ball.SetActive(true);
        BoxCollider collider = ball.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.5f, 0.5f, 0.5f);
        collider.isTrigger = true;
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        ball.transform.position = m_throwLocation.position;

        Vector3 direction = m_player.transform.position - transform.position;
        Vector3 force = direction.normalized * m_throwSpeed;
        rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(StopIceBall(ball));
    }

    IEnumerator StopIceBall(GameObject iceball)
    {
        yield return new WaitForSeconds(m_lifetime);

        iceball.SetActive(false);
    }

    public void PlayIntro()
    {
        m_playIntro = true;
        Destroy(GetComponent<SphereCollider>());
        m_playedIntro = true;
    }
}
