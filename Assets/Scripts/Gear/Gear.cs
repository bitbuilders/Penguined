using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] [Range(-900.0f, 900.0f)] float m_rotationSpeed = 360.0f;

    private void Update()
    {
        Vector3 angle = Vector3.forward * m_rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation *= rotation;
    }
}
