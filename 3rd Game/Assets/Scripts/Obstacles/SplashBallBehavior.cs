using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBallBehavior : MonoBehaviour
{
    [HideInInspector] public bool IsNeeded;

    [HideInInspector] public float Speed;
    [Tooltip("The Distance between the Splasher and the Ball to destroy this Ball")]
    [HideInInspector] public float MaxDistance;

    public GameObject Ball;
    public GameObject Splash;

    [Tooltip("How Much Time Before Splash Object is destroyed or disativated in Seconds")]
    public float LiveTime;
    [Tooltip("How Much Between each Distance Check")]
    public float CheckRate;     
    
    private Rigidbody rb;
    private float StartPosZ;
    
    public void start()
    {
        StartPosZ = transform.position.z;

        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -Speed);                      

        Splash.GetComponent<MeshRenderer>().material = Ball.GetComponent<MeshRenderer>().material;

        InvokeRepeating("CheckDes", CheckRate, CheckRate);
    }

    void CheckDes()
    {
        if (StartPosZ - transform.position.z > MaxDistance)
        {
            DesOrSpare();
        }
    }

    void DesOrSpare()
    {
        if (IsNeeded)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {        
        AudioManager.AudMan.Play("Color Ball Collided", true);

        Ball.SetActive(false);
        rb.velocity = Vector3.zero;

        Transform obj = Splash.transform.parent.parent;
        obj.gameObject.SetActive(true);

        StartCoroutine(Des(obj.gameObject));               
    }
       
    IEnumerator Des(GameObject SplashEff)
    {
        yield return new WaitForSeconds(LiveTime);        

        if (IsNeeded)
        {
            gameObject.SetActive(false);
            SplashEff.SetActive(false);
            Ball.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }   
   
}
