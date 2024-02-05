using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using EZCameraShake;

public class PlayerMovement : MonoBehaviour
{
    public Camera Cam;
    [HideInInspector] public LensDistortion Lens;
    public ParticleSystem LandEff;

    [Header("Speed Values")]

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
    [Tooltip("how much the side speed will change each frame Going from a Speed Lv to another(using lerp so % based)")] [Range(0 , 1)]
    public float ChangeRate0To1, ChangeRate1To2;
    [Tooltip("How much time it will take for the player to stop when on the Finish line (1 is 1 second , 2 is 1/2 second)")] [Range(0 , 10f)]
    public float StopRate;

    [Header("Boosting Effects")]

    [Tooltip("The New Value I Will Set for The Lens Distortion Intensity when Speeding Up")]
    public float LensDisLv1;
    public float LensDisLv2;
    [Tooltip("The Speed At which the Lens Distortion will change it's value from one to another")] [Range(0,1)]
    public float LensDisChangeSpeed;
    [Tooltip("How much the Camera view field will narrow when using a speed boost")]
    public float ViewChange;
    [Tooltip("THe Speed at which the Camera view field will change")]
    public float ViewChangeSpeed;
    [Tooltip("How Much The Camera Will Go Back When Speeding up")]
    public float ZOffsetChange;
    [Tooltip("How Fast the Z Camera Offset Will Change")]
    public float ZOffsetChangeSpeed;


    [Space]
    public LayerMask GroundLayer;
    public Vector3 GroundedSize;

    private CameraShakeInstance Shake;

    [HideInInspector] public bool MoveForward, InputMove;
    [HideInInspector] public Rigidbody rb;
    private bool StopSliding;
    private GameObject BoostEffect;

    private int StackedBoosts;

    private bool ChangeLensDis, ChangeViewField, ChangeCamOffset;    
    private float DefaultView, TargetView , TargetLensDis, DefaultCamOffset, TargetCamOffset;
    private float DefaultForSpeed;
    private float LastTargetSpeed;
    private CameraMovement CM;

    private Collider[] Cols = new Collider[1];
    private bool Grounded , LastGrounded;
    private bool Boosted;

    void Start()
    {
        Boosted = false;
        Grounded = true;
        LastGrounded = true;
        LastTargetSpeed = 0;
        ChangeViewField = false;
        ChangeLensDis = false;
        MoveForward = true;
        InputMove = true;
        StopSliding = false;
        StackedBoosts = 0;
        DefaultForSpeed = ForwardSpeed;
     
        rb = GetComponent<Rigidbody>();
        BoostEffect = Cam.transform.GetChild(0).gameObject;
        CM = Cam.transform.parent.GetComponent<CameraMovement>();
        DefaultCamOffset = CM.Offset.z;

        DefaultView = Cam.fieldOfView;
        Lens = (LensDistortion)FindObjectOfType<Volume>().profile.components.Find(comp => comp is LensDistortion);

        LandEff = Instantiate(LandEff.gameObject, transform.position, Quaternion.Euler(Vector3.left * 90)).GetComponent<ParticleSystem>();
        LandEff.Stop();
    }

