using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Intro : Singleton<Intro>
{
    [SerializeField] Camera m_mainCamera = null;
    [SerializeField] Camera m_introCamera = null;
    [SerializeField] GameObject m_introCanvas = null;
    [SerializeField] GameObject m_bar = null;
    [SerializeField] TextMeshProUGUI m_title = null;
    [SerializeField] TextMeshProUGUI m_description = null;
    [SerializeField] GameObject m_descriptionHolder = null;
    [SerializeField] Transform m_startPosition = null;
    [SerializeField] Transform m_endPosition = null;
    [SerializeField] Transform m_titleStart = null;
    [SerializeField] Transform m_titleEnd = null;
    [SerializeField] Transform m_barStart = null;
    [SerializeField] Transform m_barEnd = null;
    [SerializeField] Transform m_descriptionStart = null;
    [SerializeField] Transform m_descriptionEnd = null;

    EnemyIntroData m_introData;
    bool m_isPlaying = false;

    public bool IsPlaying { get { return m_isPlaying; } }

    public void PlayIntro(EnemyIntroData data)
    {
        // MAKE SURE WHEN PLAYING SOUND USE GLOBAL BECAUSE INTRO CAMERA ISN'T AUDIO LISTENER
        m_mainCamera.enabled = false;
        m_introCamera.enabled = true;
        m_isPlaying = true;
        m_introData = data;
        m_title.text = m_introData.title;
        m_description.text = m_introData.shortDescription;
        GameObject firstEnemy = Instantiate(m_introData.enemies[0], m_startPosition.position, Quaternion.identity, m_introCanvas.transform);
        StartCoroutine(HandleIntroSequence(firstEnemy));
    }

    IEnumerator HandleIntroSequence(GameObject enemy)
    {
        for (float i = m_introData.duration; i > 0.0f; i -= Time.deltaTime)
        {
            float time = m_introData.duration - i;
            float t = time / m_introData.duration;
            t = Mathf.Clamp01(t);
            float interp = Interpolation.ExpoInOut(t);
            enemy.transform.position = Vector3.LerpUnclamped(m_startPosition.position, m_endPosition.position, interp);

            m_title.transform.position = Vector3.LerpUnclamped(m_titleStart.position, m_titleEnd.position, interp);
            m_bar.transform.position = Vector3.LerpUnclamped(m_barStart.position, m_barEnd.position, interp);
            m_descriptionHolder.transform.position = Vector3.LerpUnclamped(m_descriptionStart.position, m_descriptionEnd.position, interp);
            
            yield return null;
        }

        m_mainCamera.enabled = true;
        m_introCamera.enabled = false;
        m_isPlaying = false;
        Destroy(enemy);
    }
}
