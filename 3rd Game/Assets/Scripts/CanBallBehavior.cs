using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBallBehavior : MonoBehaviour
{
    [Tooltip("The Cannon Ball Destruction particle effect")]
    public GameObject CanBallDesEffect;
    [Tooltip("How Much Times passes between each Distance Check (in seconds)")]
    public float CheckRate;

    [Tooltip("The Speed at which the Cannon Ball Goes")]
    [HideInInspector] public float Speed;
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    [HideInInspector] public float MaxDistance;

    private Rigidbody rb;
    private float StartZ;
    private bool AlreadyDoneFor;
    [HideInInspector] public AudioSource Crashed;

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
        if(StartZ - transform.position.z > MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!AlreadyDoneFor)
        {
            AlreadyDoneFor = true;
            Crashed.Play();

            //Debug.Log("I Collided with something that is : " + collision.transform.name);
            Instantiate(CanBallDesEffect, transform.position, new Quaternion());
            Destroy(gameObject);
        }
        
    }


}
