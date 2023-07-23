using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public Camera Cam;
    [Tooltip("The Amount of friction applied after the player gets his hands of the screen")] [Range(1 , 20)]
    [SerializeField] private float FrictionValue;
    [Tooltip("If this much time Passes after the last movement i will start applying friction")]
    public float time;
    private float RemainingTime;
    public float ForwardSpeed , MaxSideSwipe , MaxSideSpeed;
    [Tooltip("The Speed Change Multiplier")]
    public float IncreaseRate;
    [Tooltip("How Much this Frame Side-Speed Gain represents Compared to the last one. if it's bigger than this pourcentage I will lower it")]
    [Range(1 , 2)]
    public float RelativeGain;
    [Tooltip("The Speed i will lose gradualy after reaching the Finish line")]
    public float StopRate;    
    [Tooltip("The Duraction of the Speed Boost (In Seconds)")]
    public float SpeedBoostTime;
    [Tooltip("How much the Camera view field will narrow when using a speed boost")]
    public float ViewChange;
    [Tooltip("THe Speed at which the Camera view field will change")]
    public float ViewChangeSpeed;
    public LayerMask GroundLayer;
    public Vector3 GroundedSize;

    [HideInInspector] public bool MoveForward, InputMove;
    private float LastFrameVel;    
    [HideInInspector] public Rigidbody rb;
    private bool StopSliding;
    private GameObject BoostEffect;

    private bool ChangeViewField;
    private float DefaultView, TargetView;

    void Start()
    {
        ChangeViewField = false;
        MoveForward = true;
        InputMove = true;
        StopSliding = false;

        LastFrameVel = int.MaxValue;
        DefaultView = Cam.fieldOfView;

        rb = GetComponent<Rigidbody>();
        BoostEffect = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (MoveForward)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, ForwardSpeed);
        }
       

        if(RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;

            if(RemainingTime < 0)
            {
                StopSliding = true;
            }
        }

        //Friction Simulation
        if (StopSliding)
        {
            //Debug.Log("Before friction : " + rb.velocity.x);
            rb.velocity -= Time.deltaTime * (rb.velocity.x / 5) * FrictionValue * Vector3.right;
            //Debug.Log("after friction  :" + rb.velocity.x);

            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                StopSliding = false;
            }
        }

        if (ChangeViewField)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, TargetView, ViewChangeSpeed);

            if (Mathf.Abs(Cam.fieldOfView - TargetView) < .2f)
            {
                ChangeViewField = false;
            }
        }
    }      

    public void ChangeVel(float DifInPixel)
    {
        if (InputMove)
        {
            Debug.Log("DifInPixel = " + DifInPixel);

            float Dif = Mathf.Clamp(DifInPixel, -MaxSideSwipe, MaxSideSwipe);

            if (rb.velocity.x < 0 && Dif > .75f || rb.velocity.x > 0 && Dif < -.75f)
            {
                rb.velocity = new Vector3(Dif * IncreaseRate * Time.deltaTime, rb.velocity.y, rb.velocity.z);
                Debug.Log("rb.velocity : " + rb.velocity);
            }
            else
            {
                //If the speed becomes 'RelativeGain' times or more superior to the last speed boost then i will make
                //it RelativeGain * LastSpeedboost instead So that i can prevent a super fast accelerations

                float vel = Dif * IncreaseRate * Time.deltaTime > RelativeGain * LastFrameVel ?
                    Dif * IncreaseRate * Time.deltaTime * RelativeGain : Dif * IncreaseRate * Time.deltaTime;

                rb.velocity += vel * Vector3.right;

                LastFrameVel = vel;
            }

            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxSideSpeed, MaxSideSpeed)
                                                                            , rb.velocity.y, rb.velocity.z);

            RemainingTime = time;
        }       
    }        

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.down * .5f, GroundedSize * 2);
    }

    public IEnumerator SpeedUp(float BoostValue, bool TakeInput = true)
    {
        //Boost Effects
        BoostEffect.SetActive(true);
        ChangeFieldOfView(DefaultView - ViewChange);       

        //We Will Change the Input Move and Forward Speed temporarely
        InputMove = TakeInput;
        ForwardSpeed += BoostValue;

        yield return new WaitForSeconds(SpeedBoostTime);

        yield return new WaitUntil(() =>
        {
            return Physics.OverlapBox(transform.position + Vector3.down * .5f, GroundedSize, new Quaternion(), GroundLayer).Length > 0;
        });

        ForwardSpeed -= BoostValue;
        InputMove = true;

        //Boost Effects
        BoostEffect.SetActive(false);
        ChangeFieldOfView(DefaultView);
    }

    private void ChangeFieldOfView(float ViewB)
    {
        TargetView = ViewB;
        ChangeViewField = true;
    }

    public IEnumerator Stop()
    {
        MoveForward = false;
        InputMove = false;
        yield return new WaitForEndOfFrame();

        //Stop The Player Gradually
        do
        {
            rb.velocity -= (rb.velocity.z / 10) * Time.deltaTime * StopRate * Vector3.forward;
            yield return new WaitForEndOfFrame();

        } while (rb.velocity.z > 0);

        rb.velocity = Vector3.zero;
    }
}
