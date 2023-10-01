using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItems : MonoBehaviour
{
    public static Material PlayerMat;
    public static Material Skybox;
    public List<GameObject> PostProcess;
    
    void Start()
    {
        Debug.Log("I Will Assign The Skybox, Skin and Instansiate the right Post Processing");

        GetComponent<MeshRenderer>().material = PlayerMat;
        RenderSettings.skybox = Skybox;

        GameObject PostPro = PostProcess.Find(obj => obj.name.Contains(Skybox.name));

        Instantiate(PostPro, Vector3.zero, new Quaternion());
    }    
}
