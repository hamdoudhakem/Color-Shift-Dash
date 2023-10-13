using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LoadItems : MonoBehaviour
{
    public static Skin PlayerSkin;
    public static Material Skybox;
    public static VolumeProfile Profile;

    ///<summary>
    ///Needs to be in Awake ('cause of the skin/color change order)
    ///</summary>
    void Start()
    {
        Debug.Log("I Will Assign The Skybox, Skin and Instansiate the right Post Processing");

        SetSkin(GetComponent<CameraMovement>().Player.GetComponent<MeshRenderer>());

        RenderSettings.skybox = Skybox;

        //Loading the Post-rocessing
        Volume PostPro = FindObjectOfType<Volume>();
        
        PostPro.profile = Profile;
    }

    void SetSkin(MeshRenderer mr)
    {
        mr.GetComponent<MeshFilter>().mesh = PlayerSkin.mesh;

        if (PlayerSkin.CustMaterials)
        {
            mr.materials = PlayerSkin.Materials;
        }
    }
}
