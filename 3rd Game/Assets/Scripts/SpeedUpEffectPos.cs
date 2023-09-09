using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffectPos : MonoBehaviour
{
    private Transform SpeedUpEf;
    private Transform Player;

    void Start()
    {
        Player = GetComponent<CameraMovement>().Player;
        SpeedUpEf = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        if (SpeedUpEf.gameObject.activeSelf)
        {
            float offset = Player.position.x - transform.position.x;

            SpeedUpEf.localPosition = new Vector3(-offset / 2, SpeedUpEf.localPosition.y, SpeedUpEf.localPosition.z);
        }        
    }
}
