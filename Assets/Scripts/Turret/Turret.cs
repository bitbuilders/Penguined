using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 2.0f)] float m_speedAdjust = 0.1f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_range = 20.0f;

    private void Update()
    {
        GameObject player = GetNearestObject("Player");

        if ((player.transform.position - transform.position).magnitude < m_range)
        {
            Vector3 targetPosition = player.transform.position + Vector3.up * -0.5f - transform.position;
            Rigidbody playerBody = player.GetComponent<Rigidbody>();
            Vector3 offset = player.GetComponent<Rigidbody>().velocity * 0.1f;
            targetPosition += offset;
            Quaternion rotation = Quaternion.LookRotation(targetPosition.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
        }
    }

    private GameObject GetNearestObject(string tag)
    {
        GameObject nearest = null;

        float nearestDistance = float.MaxValue;

        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            float distance = (obj.transform.position - transform.position).magnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = obj;
            }
        }

        return nearest;
    }
}
