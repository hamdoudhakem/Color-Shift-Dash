using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidesNavig : MonoBehaviour
{     
    [Tooltip("This is the Parent of The Slides that I Will Show. I Eather Give this a Value From The Inspector and Stick with it or change this value every time Through Script (Set Up Fonction)")]
    [SerializeField] private Transform CurSlides;
    [Tooltip("The Speed At which the Player Switches Between Slides")]
    public float Speed;
    [Tooltip("How Much The Slide at the Forefront will increase scale compared to others (Cant Do more than 2)")]
    [Range(0 , 2)] public float ScaleFactor;

    [Header("Features")]
    public GameObject NextBut;
    public GameObject PrevBut;

    [Space]

    [Tooltip("This objects is the one containing the Dots at the bottom which tells which slider I'm in")]
    public Transform SlidesIndexDots;
    public float UnusedDotsOpac;

    private int CurSlide;
    private bool Siding;
    private Transform LastSlide , NewSlide;
    private float DifBetwSlides;

    private Image[] IndexDots;

    public Vector3 OrigSildePos { get; private set; }
    private Vector3 TargetPos;

    private Vector3 LastSlideTargetScale, NewSlideTargetScale, OrigScale;

    private ISlideNavigCaller Caller;

    void Start()
    {
        Siding = false;

        if (CurSlides != null)
        {
            SetBaseVars(SlidesIndexDots);
        }
    }

    void Update()
    {
        if (Siding)
        {
            CurSlides.position = Vector3.Lerp(CurSlides.position, TargetPos, Speed);

            LastSlide.localScale = Vector3.Lerp(LastSlide.localScale, LastSlideTargetScale, Speed);

            NewSlide.localScale = Vector3.Lerp(NewSlide.localScale, NewSlideTargetScale, Speed);

            if ((CurSlides.position - TargetPos).magnitude < 3)
            {
                CurSlides.position = TargetPos;

                LastSlide.localScale = LastSlideTargetScale;

                NewSlide.localScale = NewSlideTargetScale;

                LastSlide.gameObject.SetActive(false);

                Siding = false;

                if (Caller != null)
                {
                    Caller.DoneSiding();
                }                
            }
        }
    }

    public void NextSlide()
    {
        if (CurSlide < CurSlides.childCount - 1)
        {
            CheckLastSlide();

            LastSlide = CurSlides.GetChild(CurSlide);
            NewSlide = CurSlides.GetChild(CurSlide + 1);
            NewSlide.gameObject.SetActive(true);

            TargetPos = new Vector3(OrigSildePos.x - (CurSlide + 1) * DifBetwSlides, CurSlides.position.y);
            NewSlideTargetScale = OrigScale;
            LastSlideTargetScale = OrigScale / ScaleFactor;

            CurSlide++;
            Siding = true;

            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            ActivateSwitchingButtons();

            UpdateIndexsDots(CurSlide - 1);
        }
    }

    public void PrevSlide()
    {
        if (CurSlide > 0)
        {
            CheckLastSlide();

            LastSlide = CurSlides.GetChild(CurSlide);
            NewSlide = CurSlides.GetChild(CurSlide - 1);
            NewSlide.gameObject.SetActive(true);

            TargetPos = new Vector3(OrigSildePos.x + (1 - CurSlide) * DifBetwSlides, CurSlides.position.y);
            NewSlideTargetScale = OrigScale;
            LastSlideTargetScale = OrigScale / ScaleFactor;

            CurSlide--;
            Siding = true;

            AudioManager.AudMan.Play("Switch Tuto Slide", UnPaused: true);

            ActivateSwitchingButtons();

            UpdateIndexsDots(CurSlide + 1);
        }

    }

    void CheckLastSlide()
    {
        //This Function is To Make sure that if the Player switches 2 times in a row the First Silde WILL
        //Disactivate and so the The Video will always Play when the player reaches it (On awake)

        if (LastSlide != null)
        {
            //The Second Condition is to see if i'am still using the Slides or changed
            //(exp from Gameplay Vids to Mechanic Vids) So i don't desactivate Last Slide
            if (LastSlide.gameObject.activeSelf && LastSlide.parent == CurSlides)
            {
                LastSlide.gameObject.SetActive(false);
            }
        }
    }

    public void SetUpNavig(ISlideNavigCaller caller, Transform CurSlides, Transform dots)
    {
        this.Caller = caller;
        this.CurSlides = CurSlides;

        SetBaseVars(dots);        

        Siding = false;
    }

    private void SetBaseVars(Transform dots)
    {
        OrigSildePos = CurSlides.position;
        OrigScale = CurSlides.GetChild(0).localScale;

        CurSlide = 0;

        if (CurSlides.childCount > 1)
        {
            DifBetwSlides = CurSlides.GetChild(1).position.x - CurSlides.GetChild(0).position.x;
        }

        SetUpDotIndexs(dots);

        SetUpButs();
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

    #region Set Up Fonctions (Set Indexe Dots, Set Buttons)

    void SetUpButs()
    {
        if (CurSlides.childCount == 1)
        {            
            NextBut.SetActive(false);
            PrevBut.SetActive(false);
        }
        else if (CurSlides.childCount > 1)
        {
            NextBut.SetActive(true);
            PrevBut.SetActive(false);
        }
    }

    public void SetUpDotIndexs(Transform Dots)
    {
        IndexDots = new Image[CurSlides.childCount];

        for (int i = 0; i < IndexDots.Length; i++)
        {
            IndexDots[i] = Dots.GetChild(i).GetComponent<Image>();
        }                     
    }

    #endregion

    #region Updating Features (Index Dots, Buttons ...)

    private void ActivateSwitchingButtons()
    {
        if (CurSlide == 0)
        {
            NextBut.SetActive(true);
            PrevBut.SetActive(false);
        }
        else if (CurSlide == CurSlides.childCount - 1)
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

    void UpdateIndexsDots(int PrevSlideIndex)
    {
        IndexDots[PrevSlideIndex].color = new Color(1, 1, 1, UnusedDotsOpac);

        IndexDots[CurSlide].color = Color.white;
    }

    #endregion
}
