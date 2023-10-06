using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoadItems : MonoBehaviour
{
    public static Material PlayerMat;
    public static Material Skybox;
    public List<VolumeProfile> Profile;

    //Needs to be in Awake ('cause of the skin/color change order)
    void Start()
    {
        Debug.Log("I Will Assign The Skybox, Skin and Instansiate the right Post Processing");

        GetComponent<CameraMovement>().Player.GetComponent<MeshRenderer>().material = PlayerMat;
        RenderSettings.skybox = Skybox;

        Volume PostPro = FindObjectOfType<Volume>();


        PostPro.profile = Profile.Find(volume => volume.name.Contains(Skybox.name));
    }    
}
