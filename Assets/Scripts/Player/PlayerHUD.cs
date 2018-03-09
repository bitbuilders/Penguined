using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Player m_player = null;
    [SerializeField] TextMeshProUGUI m_gearCount = null;
    [SerializeField] TextMeshProUGUI m_turretCount = null;

    private void Update()
    {
        int score = m_player.GearCount;
        if (m_gearCount.text != score.ToString())
        {
            m_gearCount.text = score.ToString();
            StartCoroutine(BlinkText(m_gearCount, 0.25f));
        }
        m_turretCount.text = m_player.TurretCount.ToString();
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
}
