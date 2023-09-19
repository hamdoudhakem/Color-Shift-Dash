using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffectPos : MonoBehaviour
{
    public Transform Trail;

    private Transform Player;

    void Start()
    {
        Player = transform.parent.GetComponent<CameraMovement>().Player;
    }

    void FixedUpdate()
    {
        Trail.position = Player.position;
    }
}
