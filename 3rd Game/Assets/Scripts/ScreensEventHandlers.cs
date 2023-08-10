using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensEventHandlers : MonoBehaviour
{
    public GameObject PauseMenu;
    public SettingsManager SettingsMan;

    void Start()
    {
        SettingsMan.LoadSettings();
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
        Time.timeScale = 1;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit_EventHandler()
    {
        Time.timeScale = 1;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(0);

        //while (!asyncOp.isDone)
        //{

        //}
    }
}
