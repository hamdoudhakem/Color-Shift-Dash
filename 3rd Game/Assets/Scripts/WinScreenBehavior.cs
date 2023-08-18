using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class WinScreenBehavior : MonoBehaviour
{
    public Transform Stars;
    [Header("Money Related")]
    [Tooltip("The Text Mesh Pro for the Money Won")]
    public TextMeshProUGUI MoneyCount;
    public int MoneyPerStar;
    [Tooltip("How Much Seconds it will take to reach the Money earned on this level on the money display")]
    [Range(1 , 5)]public int MoneyTime;

    [Header("Performance")]
    [Tooltip("The Text Mesh Pro for the Note that I will give the player (good, passable...)")]
    public TextMeshProUGUI Remark;
    [Tooltip("This Array represents the possible remarks for the Player performance and they are sorted (exp: if he has 2 stars it's index need's to be 1)")]
    public string[] Remarks;

    [Tooltip("How Much Time between each star appearance (in seconds)")]
    public float StarInterval;
    [Tooltip("How Much Time between each character is written (in seconds)")]
    public float TypingInterval;

    private bool IncreaseMoney;
    private float MoneyProgress;
    private int MoneyGoal;
    private string remark;
    private bool Finished;

    void Start()
    {
        AudioManager.AudMan.Play("Won");
        Finished = false;
        IncreaseMoney = false;
        MoneyProgress = 0;
        remark = Remarks[PlayerInteractions.StarsNum - 1];

        int StarDif = PlayerInteractions.StarsNum - PlayerData.LvXStars[SceneManager.GetActiveScene().buildIndex];

        if(StarDif > 0)
        {
            MoneyGoal = StarDif * MoneyPerStar;
        }
        else
        {
            MoneyGoal = 30;
        }        

        UpdateData();
        StartCoroutine(DisplayStars());
    }

    IEnumerator DisplayStars()
    {
        yield return new WaitForSeconds(StarInterval);

        //Showing The Stars
        for (int i = 0; i < PlayerInteractions.StarsNum; i++)
        {
            Stars.GetChild(i).gameObject.SetActive(true);
            AudioManager.AudMan.Play("Star" , true);

            yield return new WaitForSeconds(StarInterval);            
        }

        //Started Showing The Money
        MoneyCount.gameObject.SetActive(true);

        IncreaseMoney = true;        

        StartCoroutine(ShowTheRest());
    }

    IEnumerator ShowTheRest()
    {
        if(PlayerInteractions.StarsNum < 3)
        {
            Remark.text = "   ";
        }                

        for (int i = 0; i < remark.Length; i++)
        {
            Remark.text += remark[i];

            yield return new WaitForSeconds(TypingInterval);
        }
    }

    void Update()
    {
        if (IncreaseMoney)
        {
            if (MoneyProgress <= MoneyGoal)
            {
                float num = MoneyGoal/MoneyTime * Time.deltaTime;

                MoneyProgress += num;

                MoneyCount.text = "+" + MoneyProgress.ToString("0");
            }
            else
            {
                MoneyCount.text = "+" + MoneyGoal.ToString("0");

                IncreaseMoney = false;

                Finished = true;
            }
            
        }
    }

    public void ShowAll()
    {
        if (!Finished)
        {
            Finished = true;

            AudioManager.AudMan.Play("Star", true);
            StopAllCoroutines();
            IncreaseMoney = false;

            for (int i = 0; i < PlayerInteractions.StarsNum; i++)
            {
                Stars.GetChild(i).gameObject.SetActive(true);
            }

            MoneyCount.gameObject.SetActive(true);
            MoneyCount.text = "+" + MoneyGoal.ToString("0");

            if (PlayerInteractions.StarsNum < 3)
            {
                Remark.text = "   ";
            }

            Remark.text += remark;
        }
        
    }

    void UpdateData()
    {
        int CurLv = SceneManager.GetActiveScene().buildIndex;

        PlayerData.CollectedStarsIndex[CurLv - 1] = PlayerInteractions.IndexsOfObtainedStars;

        PlayerData.Money += MoneyGoal;

        if (PlayerData.LvXStars[CurLv] < PlayerInteractions.StarsNum)
        {
            PlayerData.LvXStars[CurLv] = PlayerInteractions.StarsNum;
        }

        //Check if i'm not on the last lv and if this is the latest unlocked level
        if (PlayerData.CurrentLv == CurLv)
        {
            if (PlayerData.CurrentLv != SceneManager.sceneCountInBuildSettings)
            {
                PlayerData.CurrentLv++;
                PlayerData.LvXStars.Add(PlayerData.CurrentLv, 0);
            }                   
        }

        SaveSystem.Save();
    }
}
