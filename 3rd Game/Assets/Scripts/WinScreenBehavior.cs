using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WinScreenBehavior : MonoBehaviour
{
    public Transform Stars;
    [Tooltip("The Text Mesh Pro for the Money Won")]
    public TextMeshProUGUI MoneyCount;
    public int MoneyPerStar;
    [Tooltip("How Much Seconds it will take to reach the Money earned on this level on the money display")]
    [Range(1 , 5)]public int MoneyTime;
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
        Finished = false;
        IncreaseMoney = false;
        MoneyProgress = 0;
        MoneyGoal = PlayerInteractions.StarsNum * MoneyPerStar;
        remark = Remarks[PlayerInteractions.StarsNum - 1];

        StartCoroutine(DisplayStars());
    }

    IEnumerator DisplayStars()
    {
        //Showing The Stars
        for (int i = 0; i < PlayerInteractions.StarsNum; i++)
        {
            Stars.GetChild(i).gameObject.SetActive(true);

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
            }
            
        }
    }

    public void ShowAll()
    {
        if (!Finished)
        {
            Finished = true;
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

            Remark.text = remark;
        }
        
    }
}
