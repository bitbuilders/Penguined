using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] float m_speed = 50.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_turnSpeed = 4.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpResistance = 2.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_fallMultiplier = 2.0f;

    Rigidbody m_rigidbody;
    float m_speedScale;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_speedScale = m_rigidbody.mass * 50.0f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_rigidbody.AddForce(Vector3.up * 100.0f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        velocity.z = Input.GetAxis("Vertical");
        velocity.x = Input.GetAxis("Horizontal");
        velocity = velocity * m_speed * m_speedScale * Time.deltaTime;

        velocity = Camera.main.transform.rotation * velocity; // only make forward affected by camera
        velocity.y = 0.0f;
        m_rigidbody.AddForce(velocity, ForceMode.Force);

        if (m_rigidbody.velocity.magnitude > 0.00001f)
        {
            Vector3 lookDir = m_rigidbody.velocity.normalized;
            lookDir.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);
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
}
