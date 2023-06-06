using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSwitcher : MonoBehaviour
{
    [Tooltip("The Distance the player needs to be at for the switcher to chose a final ground")]
    public float Distance;
    [Tooltip("How much Delay between every Ground Change (In seconds)")]
    public float Delay;
    [Tooltip("The Offset to the center of the Overlap Box")]
    public Vector3 Offset;
    [Tooltip("Size of the overlap Box")]
    public Vector3 Size;
    public LayerMask PlayerLayer;

    private GameObject[] Grs;    

    void Start()
    {
        Grs = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            Grs[i] = transform.GetChild(i).gameObject;
        }

        InvokeRepeating("ChangeDelay", 0, Delay);
    }    

    void ChangeDelay()
    {
        if(Grs[0].activeSelf == false)
        {
            Grs[0].SetActive(true);
            Grs[1].SetActive(false);
        }
        else
        {
            Grs[0].SetActive(false);
            Grs[1].SetActive(true);
        }       

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Offset, Size);
    }

    void Update()
    {
        if (Physics.OverlapBox(transform.position + Offset, Size ,new Quaternion(),PlayerLayer).Length > 0) 
        {
            CancelInvoke();
        }

    }

}
