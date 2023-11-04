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
    /// 1 - Every Point and it's the Coresponding Tuto (Slider UI) must have the Same getchild() Index
    /// (They Must have the same order if the Point is 2nd child the TuTo must Also Be the 2nd Child)
    /// 2 - The 2nd, 3rd and so on Tuto Sliders (Videos) must be smaller than the First one for  
    /// the scaling up and down effect to work when switching between them 
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
        UnusedIndexsDotsOpacity = -1;
        CurPoint = TutorialCheckPo.GetChild(CurPointIndex);
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

                CurSlide = 0;

                if (CurTuto.childCount > 1)
                {
                    DifBetwSlides = CurTuto.GetChild(1).position.x - CurTuto.GetChild(0).position.x;                    
                }               

                SetUpDotIndexs();

                SetUpSwitchingButs();                
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

    public void NextSlide()
    {
        if(CurSlide < CurTuto.childCount - 1)
        {
            CheckLastSlide();

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
            CheckLastSlide();

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
  

    #region Setting Up and Updating Features (Buttons, Indexes Dots ...)

    void SetUpSwitchingButs()
    {
        if(CurTuto.childCount == 1)
        {
            NextBut.SetActive(false);
            PrevBut.SetActive(false);
        }
        else if (CurTuto.childCount > 1)
        {
            NextBut.SetActive(true);
            PrevBut.SetActive(false);
        }
    }

    private void ActivateSwitchingButtons()
    {
        if (CurSlide == 0)
        {
            NextBut.SetActive(true);
            PrevBut.SetActive(false);
        }
        else if (CurSlide == CurTuto.childCount - 1)
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

    private void SetUpDotIndexs()
    {
        if(IndexDots == null)
        {
            //This Is In case I Still Haven't Seen the Tutorial
            IndexDots = new Image[CurTuto.childCount];

            for (int i = 0; i < CurTuto.childCount; i++)
            {
                IndexDots[i] = SlidesIndexDots.GetChild(i).GetComponent<Image>();

                IndexDots[i].gameObject.SetActive(true);
            }

            CurIndexDot = SlidesIndexDots.GetChild(0).GetComponent<Image>();
                        
            UnusedIndexsDotsOpacity = IndexDots[1].color.a;
            
        }       
        else
        {
            //This is in case I'm seeing the tutorial AGAIN
            for (int i = 1; i < CurTuto.childCount; i++)
            {               
                IndexDots[i].gameObject.SetActive(true);

                IndexDots[i].color = new Color(1, 1, 1, UnusedIndexsDotsOpacity);
            }

            IndexDots[0].color = Color.white;

            CurIndexDot = IndexDots[0];
        }
    } 

    void UpdateIndexsDots()
    {
        CurIndexDot.color = new Color(1, 1, 1, UnusedIndexsDotsOpacity);

        IndexDots[CurSlide].color = Color.white;

        CurIndexDot = IndexDots[CurSlide];
    }

    void CheckLastSlide()
    {
        //This Function is To Make sure that if the Player switches 2 times in a row the First Silde WILL
        //Disactivate and so the The Video will always Play when the player reaches it (On awake)

        if (LastSlide != null)
        {
            if (LastSlide.gameObject.activeSelf)
            {
                LastSlide.gameObject.SetActive(false);
            }
        }
    }

    #endregion

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

        for(int i = 1; i < IndexDots.Length; i++)
        {
            IndexDots[i].gameObject.SetActive(false);
        }

        CurPointIndex++;

        if(CurPointIndex < TutorialCheckPo.childCount)
        {
            CurPoint = TutorialCheckPo.GetChild(CurPointIndex);
        }

        InTuto = false;
    }
}
