using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    public GameObject ParticleEffect;    

    void Start()
    {
        //Fixing Size
        ParticleEffect = Instantiate(ParticleEffect, transform.position + Vector3.down , new Quaternion() , transform);
        ParticleEffect.transform.localScale = Vector3.one;

        //Setting X scale value for the Shape
        ParticleSystem Ps = ParticleEffect.GetComponent<ParticleSystem>();
        var shape = Ps.shape;
        shape.scale = new Vector3(transform.localScale.x, 1 , 1);

        //Setting the color of the particles
        var main = Ps.main;
        main.startColor = GetComponent<MeshRenderer>().material.color;

        ParticleEffect.transform.Rotate(new Vector3(-90, 0));
    }
   
}
