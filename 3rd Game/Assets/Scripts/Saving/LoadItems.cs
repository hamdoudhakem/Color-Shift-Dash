using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoadItems : MonoBehaviour
{
    public static Skin PlayerSkin;
    public static SkyboxProperties Skybox;   

    void Awake()
    {
        Vibration.Init();   
    }

    ///<summary>
    ///Needs to be in Awake ('cause of the skin/color change order)
    ///</summary>
    void Start()
    {             
        Debug.Log("I Will Assign The Skybox, Skin and Instansiate the right Post Processing");

        //Getting the Variables
        Transform Player = GetComponent<CameraMovement>().Player;

        MeshFilter mr = Player.GetComponent<MeshFilter>();

        //Loading The Skin
        mr.mesh = PlayerSkin.mesh;
        if (PlayerSkin.CustMaterials)
        {
            mr.GetComponent<MeshRenderer>().materials = PlayerSkin.Materials;
        }
        
        //Loading The Skybox
        RenderSettings.skybox = Skybox.SkyboxMaterial;

        //Loading the Post-rocessing
        Volume PostPro = GameObject.FindWithTag("Volume").GetComponent<Volume>();

        PostPro.profile = Skybox.Profile;

        Player.GetComponent<PlayerMovement>().Lens = (LensDistortion)PostPro.profile.components.Find(comp => comp is LensDistortion);
              
    }        
}
