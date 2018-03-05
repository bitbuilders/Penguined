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
    [SerializeField] [Range(1, 10)] int m_bonusJumps = 2;
    [SerializeField] Transform m_groundTouchPoint = null;
    [SerializeField] LayerMask m_groundMask;

    Rigidbody m_rigidbody;
    float m_speedScale;
    int m_currentJump = 0;
    bool m_onGround;

    public bool OnGround { get { return m_onGround; } }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_speedScale = m_rigidbody.mass * 50.0f;
    }

    void Update()
    {
        Collider[] c = Physics.OverlapSphere(m_groundTouchPoint.position, 0.1f, m_groundMask);
        m_onGround = c.Length > 0 ? true : false;
        
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

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        velocity.z = Input.GetAxis("Vertical");
        velocity.x = Input.GetAxis("Horizontal");

        velocity = Camera.main.transform.rotation * velocity;
        velocity.y = 0.0f;
        velocity.Normalize();
        velocity = velocity * m_speed * m_speedScale * Time.deltaTime;
        velocity = OnGround ? velocity : velocity * 0.6f;
        m_rigidbody.AddForce(velocity, ForceMode.Force);

        if (m_rigidbody.velocity.magnitude > 0.00001f)
        {
            Vector3 lookDir = m_rigidbody.velocity.normalized;
            lookDir.y = 0.0f;
            if (lookDir != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);
            }
        }

        if (m_rigidbody.velocity.y < 0.0f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallMultiplier - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y > 0.0f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == m_groundMask.value && m_onGround)
        {
            m_currentJump = 0;
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
}
