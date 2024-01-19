using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoadItems : MonoBehaviour
{
    public static Skin PlayerSkin;
    public static Material Skybox;
    public static VolumeProfile Profile;

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

        Transform Player = GetComponent<CameraMovement>().Player;

        SetSkin(Player.GetComponent<MeshFilter>());

        RenderSettings.skybox = Skybox;

        //Loading the Post-rocessing
        Volume PostPro = FindObjectOfType<Volume>();

        PostPro.profile = Profile;

        Player.GetComponent<PlayerMovement>().Lens = (LensDistortion)PostPro.profile.components.Find(comp => comp is LensDistortion);
    }

    void SetSkin(MeshFilter mr)
    {
        mr.mesh = PlayerSkin.mesh;

        if (PlayerSkin.CustMaterials)
        {
            mr.GetComponent<MeshRenderer>().materials = PlayerSkin.Materials;
        }
    }
}
