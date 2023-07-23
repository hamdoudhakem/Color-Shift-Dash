using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBallBehavior : MonoBehaviour
{    
    [Tooltip("How Much Times passes between each Distance Check (in seconds)")]
    public float CheckRate;

    [Tooltip("The Speed at which the Cannon Ball Goes")]
    [HideInInspector] public float Speed;
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    [HideInInspector] public float MaxDistance;

    private Rigidbody rb;
    private float StartZ;

    public void Set()
    {
        StartZ = transform.position.z;

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
        if(collision.transform.tag == "Player")
        {
            rb.velocity = Vector3.back * Speed;
        }
        else
        {
            //Debug.Log("I Didn't Collide with the Player I Col with : " + collision.transform.name);
            Destroy(gameObject);
        }       
    }


}
