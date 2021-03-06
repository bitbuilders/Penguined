﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : Singleton<PlayerHUD>
{
    [SerializeField] Player m_player = null;
    [SerializeField] TextMeshProUGUI m_gearCount = null;
    [SerializeField] TextMeshProUGUI m_turretCount = null;
    [SerializeField] TextMeshProUGUI m_infoText = null;
    [SerializeField] TextMeshProUGUI m_pickupText = null;
    [SerializeField] GameObject m_infoPanel = null;

    Image m_panel;
    float m_maxFade;

    private void Start()
    {
        m_panel = m_infoPanel.GetComponent<Image>();
        m_maxFade = m_panel.color.a;
    }

    private void Update()
    {
        int score = m_player.GearCount;
        if (m_gearCount.text != score.ToString())
        {
            m_gearCount.text = score.ToString();
            StartCoroutine(BlinkText(m_gearCount, 0.25f));
        }
        m_turretCount.text = m_player.TurretCount.ToString();
        m_pickupText.gameObject.SetActive(false);
    }

    IEnumerator BlinkText(TextMeshProUGUI text, float duration)
    {
        float startingFont = text.fontSize;
        float endingFont = startingFont + 30.0f;
        for (float i = 0.0f; i <= duration / 2; i += Time.deltaTime)
        {
            float amount = endingFont - startingFont;
            float size = amount * (i / duration);
            text.fontSize = startingFont + size;
            yield return null;
        }

        for (float i = duration / 2; i >= 0.0f; i -= Time.deltaTime)
        {
            float amount = endingFont - startingFont;
            float size = amount * (i / duration);
            text.fontSize = startingFont + size;
            yield return null;
        }

        text.fontSize = startingFont;
    }

    public void SetInfoText(string text, float duration)
    {
        m_infoText.text = text;
        StopCoroutine("FadeInfo");
        StartCoroutine(FadeInfo(duration));
        m_infoPanel.SetActive(true);
    }

    IEnumerator FadeInfo(float duration)
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            m_infoText.alpha = i;
            if (i <= m_maxFade)
                m_panel.color = new Color(m_panel.color.r, m_panel.color.g, m_panel.color.b, i);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        for (float i = 1.0f; i >0; i -= Time.deltaTime)
        {
            m_infoText.alpha = i;
            if (i <= m_maxFade)
                m_panel.color = new Color(m_panel.color.r, m_panel.color.g, m_panel.color.b, i);
            yield return null;
        }
        m_infoPanel.SetActive(false);
    }

    public void EnablePickupText()
    {
        m_pickupText.gameObject.SetActive(true);

    }
}
