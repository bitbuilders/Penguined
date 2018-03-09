using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInfo : Singleton<PickupInfo>
{
    [SerializeField] Camera m_mainCamera = null;

    public Vector3 GetWorldFromScreenPoint()
    {
        Vector3 offset = m_mainCamera.transform.rotation * (new Vector3(-10.0f, 4.5f, 10.0f));
        //print(transform.position + offset);
        return transform.position + offset;
    }
}
