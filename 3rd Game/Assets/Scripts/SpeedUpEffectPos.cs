using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffectPos : MonoBehaviour
{
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
        }       
    }
}
