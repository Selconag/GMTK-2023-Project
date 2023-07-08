using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action LevelFail, LevelSucces;
    private static GameManager _Instance;

    private GameManager() { }

    public static GameManager Instance
    {
        get { return _Instance; }
    }
    private void Awake() => _Instance = this;

    void Start()
    {
        LevelFail += ReLoadScene;
        LevelSucces += LoadNextScene;
    }

    private void OnDestroy()
    {
        LevelFail -= ReLoadScene;
        LevelSucces -= LoadNextScene;
    }

    #region Scene Management
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region General Game

    private void ExitGame()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        //ExitSave();
    }
    #endregion
}
