using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

public class AdsDisplayer : MonoBehaviour, IAdCallBack
{
    private static int RemainGames = -1;
       
    [Tooltip("what is the lowest number of Games that after being played I will launch an ad")]
    public int MinGame;
    [Tooltip("what is the Highest number of Games that after being played I will launch an ad")]
    public int MaxGame;

    [Header("Events")]
    public UnityEvent Pause;
    public UnityEvent UnPause;

    void Start()
    {
        //First Inisialization
        if(RemainGames == -1)
        {
            RemainGames = Random.Range(MinGame, MaxGame);
        }

        RemainGames--;

        //Condition for launching an Ad
        if(RemainGames == 0)
        {            
            AdsManager.StartAd(AdTypes.Interstitial_Android, this);
            Pause.Invoke();

            RemainGames = -1;
        }
    }

    public void ResumeGame()
    {
        UnPause.Invoke();
    }

    public void Reward() { }
   
}
