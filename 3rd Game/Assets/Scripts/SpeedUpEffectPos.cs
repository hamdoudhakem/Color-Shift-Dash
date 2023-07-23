using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffectPos : MonoBehaviour
{
    private Transform SpeedUpEf;
    private Transform Cam;

    void Start()
    {
        Cam = GetComponent<PlayerMovement>().Cam.transform;
        SpeedUpEf = transform.GetChild(0);
    }

    void Update()
    {
        if (SpeedUpEf.gameObject.activeSelf)
        {
            float offset = transform.position.x - Cam.position.x;

            SpeedUpEf.localPosition = new Vector3(-offset / 2, SpeedUpEf.localPosition.y, SpeedUpEf.localPosition.z);
        }        
    }
}
