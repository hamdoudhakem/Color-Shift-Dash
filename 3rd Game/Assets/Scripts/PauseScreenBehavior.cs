using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehavior : MonoBehaviour
{
    public void Pause_EventHandler()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void Resume_EventHandler()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
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
