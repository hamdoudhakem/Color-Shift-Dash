using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectsBehavior : MonoBehaviour
{
    public Transform Trail;
    public VisualEffect TrailParticles;
    public VisualEffect DeathBall;

    private Transform SpeedUpEf;
    private Transform Player;
    private Vector3 BaseOffset;

    void Start()
    {
        Player = transform.parent.GetComponent<CameraMovement>().Player;
        SpeedUpEf = transform.GetChild(0);
        
        BaseOffset = SpeedUpEf.position - transform.position;
    }

    void FixedUpdate()
    {
        if (!PlayerInteractions.Dead)
        {
            if (SpeedUpEf.gameObject.activeSelf)
            {
                float offset = Player.position.x - transform.position.x;

                SpeedUpEf.transform.position = transform.position + BaseOffset + (offset / 2) * Vector3.right;
            }

            Trail.position = Player.position;

        }
        else
        {
            SpeedUpEf.gameObject.SetActive(false);
            DeathBall.gameObject.SetActive(true);
            enabled = false;
        }
    }       
    
}
