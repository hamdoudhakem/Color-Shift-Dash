using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Basic Tuto Variables")]
    public Transform Player;
    [Tooltip("I Will Use this Object to start the Coresponding Tuto Assigned to the reached Point")]
    public Transform Tutos;
    public Transform TutorialCheckPo;

    [Header("Features Helping Tutos Navigating")]
      [Tooltip("This objects is the one containing the Dots at the bottom which tells which slider I'm in")]
    public Transform SlidesIndexDots;
    public GameObject NextBut, PrevBut, ExitBut;
      [Tooltip("How Much The Tutos Panels Will Increase Size when in forefront (Don't Increase More than 2)")] [Range(1, 2)]
    public float ScaleFactor;
      [Tooltip("The Speed At which I Will switch Slides")]
    public float Speed;

    [Header("Events")]
    [Tooltip("I Will Invoke this Event to set up the basics for showing the tutorial (black background, features ...)")]
    public UnityEvent GotInTuto;
    public UnityEvent GotOutTuto;

    private Transform CurTuto;
    private Transform CurPoint;
    private int CurPointIndex;
    private bool InTuto;
    private float DifBetwSlides;
    private Vector3 OrigTutoPos;

    [Tooltip("If I'm in the Process of Switching Slides")]
    private bool Siding;
    private Transform LastSlide, NewSlide;
    private Vector3 TargetPos, LastSlideTargetScale, NewSlideTargetScale;
    private int CurSlide;
    private Vector3 OrigScale;

    private float UnusedIndexsDotsOpacity;
    private Image CurIndexDot;
    private Image[] IndexDots;

    /// <summary>
    /// Every Point and it's the Coresponding Tuto (Slider UI) must have the Same getchild() Index
    /// (They Must have the same order if the Point if 2nd child the TuTo must Also Be 2nd Child)
    /// </summary>

    void Awake()
    {
        if (PlayerData.LvXTuToUsed[SceneManager.GetActiveScene().buildIndex])
        {
            Destroy(TutorialCheckPo.gameObject);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Siding = false;
        InTuto = false;
        CurSlide = 0;
        CurPointIndex = 0; 
        CurPoint = TutorialCheckPo.transform.GetChild(CurPointIndex);
    }

    void Update()
    {
        if (!InTuto)
        {

            if (Player.transform.position.z - CurPoint.position.z >= 0)
            {
                InTuto = true;

                GotInTuto.Invoke();

                CurTuto = Tutos.GetChild(CurPointIndex);

                CurTuto.gameObject.SetActive(true);

                OrigTutoPos = CurTuto.position;

                OrigScale = CurTuto.GetChild(0).localScale;

                if (CurTuto.childCount > 1)
                {
                    CurSlide = 0;
                    DifBetwSlides = CurTuto.GetChild(1).position.x - CurTuto.GetChild(0).position.x;                    
                }
                else
                {
                    NextBut.SetActive(false);
                }

                SetUpDotIndexs();

            }
        }

        if (Siding)
        {
            CurTuto.position = Vector3.Lerp(CurTuto.position, TargetPos, Speed);

            LastSlide.localScale = Vector3.Lerp(LastSlide.localScale, LastSlideTargetScale, Speed);

            NewSlide.localScale = Vector3.Lerp(NewSlide.localScale, NewSlideTargetScale, Speed);

            if((CurTuto.position - TargetPos).magnitude < 3)
            {
                CurTuto.position = TargetPos;

                LastSlide.localScale = LastSlideTargetScale;

                NewSlide.localScale = NewSlideTargetScale;

                LastSlide.gameObject.SetActive(false);

                Siding = false;

                if(CurSlide == CurTuto.childCount - 1)
                {
                    ExitBut.SetActive(true);
                }
            }
        }
    }

    private void SetUpDotIndexs()
    {
        IndexDots = new Image[CurTuto.childCount];

        for (int i = 0; i < CurTuto.childCount; i++)
        {
            IndexDots[i] = SlidesIndexDots.GetChild(i).GetComponent<Image>();

            IndexDots[i].gameObject.SetActive(true);
        }

        CurIndexDot = SlidesIndexDots.GetChild(0).GetComponent<Image>();

        UnusedIndexsDotsOpacity = IndexDots[1].color.a;
    }

    private void ActivateSwitchingButtons()
    {
        if(CurSlide == 0)
        {
            NextBut.SetActive(true);
            PrevBut.SetActive(false);
        }
        else if(CurSlide == CurTuto.childCount - 1)
        {
            PrevBut.SetActive(true);
            NextBut.SetActive(false);
        }
        else
        {
            PrevBut.SetActive(true);
            NextBut.SetActive(true);
        }
    }

    public void NextSlide()
    {
        if(CurSlide < CurTuto.childCount - 1)
        {
            LastSlide = CurTuto.GetChild(CurSlide);
            NewSlide = CurTuto.GetChild(CurSlide + 1);
            NewSlide.gameObject.SetActive(true);

            TargetPos = new Vector3(OrigTutoPos.x - (CurSlide + 1) * DifBetwSlides, CurTuto.position.y);
            NewSlideTargetScale = OrigScale;
            LastSlideTargetScale = OrigScale / ScaleFactor;

            CurSlide++;
            Siding = true;

            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            ActivateSwitchingButtons();

            UpdateIndexsDots();
        }
    }

    public void PrevSlide()
    {
        if (CurSlide > 0)
        {
            LastSlide = CurTuto.GetChild(CurSlide);
            NewSlide = CurTuto.GetChild(CurSlide - 1);
            NewSlide.gameObject.SetActive(true);

            TargetPos = new Vector3(OrigTutoPos.x + (1 - CurSlide) * DifBetwSlides, CurTuto.position.y);
            NewSlideTargetScale = OrigScale;
            LastSlideTargetScale = OrigScale / ScaleFactor;

            CurSlide--;
            Siding = true;

            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            ActivateSwitchingButtons();

            UpdateIndexsDots();
        }
    }

    void UpdateIndexsDots()
    {
        CurIndexDot.color = new Color(1, 1, 1, UnusedIndexsDotsOpacity);

        IndexDots[CurSlide].color = Color.white;

        CurIndexDot = IndexDots[CurSlide];
    }


    public void GoToSlide(int NewSlide)
    {
        int NewSlideIndex = NewSlide - 1;

        if (CurSlide < NewSlideIndex)
        {
            int IterationNum = NewSlideIndex - CurSlide;

            for (int i = 0; i < IterationNum; i++)
            {
                NextSlide();
            }
        }
        else if (CurSlide > NewSlideIndex)
        {
            int IterationNum = CurSlide - NewSlideIndex;

            for (int i = 0; i < IterationNum; i++)
            {
                PrevSlide();
            }
        }
    }

    public void ExitTuto()
    {
        GotOutTuto.Invoke();

        CurTuto.gameObject.SetActive(false);

        CurTuto.position = OrigTutoPos;

        CurTuto.GetChild(0).localScale = OrigScale;

        for(int i = 0; i < IndexDots.Length; i++)
        {
            IndexDots[i].gameObject.SetActive(false);
        }
    }
}
