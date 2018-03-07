using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gears : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 100.0f)] float m_amplitude = 3.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_rate = 3.0f;
    [SerializeField] [Range(-900.0f, 900.0f)] float m_rotationSpeed = 90.0f;

    Player m_player;
    Vector3 m_acutalPosition;
    Vector3 m_heightOffset;
    float m_speedIncrease = 1.0f;
    bool m_traveling;

    private void Start()
    {
        m_acutalPosition = transform.position;
    }

    private void Update()
    {
        if (m_traveling)
        {
            m_speedIncrease += Time.deltaTime * 4.0f;
            Vector3 target = PickupInfo.Instance.GetWorldFromScreenPoint();
            MovePosition(Vector3.Lerp(m_acutalPosition, target, Time.deltaTime * m_speedIncrease));

            if ((target - m_acutalPosition).magnitude < 0.5f)
            {
                m_player.GearCount++;
                Destroy(gameObject);
            }
        }
        else
        {
            float y = Time.time * m_rate;
            float t = (1.0f - Mathf.Cos(y * Mathf.PI)) * 0.5f;
            m_heightOffset = Vector3.up * (t * m_amplitude);
        }
        transform.position = m_acutalPosition + m_heightOffset;

        Vector3 angle = Vector3.up * m_rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(angle);
        transform.rotation *= rotation;
    }

    public void MovePosition(Vector3 newPosition)
    {
        m_acutalPosition = newPosition;
    }

    public void Pickup(Player player)
    {
        m_traveling = true;
        m_player = player;
    }
}
