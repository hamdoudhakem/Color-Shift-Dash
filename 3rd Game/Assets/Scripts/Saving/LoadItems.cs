using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItems : MonoBehaviour
{
    public static Material PlayerMat;
    public static Material Skybox;
    
    void Start()
    {
        Debug.Log("I Will Assign The Skybox and Skin");

        GetComponent<MeshRenderer>().material = PlayerMat;
        RenderSettings.skybox = Skybox;
        
    }    
}
