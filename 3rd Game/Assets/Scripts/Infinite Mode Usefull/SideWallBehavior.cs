using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWallBehavior : MonoBehaviour
{
    public Material NeededColor;

    void Start()
    {
        Material j;
       
        do
        {
            j = StaticData.Materials[Random.Range(0, StaticData.Materials.Count)];

        } while (j == NeededColor);

        GetComponent<MeshRenderer>().material = j;
    }

}
