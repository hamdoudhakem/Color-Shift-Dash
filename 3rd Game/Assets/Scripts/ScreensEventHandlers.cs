using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensEventHandlers : MonoBehaviour
{
    public GameObject PauseMenu;
    public SettingsManager SettingsMan;
    public Animator LoadScreen;
    [Tooltip("How Much Time after the scene loaded to Disable the LoadScreen (in seconds)")]
    public float LoadTime;

    private bool LoadingScene;

    void Start()
    {
        SettingsMan.LoadSettings();
        LoadingScene = false;
        Invoke("Disable", LoadTime);
    }

    void Disable()
    {
        LoadScreen.gameObject.SetActive(false);
    }

    public void Pause_EventHandler()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume_EventHandler()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel_EventHandler()
    {
        if (!LoadingScene)
        {
            Time.timeScale = 1;
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            StartLoadScreen();

            LoadingScene = true;
        }
       
    }

    public void NextLevel_EventHandler()
    {
        if (!LoadingScene)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            StartLoadScreen();

            LoadingScene = true;
        }
    }

    public void Quit_EventHandler()
    {
        if (!LoadingScene)
        {
            Time.timeScale = 1;
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(0);
            StartLoadScreen();

            LoadingScene = true;
        }              
    }

    void StartLoadScreen()
    {
        LoadScreen.gameObject.SetActive(true);
        LoadScreen.SetTrigger("Appear");
    }

}
