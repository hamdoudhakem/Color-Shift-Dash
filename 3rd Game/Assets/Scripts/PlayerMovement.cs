using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera Cam;
    [Tooltip("The Amount of friction applied after the player gets his hands of the screen")]
    [Range(1, 20)]
    [SerializeField] private float FrictionValue;
    [Tooltip("If this much time Passes after the last movement i will start applying friction")]
    public float time;
    private float RemainingTime;
    public float ForwardSpeed;
    [Tooltip("How much I should swipe to go from Speed lv 1 to lv 2")]
    public float SwipeLv1;
    [Tooltip("How much I should swipe to go from Speed lv 1 to lv 2")]
    public float SwipeLv2;
    [Tooltip("The Side Speed Lv 1 and Lv 2")]
    public float  lv0Speed ,lv1Speed, lv2Speed;
    [Tooltip("how much the side speed will change each frame (using lerp so % based)")] [Range(0 , 1)]
    public float ChangeRate0To1, ChangeRate1To2;
    [Tooltip("How much time it will take for the player to stop when on the Finish line (1 is 1 second , 2 is 1/2 second)")] [Range(0 , 10f)]
    public float StopRate;
    [Tooltip("How much the Camera view field will narrow when using a speed boost")]
    public float ViewChange;
    [Tooltip("THe Speed at which the Camera view field will change")]
    public float ViewChangeSpeed;
    public LayerMask GroundLayer;
    public Vector3 GroundedSize;

    [HideInInspector] public bool MoveForward, InputMove;
    [HideInInspector] public Rigidbody rb;
    private bool StopSliding;
    private GameObject BoostEffect;

    private int StackedBoosts;
    private bool ChangeViewField;
    private float DefaultView, TargetView;
    private float DefaultForSpeed;
    private float LastTargetSpeed;

    private bool Grounded , LastGrounded;
    private bool Boosted;

    void Start()
    {
        Boosted = false;
        Grounded = true;
        LastGrounded = true;
        LastTargetSpeed = 0;
        ChangeViewField = false;
        MoveForward = true;
        InputMove = true;
        StopSliding = false;
        StackedBoosts = 0;
        DefaultForSpeed = ForwardSpeed;

        DefaultView = Cam.fieldOfView;

        rb = GetComponent<Rigidbody>();
        BoostEffect = Cam.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (!ScreensEventHandlers.IsPaused)
        {
            Grounded = Physics.OverlapBox(transform.position + Vector3.down * .5f, GroundedSize, new Quaternion(), GroundLayer, QueryTriggerInteraction.Ignore).Length > 0;

            if (Grounded && rb.velocity.z > .3f)
            {
                if (!LastGrounded)
                {
                    AudioManager.AudMan.Play("Landed");
                }

                AudioManager.AudMan.Play("Rolling");

            }
            else
            {
                AudioManager.AudMan.Stop("Rolling");
            }

            if(!Grounded && Boosted)
            {
                AudioManager.AudMan.Play("Air Resistance");
            }
            else
            {
                AudioManager.AudMan.Stop("Air Resistance");

            }

            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;

                if (RemainingTime < 0)
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

            LastGrounded = Grounded;
        }      
    }

    void FixedUpdate()
    {
        if (MoveForward)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, ForwardSpeed);
        }

        if (ChangeViewField)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, TargetView, ViewChangeSpeed);

            float Remaining = Mathf.Abs(Cam.fieldOfView - TargetView);

            if (Remaining < .2f)
            {
                ChangeViewField = false;                
            }
            
            if((Remaining * 100 / ViewChange) <= 15 && TargetView == DefaultView)
            {
                BoostEffect.SetActive(false);
            }
        }
    }

    public void Move(float Dif)
    {
        if (InputMove)
        {
            //Debug.Log("Dif = " + Dif);

            float speed = 0 , ChangeRate = 0;

            if (Mathf.Abs(Dif) < SwipeLv1)
            {
                speed = lv0Speed;

                //So that when I switch sides while on this speed i do it rapidly
                if(Mathf.Abs(LastTargetSpeed) == lv0Speed)
                {
                    ChangeRate = .7f;
                }
                else
                {
                    ChangeRate = Mathf.Min(ChangeRate0To1, ChangeRate1To2);
                }
                
            }
            else if (SwipeLv1 < Mathf.Abs(Dif) && Mathf.Abs(Dif) < SwipeLv2)
            {
                speed = lv1Speed;
                ChangeRate = ChangeRate0To1;
            }       
            else if (SwipeLv2 < Mathf.Abs(Dif))
            {
                speed = lv2Speed;
                ChangeRate = ChangeRate1To2;
            }           

            if (Dif < 0)
            {
                speed *= -1;
            }

            LastTargetSpeed = speed;

            float velX = Mathf.Abs(rb.velocity.x);

            if (Mathf.Abs(velX - speed) < .3f)
            {
                rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                //Debug.Log("Current rb.velocity.x = " + rb.velocity.x + " and ChangeRate is : " + ChangeRate);
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, speed, ChangeRate), rb.velocity.y, rb.velocity.z);
            }                       

            RemainingTime = time;

            //Debug.Log("After rb.velocity.x : " + rb.velocity.x + " Target is : " + speed);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.down * .5f, GroundedSize * 2);
    }

    public IEnumerator SpeedUp(float BoostValue, float BoostTime, bool TakeInput)
    {
        AudioManager.AudMan.Play("Boost" , true);
        AudioManager.AudMan.Stop("Deboost");

        //Boost Effects
        Boosted = true;
        StackedBoosts++;
        BoostEffect.SetActive(true);
        ChangeFieldOfView(DefaultView - ViewChange);

        //We Will Change the Input Move and Forward Speed temporarely
        InputMove = TakeInput;
        ForwardSpeed += BoostValue;

        yield return new WaitForSeconds(BoostTime);

        yield return new WaitUntil(() =>
        {
            return Physics.OverlapBox(transform.position + Vector3.down * .5f, GroundedSize, new Quaternion(), GroundLayer ,QueryTriggerInteraction.Ignore).Length > 0
            || Physics.OverlapBox(transform.position + Vector3.forward * .5f, GroundedSize, Quaternion.Euler(90, 0, 0), GroundLayer, QueryTriggerInteraction.Ignore).Length > 0;
        });

        //AudioManager.AudMan.Stop("Boost");
        AudioManager.AudMan.Play("Deboost");

        StackedBoosts--;
        ForwardSpeed -= BoostValue;
        InputMove = !TakeInput ? true : InputMove;

        //Cancel Boost Effects
        if(StackedBoosts == 0)
        {
            Boosted = false;            
            ChangeFieldOfView(DefaultView);
        }
        
    }

    private void ChangeFieldOfView(float ViewB)
    {
        TargetView = ViewB;
        ChangeViewField = true;
    }

    public IEnumerator Stop()
    {
        rb.constraints |= RigidbodyConstraints.FreezeRotationX;
             
        MoveForward = false;
        InputMove = false;
        BoostEffect.SetActive(false);
        ChangeFieldOfView(DefaultView);        
        yield return new WaitForEndOfFrame();

        rb.velocity = new Vector3(0, rb.velocity.y, DefaultForSpeed);

        //Stop The Player Gradually
        do
        {
            rb.velocity += rb.velocity.z * Time.fixedDeltaTime * StopRate * Vector3.back;
            yield return new WaitForFixedUpdate();

        } while (rb.velocity.z > 0.3f);

        rb.velocity = Vector3.zero;
    }
}
