using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoving : MonoBehaviour
{
    [HideInInspector] public BallsPoolBehavior BallsPool;
    public Rigidbody rb;

    private bool AlreadyMoving;

    void Start()
    {
        AlreadyMoving = false;
    }
   
    void OnCollisionEnter(Collision collision)
    {
        if(BallsPool != null)
        {
            if(BallsPool.SoundLayers == (BallsPool.SoundLayers | (1 << collision.gameObject.layer)))
            {
                AudioManager.AudMan.Play("Hit Balls");

                Debug.Log("I'm Hitting Balls");

                InvokeRepeating("CheckMoving", 0, .5f);
            }
        }
    }

    void CheckMoving()
    {
        Vector3 vec = rb.velocity + rb.velocity.y * Vector3.down;

        if (vec.magnitude > .5f /*BallsPool.RelevantSpeed*/ )
        {
            //To Increase It Once not more
            if (!AlreadyMoving)
            {
                AlreadyMoving = true;
                BallsPool.MovingBalls++;
            }

        }
        else
        {
            //To Decrease It Once not more
            if (AlreadyMoving)
            {
                AlreadyMoving = false;
                BallsPool.MovingBalls--;
                CancelInvoke();
            }
        }
    }
}
