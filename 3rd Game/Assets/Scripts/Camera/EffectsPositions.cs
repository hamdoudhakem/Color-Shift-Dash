using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPositions : MonoBehaviour
{
    public Transform Trail;

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
        if (SpeedUpEf.gameObject.activeSelf)
        {
            if (!PlayerInteractions.Dead)
            {
                float offset = Player.position.x - transform.position.x;

                SpeedUpEf.transform.position = transform.position + BaseOffset + (offset / 2) * Vector3.right;
            }
            else
            {
                SpeedUpEf.gameObject.SetActive(false);
            }
        }

        if (!PlayerInteractions.Dead)
        {
            Trail.position = Player.position;
        }
    }
}
