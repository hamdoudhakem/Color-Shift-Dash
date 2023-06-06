using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBallBehavior : MonoBehaviour
{
    [Tooltip("The Speed at which the Cannon Ball Goes")]
    public float Speed;
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    public float MaxDistance;
    [Tooltip("How Much Times passes between each Distance Check (in seconds)")]
    public float CheckRate;

    private Rigidbody rb;
    private float StartZ;

    void Start()
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
            Destroy(gameObject);
        }       
    }


}
