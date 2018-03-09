using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] float m_speed = 50.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_turnSpeed = 4.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpResistance = 2.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_fallMultiplier = 2.0f;
    [SerializeField] [Range(1.0f, 200.0f)] float m_jumpForce = 100.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_turretSpeed = 0.5f;
    [SerializeField] [Range(1, 10)] int m_bonusJumps = 2;
    [SerializeField] GameObject m_turret = null;
    [SerializeField] Transform m_turretThrowLocation = null;
    [SerializeField] Transform m_turretContainer = null;
    [SerializeField] Transform m_groundTouchPoint = null;
    [SerializeField] LayerMask m_groundMask;

    public bool OnGround { get { return m_onGround; } }
    public int GearCount { get; set; }
    public int TurretLimit { get; set; }
    public int TurretCount { get; set; }
    public int PotionCount { get; set; }
    public float Health { get; set; }
    public bool Alive { get { return Health > 0.0f; } }
    public float TurretSpeed { get { return m_turretSpeed; } set { m_turretSpeed = value; } }

    Animator m_animator;
    Rigidbody m_rigidbody;
    float m_speedScale;
    int m_currentJump = 0;
    bool m_onGround;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_speedScale = m_rigidbody.mass * 50.0f;
        ResetValues();
        GearCount = 5;
    }

    void Update()
    {
        Collider[] c = Physics.OverlapSphere(m_groundTouchPoint.position, 0.6f, m_groundMask);
        m_onGround = c.Length > 0 ? true : false;
        
        if (CanMove())
        {
            if (Input.GetButtonDown("Jump") && CanJump())
            {
                Vector3 velocity = m_rigidbody.velocity;
                velocity.y = 0.0f;
                m_rigidbody.velocity = velocity;
                float force = GetJumpForce();
                m_rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
                ++m_currentJump;
            }
        }
    }

    private void FixedUpdate()
    {
        if (CanMove())
        {
            Vector3 velocity = Vector3.zero;
            velocity.z = Input.GetAxis("Vertical");
            velocity.x = Input.GetAxis("Horizontal");
            velocity = velocity * m_speed * m_speedScale * Time.deltaTime;
            velocity = OnGround ? velocity : velocity * 0.6f;

            float angle = Camera.main.transform.rotation.eulerAngles.y;
            Quaternion camRot = Quaternion.AngleAxis(angle, Vector3.up);
            velocity = camRot * velocity;
            velocity.y = 0.0f;
            m_rigidbody.AddForce(velocity, ForceMode.Force);

            if (m_rigidbody.velocity.magnitude > 0.00001f)
            {
                Vector3 lookDir = velocity.normalized;
                lookDir.y = 0.0f;
                if (lookDir != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);
                }
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

            if (Input.GetButtonDown("Attack"))
            {
                m_animator.SetTrigger("Attack");
            }
            if (Input.GetButtonDown("Throw"))
            {
                m_animator.SetTrigger("Throw");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == m_groundMask.value && m_onGround)
        {
            m_currentJump = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gear"))
        {
            other.GetComponent<Gears>().Pickup(this);
        }
    }

    private bool CanJump()
    {
        if (OnGround)
        {
            return true;
        }
        else
        {
            return (m_currentJump < m_bonusJumps + 1);
        }
    }

    private float GetJumpForce()
    {
        return m_jumpForce - (10.0f * m_currentJump);
        //return m_jumpForce;
    }

    private void ResetValues()
    {
        GearCount = 0;
        Health = 100.0f;
    }

    private bool CanMove()
    {
        return !Intro.Instance.IsPlaying && !UpgradeShop.Instance.InMenus;
    }

    public void ThrowTurret()
    {
        if (TurretCount > 0)
        {
            GameObject turret = Instantiate(m_turret, m_turretThrowLocation.position, transform.rotation, m_turretContainer);
            turret.GetComponent<Turret>().RegisterPlayer(this, true);

            Quaternion angle = Quaternion.AngleAxis(-35.0f, Vector3.right);
            Vector3 force = angle * Vector3.forward * 5.0f;
            force = transform.rotation * force;
            turret.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

            TurretCount--;
        }
    }
}
