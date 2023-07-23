using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWayObstacles : MonoBehaviour
{
    public Material AvoidMat;

    void Start()
    {
        int j = StaticData.ChooseMat(AvoidMat);

        GetComponent<MeshRenderer>().material = StaticData.Materials[j];
    }

    
}
