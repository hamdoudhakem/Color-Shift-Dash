using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBallBehavior : MonoBehaviour
{
    [Range(0f , 50)]
    public float Speed;    
    public GameObject Ball;
    public GameObject Splash;
    [Tooltip("How Much Time Before Splash Object is destroyed in Seconds")]
    public float LiveTime;
    [Tooltip("The Distance this Ball needs to be from the player to start Falling Down")]
    public float FallDistance;
    [Tooltip("How Much Between each Distance Check")]
    public float CheckRate;
    [Tooltip("The Distance between the Splasher and the Ball to destroy this Ball")]
    public float MaxDistance;
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    [Tooltip("The Size of the Box Cast that will help Locat the Player")]
    public Vector3 CastSize;
    
    private Rigidbody rb;
    private float StartPosZ;
    
    void Start()
    {
        StartPosZ = transform.position.z;

        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -Speed);                      

        Splash.GetComponent<MeshRenderer>().material = Ball.GetComponent<MeshRenderer>().material;

        InvokeRepeating("CheckDes", CheckRate, CheckRate);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * .5f, CastSize);   
        Gizmos.DrawCube(transform.position + Vector3.back * (FallDistance +.5f), CastSize);   
    }

    void Update()
    {       
        if(Physics.BoxCast(transform.position + Vector3.back * .5f, CastSize , Vector3.back , new Quaternion() , FallDistance , PlayerLayer))
        {
            rb.useGravity = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Ball.SetActive(false);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        Transform obj = Splash.transform.parent.parent;
        obj.position = transform.position + Vector3.down * 1.05f;
        
        obj.gameObject.SetActive(true);
        StartCoroutine(Des());               
    }
       
    IEnumerator Des()
    {
        yield return new WaitForSeconds(LiveTime);

        Destroy(gameObject);
    }

    void CheckDes()
    {
        if(StartPosZ - transform.position.z > MaxDistance)
        {
            Destroy(gameObject);
        }
    }
}