    void Update()
    {        
        if (!ScreensEventHandlers.IsPaused)
        {
            Grounded = Physics.OverlapBoxNonAlloc(transform.position + Vector3.down * .5f, GroundedSize, Cols, new Quaternion(), GroundLayer, QueryTriggerInteraction.Ignore) > 0;

            if (Grounded && rb.velocity.z > .3f)
            {
                if (!LastGrounded)
                {
                    AudioManager.AudMan.Play("Landed");
                    LandEff.transform.position = transform.position + Vector3.down * 0.4f;
                    LandEff.Play();
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

        #region Visual Effects For Speeding Up

        float Remaining;

        if (ChangeViewField)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, TargetView, ViewChangeSpeed);

            Remaining = Mathf.Abs(Cam.fieldOfView - TargetView);

            if (Remaining < .2f)
            {
                Cam.fieldOfView = TargetView;
                ChangeViewField = false;                
            }
            
            if((Remaining * 100 / ViewChange) <= 15 && TargetView == DefaultView)
            {
                BoostEffect.SetActive(false);
            }
        }

        if (ChangeLensDis)
        {
            Lens.intensity.value = Mathf.Lerp(Lens.intensity.value, TargetLensDis, LensDisChangeSpeed);

            Remaining = Mathf.Abs(Lens.intensity.value - TargetLensDis);

            if (Remaining < .06f)
            {
                Lens.intensity.value = TargetLensDis;
                ChangeLensDis = false;
            }                       
        }

        if (ChangeCamOffset)
        {
            float NewZ = Mathf.Lerp(CM.Offset.z, TargetCamOffset, ZOffsetChangeSpeed);

            CM.Offset = new Vector3(CM.Offset.x, CM.Offset.y, NewZ);

            Remaining = Mathf.Abs(TargetCamOffset - CM.Offset.z);

            if (Remaining < .5f)
            {
                CM.Offset = new Vector3(CM.Offset.x, CM.Offset.y, TargetCamOffset);
                ChangeCamOffset = false;
            }
        }

        #endregion
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

    #region Speed Up Related

    public IEnumerator SpeedUp(float BoostValue, float BoostTime, bool TakeInput, int BoostLv)
    {
        AudioManager.AudMan.Play("Boost" , true);
        AudioManager.AudMan.Stop("Deboost");

        //Boost Effects
        Boosted = true;
        StackedBoosts++;
        BoostEffect.SetActive(true);
        Shake = CameraShaker.Instance.StartShake(.5f, 2.5f, .3f);

        //Changing The Lens Distortion
        float LensDis = LensDisLv1;
        if(BoostLv == 2)
        {
            LensDis = LensDisLv2;
        }
        BoostingEffects(DefaultView + ViewChange, LensDis, DefaultCamOffset - ZOffsetChange);

        //We Will Change the Input Move and Forward Speed temporarely
        InputMove = TakeInput;
        ForwardSpeed += BoostValue;

        yield return new WaitForSeconds(BoostTime);

        //Wait Until I'm Either Grounded or Colliding Head first against Something (a problem)
        Collider[] cols = new Collider[1];
        yield return new WaitUntil(() =>
        {
            return Grounded || Physics.OverlapBoxNonAlloc(transform.position + Vector3.forward * .5f, GroundedSize, cols, Quaternion.Euler(90, 0, 0), GroundLayer, QueryTriggerInteraction.Ignore) > 0;
        });
        
        //AudioManager.AudMan.Stop("Boost");
        Debug.Log("I Got Here");
        AudioManager.AudMan.Play("Deboost");

        StackedBoosts--;
        ForwardSpeed -= BoostValue;
        InputMove = !TakeInput ? true : InputMove;

        //Cancel Boost Effects
        if(StackedBoosts == 0)
        {
            Boosted = false;
            BoostingEffects(DefaultView , 0,DefaultCamOffset);
            Shake.StartFadeOut(.5f);
        }
        
    }

    private void BoostingEffects(float TargetFieldOfView, float TargetLensDistortion, float TargetCamOffset)
    {
        this.TargetCamOffset = TargetCamOffset;
        TargetView = TargetFieldOfView;
        TargetLensDis = TargetLensDistortion;

        ChangeCamOffset = true;
        ChangeViewField = true;
        ChangeLensDis = true;
    }

    #endregion

    #region Finishing Or Dying Related

    public IEnumerator Stop()
    {
        rb.constraints |= RigidbodyConstraints.FreezeRotationX;
                
        MoveForward = false;
        InputMove = false;
        CancelAllEffects();
        BoostEffect.SetActive(false);
        BoostingEffects(DefaultView, 0,DefaultCamOffset);
        
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

    public void CancelAllEffects()
    {
        Lens.active = false;
        if(Shake != null)
        {
            Shake.StartFadeOut(0);
        }
    }

    #endregion
}
