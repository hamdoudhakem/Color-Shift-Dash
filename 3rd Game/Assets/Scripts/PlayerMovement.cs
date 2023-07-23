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
    public float lv0Speed ,lv1Speed, lv2Speed;
    [Tooltip("How fast the speed will change from lv 1 to lv 2 and vice-versa. Make it big something like lv1Speed / 2")] [Range(0 , 50)]
    public float ChangeRate;
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

    void Start()
    {
        ChangeViewField = false;
        MoveForward = true;
        InputMove = true;
        StopSliding = false;
        StackedBoosts = 0;
        DefaultForSpeed = ForwardSpeed;

        DefaultView = Cam.fieldOfView;

        rb = GetComponent<Rigidbody>();
        BoostEffect = transform.GetChild(0).gameObject;
    }

    void Update()
    {      
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

        if (ChangeViewField)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, TargetView, ViewChangeSpeed);

            if (Mathf.Abs(Cam.fieldOfView - TargetView) < .2f)
            {
                ChangeViewField = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (MoveForward)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, ForwardSpeed);
        }
    }

    public void Move(float Dif)
    {
        if (InputMove)
        {
            //Debug.Log("Dif = " + Dif);

            float speed = 0;

            if (Mathf.Abs(Dif) < SwipeLv1)
            {
                speed = lv0Speed;
            }
            else if (SwipeLv1 < Mathf.Abs(Dif) && Mathf.Abs(Dif) < SwipeLv2)
            {
                speed = lv1Speed;
            }
            else if (SwipeLv2 < Mathf.Abs(Dif))
            {
                speed = lv2Speed;
            }

            if (Dif < 0)
            {
                speed *= -1;
            }

            float velX = Mathf.Abs(rb.velocity.x);

            if (Mathf.Abs(velX - speed) < .1f)
            {
                rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, speed, ChangeRate), rb.velocity.y, rb.velocity.z);
                //Debug.Log("I changed Progressivly cause : " + Mathf.Abs(velX - speed) + " is > .1");
            }

            //Debug.Log("Current X Velocity = " + rb.velocity.x);

            RemainingTime = time;

            // Debug.Log("rb.velocity.x : " + rb.velocity.x);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.down * .5f, GroundedSize * 2);
    }

    public IEnumerator SpeedUp(float BoostValue, float BoostTime, bool TakeInput)
    {
        //Boost Effects
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

        StackedBoosts--;
        ForwardSpeed -= BoostValue;
        InputMove = !TakeInput ? true : InputMove;

        //Cancel Boost Effects
        if(StackedBoosts == 0)
        {
            BoostEffect.SetActive(false);
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
        MoveForward = false;
        InputMove = false;
        BoostEffect.SetActive(false);
        ChangeFieldOfView(DefaultView);        
        yield return new WaitForEndOfFrame();

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, DefaultForSpeed);

        //Stop The Player Gradually
        do
        {
            rb.velocity += rb.velocity.z * Time.deltaTime * StopRate * Vector3.back;
            yield return new WaitForEndOfFrame();

        } while (rb.velocity.z > 0.3f);

        rb.velocity = Vector3.zero;
    }
}
