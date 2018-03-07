using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInfo : Singleton<PickupInfo>
{
    [SerializeField] Vector3 m_pickupScreenPoint = Vector3.zero;
    [SerializeField] Camera m_mainCamera = null;

    public Vector3 ScreenPoint { get { return m_pickupScreenPoint; } }

    private void Start()
    {
        //m_pickupScreenPoint.x = -Screen.width / 2;
        //m_pickupScreenPoint.y = -Screen.height / 2;
    }

    public Vector3 GetWorldFromScreenPoint()
    {
        return m_mainCamera.ScreenToWorldPoint(ScreenPoint);
    }
}
