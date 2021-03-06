﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 2.0f)] float m_speedAdjust = 0.1f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_range = 20.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fireRate = 0.5f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_launchSpeed = 50.0f;
    [SerializeField] GameObject m_cartridge = null;
    [SerializeField] Transform m_cartridgePoint = null;
    [SerializeField] Transform[] m_firePoints = null;
    [SerializeField] LayerMask m_groundMask;

    Player m_player = null;
    float m_fireTime = 0.0f;
    int m_barrelIndex = 0;
    bool m_targetInRange = false;

    private void Update()
    {
        GameObject player = GetNearestObject("Enemy");

        m_targetInRange = (player.transform.position - transform.position).magnitude < m_range;
        if (m_targetInRange)
        {
            Vector3 targetPosition = player.transform.position + Vector3.up * 0.75f - transform.position;
            Rigidbody playerBody = player.GetComponent<Rigidbody>();
            Vector3 offset = playerBody.velocity * m_speedAdjust;
            targetPosition += offset;
            Quaternion rotation = Quaternion.LookRotation(targetPosition.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);

            if (m_player) m_fireRate = m_player.TurretSpeed;
            m_fireTime += Time.deltaTime;
            if (m_fireTime >= m_fireRate)
            {
                m_fireTime = 0.0f;
                GameObject bullet = BulletPool.Instance.Get();
                bullet.SetActive(true);
                bullet.transform.rotation = transform.rotation;
                m_barrelIndex = m_barrelIndex % m_firePoints.Length;
                bullet.transform.position = m_firePoints[m_barrelIndex++].position;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward.normalized * m_launchSpeed, ForceMode.Impulse);

                Quaternion spawnRot = Quaternion.Euler(0.0f, 120.0f, 0.0f);
                GameObject cartridge = Instantiate(m_cartridge, m_cartridgePoint.position, transform.rotation * spawnRot, m_cartridgePoint);
                Destroy(cartridge, 2.0f);

                AudioManager.Instance.PlayClip("TurretFire", transform.position, false, transform);
            }
        }
        else
        {
            m_fireTime = 0.0f;
        }

        if ((m_player.transform.position - transform.position).magnitude <= 3.0f)
        {
            PlayerHUD.Instance.EnablePickupText();
            if (Input.GetButtonDown("Pickup") && m_player.TurretCount < m_player.TurretLimit)
            {
                m_player.TurretCount++;
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == m_groundMask.value)
        {
            Destroy(GetComponent<Rigidbody>());
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

    public void RegisterPlayer(Player player, bool startup)
    {
        m_player = player;
        if (startup)
        {
            StartCoroutine(ScaleUp());
        }
    }

    IEnumerator ScaleUp()
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            transform.localScale = new Vector3(i, i, i);
            yield return null;
        }

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
