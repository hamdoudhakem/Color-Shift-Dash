using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour, ISlideNavigCaller
{
    [Header("Basic Tuto Variables")]
    public Transform Player;
    [Tooltip("I Will Use this Object to start the Coresponding Tuto Assigned to the reached Point")]
    public Transform Tutos;
    public float[] TutorialStartPoints;

    [Header("Features Helping Tutos Navigating")]
      [Tooltip("This objects is the one containing the Dots at the bottom which tells which slider I'm in")]
    public Transform SlidesIndexDots;
    public GameObject ExitBut;

    [Header("Events")]
    [Tooltip("I Will Invoke this Event to set up the basics for showing the tutorial (black background, features ...)")]
    public UnityEvent GotInTuto;
    public UnityEvent GotOutTuto;

    private SlidesNavig SN;
    private Transform CurTuto;
    private int CurPointIndex;
    private bool InTuto;  

    private int CurSlide;   

    private float UnusedIndexsDotsOpacity;
    private Image[] IndexDots;

    /// <summary>
    /// 1 - Every Point and it's Coresponding Tuto (Slider UI) must have the Same getchild() Index
    /// (They Must have the same order if the Point is 2nd in the array the TuTo must Also Be the 2nd Child)
    /// 2 - The 2nd, 3rd and so on Tuto Sliders (Videos) must be smaller than the First one for  
    /// the scaling up and down effect to work when switching between them 
    /// 3 - Each Lv that Have a Tutorial Needs To Be added to the Player Data "LvXTutoUsed"
    /// </summary>

    void Awake()
    {
        if (PlayerData.LvXTuToUsed[SceneManager.GetActiveScene().buildIndex])
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InTuto = false;
        CurSlide = 0;
        CurPointIndex = 0;
        UnusedIndexsDotsOpacity = -1;
        SN = GetComponent<SlidesNavig>();
    }

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            if (!InTuto)
            {
                if (Player.transform.position.z - TutorialStartPoints[CurPointIndex] >= 0)
                {
                    InTuto = true;

                    GotInTuto.Invoke();

                    CurTuto = Tutos.GetChild(CurPointIndex);

                    CurTuto.gameObject.SetActive(true);

                    CurSlide = 0;

                    if(CurTuto.childCount == 1)
                    {
                        ExitBut.SetActive(true);
                    }

                    SN.SetUpNavig(this, CurTuto, SlidesIndexDots);

                    SetUpDotIndexs();                   
                }
            }
        }
    }

    public void DoneSiding()
    {
        if (CurSlide == CurTuto.childCount - 1)
        {
            ExitBut.SetActive(true);
        }
    }

    private void SetUpDotIndexs()
    {
        //This Is In case I Havn't Already Set The IndexDots
        if (IndexDots == null)
        {
            IndexDots = new Image[CurTuto.childCount];

            for (int i = 0; i < CurTuto.childCount; i++)
            {
                IndexDots[i] = SlidesIndexDots.GetChild(i).GetComponent<Image>();

                IndexDots[i].gameObject.SetActive(true);
            }

            if (IndexDots.Length > 1)
            {
                UnusedIndexsDotsOpacity = IndexDots[1].color.a;
            }
        }
        else
        {
            //This is in case I'm Coming back to a Slides Group a second Time

            for (int i = 0; i < CurTuto.childCount; i++)
            {
                IndexDots[i].gameObject.SetActive(true);

                IndexDots[i].color = new Color(1, 1, 1, UnusedIndexsDotsOpacity);
            }

            IndexDots[0].color = Color.white;
        }
    }

    #region Event Handlers (nex But, Prev Button, ...)

    public void NextSlide()
    {
        if(CurSlide < CurTuto.childCount - 1)
        {
            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            SN.NextSlide();
            CurSlide++;                 
        }
    }

    public void PrevSlide()
    {
        if (CurSlide > 0)
        {
            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            SN.PrevSlide();
            CurSlide--;
        }
    }  

    public void GoToSlide(int NewSlide)
    {
        SN.GoToSlide(NewSlide);
        CurSlide = NewSlide - 1;  
    }

    public void ExitTuto()
    {
        GotOutTuto.Invoke();

        CurTuto.gameObject.SetActive(false);

        ExitBut.SetActive(false);

        for(int i = 0; i < IndexDots.Length; i++)
        {
            IndexDots[i].gameObject.SetActive(false);
        }

        CurPointIndex++;

        //This means that The Tutorial Ended
        if(CurPointIndex >= TutorialStartPoints.Length)
        {
            gameObject.SetActive(false);

            PlayerData.LvXTuToUsed[SceneManager.GetActiveScene().buildIndex] = true;

            SaveSystem.Save();
        }       

        InTuto = false;
    }

    #endregion

   
}
