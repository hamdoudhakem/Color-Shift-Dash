using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBallBehavior : MonoBehaviour
{
    [Tooltip("The Cannon Ball Destruction particle effect")]
    public GameObject CanBallDesEffect;
    [Tooltip("The Distance between the Player and the Cannon Ball at which the Cannon Ball is gonna self destroy")]
    public float PlayerDis;
    [Tooltip("How Much Times passes between each Distance Check (in seconds)")]
    public float CheckRate;

    [Tooltip("This Tells if this can Ball Should be Desactivated or destroyed by the end")]
    [HideInInspector] public bool NeededCanBall;
    [Tooltip("The Speed at which the Cannon Ball Goes")]
    [HideInInspector] public float Speed;
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    [HideInInspector] public float MaxDistance;
    [HideInInspector] public Transform Player;

    private Rigidbody rb;
    private float StartZ;
    private bool AlreadyDoneFor;

    public void Set()
    {
        StartZ = transform.position.z;
        AlreadyDoneFor = false;

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.back * Speed;

        InvokeRepeating("CheckDis", CheckRate, CheckRate);
    }

  
    void CheckDis()
    {      
        if (StartZ - transform.position.z > MaxDistance 
            || !PlayerInteractions.Dead && Player.position.z - transform.position.z >= PlayerDis)
        {
            DestOrDisactivate();
        }       
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!AlreadyDoneFor)
        {
            AlreadyDoneFor = true;
            AudioManager.AudMan.Play("Cannon Ball Crashed");

            //Debug.Log("I Collided with something that is : " + collision.transform.name);
            Instantiate(CanBallDesEffect, transform.position, new Quaternion());

            DestOrDisactivate();
        }
        
    }

    void DestOrDisactivate()
    {
        if (NeededCanBall)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
