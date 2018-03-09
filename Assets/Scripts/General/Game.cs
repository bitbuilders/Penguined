using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject m_pauseMenu = null;
    [SerializeField] GameObject m_helpWindow = null;

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !m_helpWindow.activeInHierarchy)
        {
            SwitchPauseMode();
        }
    }

    public void SwitchPauseMode()
    {
        if (m_pauseMenu.activeInHierarchy)
        {
            UnPauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void UnPauseGame()
    {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
