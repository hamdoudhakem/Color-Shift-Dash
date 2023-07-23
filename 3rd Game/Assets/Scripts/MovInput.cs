using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovInput : MonoBehaviour
{   
    [Tooltip("the input will start being considered only after the player swipe surpases this Value")]  [Range(0 , 2)]
    public float DeadZone;

    private PlayerMovement PM;
    //private PlayerMovement2 PM;
    private bool IsTouching;
    private float StartPosX;

    void Start()
    {
        PM = GetComponent<PlayerMovement>();
        //PM = GetComponent<PlayerMovement2>();
        IsTouching = false;
    }

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];

                if (touch.phase == TouchPhase.Began)
                {
                    IsTouching = true;
                    StartPosX = touch.position.x;
                }
                else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    IsTouching = false;
                }
            }            

            if (IsTouching)
            {
                Touch touch = Input.touches[0];

                float Dif = touch.position.x - StartPosX;

                StartPosX = touch.position.x;

                if (Mathf.Abs(Dif) > DeadZone)
                {
                    PM.Move(Dif);
                    //PM.ChangeVel(Dif);
                }

            }
        }       
    }
}
