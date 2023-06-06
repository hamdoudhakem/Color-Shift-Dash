using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static bool Dead;
    public GameObject ParticleEffect;
    [HideInInspector] public float Origin;
    [Tooltip("How much the player needs to fall from his original platform to Die")]
    public float FallLimit;
    [Tooltip("The quantity of speed to add to my original speed after the speed boost lv1")]
    public float BoostValueLv1;
    [Tooltip("The quantity of speed to add to my original speed after the speed boost lv2")]
    public float BoostValueLv2;

    private LayerMask ColorSwitch , ColorObst , FinishLine , SpeedBoost;
    private MeshRenderer Mat;
    private PlayerMovement Pm;

    void Start()
    {
        Dead = false;
        Origin = transform.position.y;
        Pm = GetComponent<PlayerMovement>();
        Mat = GetComponent<MeshRenderer>();
        ColorSwitch = LayerMask.NameToLayer("Color Switch");
        ColorObst = LayerMask.NameToLayer("Color Obst");
        FinishLine = LayerMask.NameToLayer("Finish Line");
        SpeedBoost = LayerMask.NameToLayer("Speed Boost");

    }

    void Update()
    {
        if(Origin - transform.position.y >= FallLimit)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ColorSwitch)
        {
            Mat.material.color = other.GetComponent<MeshRenderer>().material.color;
        }
        else if (other.gameObject.layer == ColorObst)
        {
            DeathCheck(other);
        }
        else if (other.gameObject.layer == SpeedBoost)
        {
            if(other.tag == "Boost lv 1")
            {
                StartCoroutine(Pm.SpeedUp(BoostValueLv1));
            }
            else if (other.tag == "Boost lv 2")
            {               
                StartCoroutine(Pm.SpeedUp(BoostValueLv2 , false));                
            }
            
        }        
        else if (other.tag == "Cannon Stuff")
        {
            other.transform.parent.GetComponent<CannonsObs>().DestCan(other.transform);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ColorObst)
        {
            DeathCheck(collision.collider);
        }
        else if (collision.gameObject.layer == FinishLine)
        {
            StartCoroutine(Pm.Stop());
        }
        else if (collision.transform.tag == "Obstacle")
        {            
            Die();
        }
    }
   
    void DeathCheck(Collider col)
    {
        if (col.gameObject.GetComponent<MeshRenderer>().material.color != Mat.material.color)
        {          
            Die();
        }
    }

    void Die()
    {
        Debug.Log("You Died !");

        Dead = true;

        Instantiate(ParticleEffect, transform.position, new Quaternion()).transform.Rotate(Vector3.right * -90);

        Destroy(gameObject);
    }
}
