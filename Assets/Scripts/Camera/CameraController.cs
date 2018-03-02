using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 25.0f)] float m_spaceFromTarget = 10.0f;
    [SerializeField] [Range(0.0f, 30.0f)] float m_verticalOffset = 3.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_followDistance = 10.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_lockIntensity = 10.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_attentiveness = 10.0f;
    [SerializeField] GameObject m_target = null;

    void Start()
    {

    }

    void LateUpdate()
    {
        Vector3 direction = m_target.transform.position - transform.position;
        if (direction.magnitude >= m_followDistance)
        {
            Vector3 dir = transform.position - m_target.transform.position;
            dir.y = 0.0f;
            Vector3 offset = dir.normalized * m_spaceFromTarget;
            Vector3 targetPosition = m_target.transform.position + offset;

            targetPosition.y = m_target.transform.position.y + m_verticalOffset;
            //if ((targetPosition - m_target.transform.position).y < m_verticalOffset)
            //    targetPosition += Vector3.up * Time.deltaTime * 7.0f;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_attentiveness);
            //transform.position = targetPosition;

            Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_lockIntensity);
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_lockIntensity);
        }
    }
}
