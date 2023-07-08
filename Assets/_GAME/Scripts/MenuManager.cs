using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] bool m_OptionsOpen;

    [Header("References")]
    [SerializeField] GameObject m_GUIPanel;
    [SerializeField] GameObject m_MainMenuPanel, m_OptionsMenuPanel, m_PauseMenuPanel;


    public static Action GameStarted;
    public static MenuManager Instance;

    private void Awake() => Instance = this;

    public void StartGame()
    {
        m_MainMenuPanel.SetActive(false);
        m_GUIPanel.SetActive(true);
        Debug.Log("Game Started");
        GameStarted?.Invoke();
    }

    public void ToggleOptions()
    { 
        m_OptionsOpen =! m_OptionsOpen;
        m_OptionsMenuPanel.SetActive(m_OptionsOpen);
        Debug.Log("Options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public static class BooleanExtensions
{
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}
