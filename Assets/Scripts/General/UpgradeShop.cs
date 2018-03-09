using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : Singleton<UpgradeShop>
{
    [SerializeField] Player m_player = null;
    [SerializeField] GameObject m_upgradeWindow = null;
    [SerializeField] GameObject m_helpWindow = null;
    [SerializeField] TextMeshProUGUI m_playerGears = null;
    [SerializeField] Button m_resumeButton = null;

    bool m_inBaseMenu = false;
    bool m_inMenus = false;

    public bool InBaseMenu { get { return m_inBaseMenu; } }
    public bool InMenus { get { return m_inMenus; } }

    private void Start()
    {
        m_inMenus = false;
    }

    private void Update()
    {
        if (m_playerGears)
        {
            m_playerGears.text = m_player.GearCount.ToString();
        }

        if (m_upgradeWindow)
        {
            m_inBaseMenu = m_upgradeWindow.activeInHierarchy;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (m_helpWindow && m_helpWindow.activeInHierarchy)
            {
                m_helpWindow.SetActive(false);
                m_upgradeWindow.SetActive(true);
            }
        }

        if (m_upgradeWindow && m_upgradeWindow.activeInHierarchy)
        {
            if (Input.GetButtonDown("BuyZ") && m_player.GearCount >= 2)
            {
                m_player.TurretCount++;
                m_player.TurretLimit++;
                m_player.GearCount -= 2;
            }
            if (Input.GetButtonDown("Throw") && m_player.GearCount >= 3)
            {
                if (m_player.TurretSpeed > 0.2f)
                {
                    m_player.TurretSpeed -= 0.1f;
                    m_player.GearCount -= 3;
                }
            }
            if (Input.GetButtonDown("Pickup") && m_player.GearCount >= 1)
            {
                m_player.PotionCount++;
                m_player.GearCount--;
            }
        }
    }

    private void OnEnable()
    {
        SelectMenu();
        m_inMenus = true;
    }

    private void OnDisable()
    {
        m_inMenus = false;
    }

    public void SelectMenu()
    {
        if (m_resumeButton)
            m_resumeButton.Select();
    }
}
