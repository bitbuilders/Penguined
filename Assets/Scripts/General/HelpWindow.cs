using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour
{
    [SerializeField] Button m_backButton = null;

    private void OnEnable()
    {
        m_backButton.Select();
    }
}
