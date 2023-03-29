using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovInput : MonoBehaviour
{
    private PlayerMovement PM;
    private bool IsTouching;
    private float StartPosX;

    void Start()
    {
        PM = GetComponent<PlayerMovement>();
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

                // if(Mathf.Abs(Dif) > .4f)
                // {
                PM.ChangeVel(Dif);
                // }            
            }
        }       
    }
}
