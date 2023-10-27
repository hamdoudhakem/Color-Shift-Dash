using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeLvs : MonoBehaviour
{
    public Transform Sliders;
    [Tooltip("The Speed at which The Sliders will position Themselves correctly")] [Range(0 , 1)]
    public float SidingSpeed;
    [Tooltip("This represents how much of the Screen should I swipe to get to the next slider (2 means half the screen, 3 Third of it..)")]
    [Range(0 , 10)] public float SwipeLenght;

    private float LeftLimit, RightLimit;
    private float SliderDif;
    private List<float> SlidersPos;
    private Vector2 StartPos, UpdatingPos, TargetSlider;
    private bool Touching, AlreadySet = false , Siding;     

    void Start()
    {
        if (!AlreadySet)
        {
            AlreadySet = true;

            SliderDif = Sliders.GetChild(1).position.x - Sliders.GetChild(0).position.x;
            LeftLimit = Sliders.position.x - (SliderDif * (Sliders.childCount - 1));
            RightLimit = Sliders.position.x;

            SaveEachSliderPos();

            //Debug.Log("Left Limit = " + LeftLimit + " Right Limit = " + RightLimit);

        }

        Siding = false;
    }  

    void Update()
    {
        if (!Siding)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    Touching = true;
                    StartPos = Input.touches[0].position;
                    UpdatingPos = StartPos;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled ||
                               Input.touches[0].phase == TouchPhase.Ended)
                {
                    GoToOverwhelmingSide();
                    Touching = false;
                }
            }

            if (Touching)
            {
                Touch touch = Input.touches[0];

                float dif = UpdatingPos.x - touch.position.x;

                if (Mathf.Abs(StartPos.x - touch.position.x) >= Screen.width / SwipeLenght)
                {
                    Touching = false;

                    float NewDif = (Screen.width / SwipeLenght) - Mathf.Abs(StartPos.x - UpdatingPos.x);
                    dif = dif > 0 ? NewDif : NewDif * -1;
                }

                float SliderMove = (dif * SliderDif / (Screen.width / SwipeLenght)) * -1;

                float NewXVal = Mathf.Clamp(Sliders.position.x + SliderMove, LeftLimit, RightLimit);

                Sliders.position = new Vector3(NewXVal, Sliders.position.y);

                UpdatingPos = touch.position;             
            }
        }
        else
        {
            Sliders.position = Vector3.Lerp(Sliders.position, TargetSlider, SidingSpeed);

            if(Mathf.Abs(Sliders.position.x - TargetSlider.x) < 7)
            {
                Sliders.position = TargetSlider;
                Siding = false;
            }
        }
       
    }

    private void GoToOverwhelmingSide()
    {
        float LimitSiding = SliderDif / 2;
        int index = 0;

        for (int i = 1; i < Sliders.childCount; i++)
        {
            float val = Mathf.Abs(Sliders.position.x - SlidersPos[i]);

            if (val < LimitSiding)
            {
                index = i;
                break;
            }            
        }

        TargetSlider = new Vector2(SlidersPos[index] , Sliders.position.y);
        Siding = true;

        if (Mathf.Abs(Sliders.position.x - TargetSlider.x) >= 20)
        {
            MainMenuAudioMan.MaAud.SwitchLvTab.Play();
        }

    }

    private void SaveEachSliderPos()
    {
        SlidersPos = new List<float>(Sliders.childCount);

        for (int i = 0; i < Sliders.childCount; i++) 
        {
            SlidersPos.Add(Sliders.position.x - (SliderDif * i));
        }
    }

    public void ExitLvs()
    {
        //Cause When I touch the Return Button it considers it a touching
        Touching = false;
        gameObject.SetActive(false);

        MainMenuAudioMan.MaAud.Click.Play();
    }
}
