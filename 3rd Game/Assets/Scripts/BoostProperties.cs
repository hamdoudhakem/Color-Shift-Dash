using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostProperties : MonoBehaviour
{
    public GameObject ParticleEffect;

    [Tooltip("if this is true then I will change the Boost value that the Player will receive to OveridedBoostVal and the Boost Duration")]
    public bool OverideBoostVal;
    [Tooltip("The Custom Speed Boost that the Player will Get")]
    public float OveridedBoostVal;
    [Tooltip("if this is true then I will change the Boost value that the Player will receive to OveridedBoostVal and the Boost Duration")]

    public bool OverideBoostTime;
    [Tooltip("The Custom Time For the Speed Boost the player will receive (0 means I will take the default value set by PlayerMovement Script)")]
    [Range(0 , 100)]
    public float OveridedBoostTime;

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
