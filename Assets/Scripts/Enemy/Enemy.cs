using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyIntroData m_introData = null;
    [SerializeField] bool m_playIntro = false;

    private void Start()
    {
        //m_intro.PlayIntro();
    }

    private void Update()
    {
        if (m_playIntro)
        {
            Intro.Instance.PlayIntro(m_introData);
            m_playIntro = false;
        }
    }
}
