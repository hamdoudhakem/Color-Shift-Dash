using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static bool Dead;
    public GameObject ParticleEffect;
    private LayerMask ColorSwitch , ColorObst , FinishLine;
    private Material Mat;
    private PlayerMovement Pm;

    void Start()
    {
        Dead = false;
        Pm = GetComponent<PlayerMovement>();
        Mat = GetComponent<MeshRenderer>().material;
        ColorSwitch = LayerMask.NameToLayer("Color Switch");
        ColorObst = LayerMask.NameToLayer("Color Obst");
        FinishLine = LayerMask.NameToLayer("Finish Line");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ColorSwitch)
        {
            Mat.color = other.GetComponent<MeshRenderer>().material.color;
        }
        else if (other.gameObject.layer == ColorObst)
        {
            DeathCheck(other);
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
    }

    void DeathCheck(Collider col)
    {
        if (col.gameObject.GetComponent<MeshRenderer>().material.color != Mat.color)
        {
            Debug.Log("You Died !");
            Dead = true;

            Instantiate(ParticleEffect, transform.position, new Quaternion()).transform.Rotate(Vector3.right * -90);               

            Destroy(gameObject);
        }
    }
}
