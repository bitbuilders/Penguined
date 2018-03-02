using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gears : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 100.0f)] float m_amplitude = 3.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_rate = 3.0f;
    [SerializeField] [Range(-900.0f, 900.0f)] float m_rotationSpeed = 90.0f;

    Vector3 m_acutalPosition;

    private void Start()
    {
        m_acutalPosition = transform.position;
    }

    private void Update()
    {
        float y = Time.time * m_rate;
        float t = (1.0f - Mathf.Cos(y * Mathf.PI)) * 0.5f;
        Vector3 heightOffset = Vector3.up * (t * m_amplitude);
        transform.position = m_acutalPosition + heightOffset;

        Vector3 angle = Vector3.up * m_rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation *= rotation;
    }

    public void MovePosition(Vector3 newPosition)
    {
        m_acutalPosition = newPosition;
    }
}
