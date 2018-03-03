using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 100.0f)] float m_lifetime = 5.0f;

    ParticleSystem m_particleSystem;
    Rigidbody m_rigidbody;
    float m_time = 0.0f;

    private void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > m_lifetime)
        {
            m_time = 0.0f;
            m_particleSystem.Stop();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (m_rigidbody)
        {
            m_rigidbody.velocity = Vector3.zero;
        }
        if (m_particleSystem)
        {
            m_particleSystem.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
