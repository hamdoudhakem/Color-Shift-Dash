using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemsForUse : MonoBehaviour
{
    public Material DefaultSkin;
    public List<Material> PlayerMats;
    public List<Material> Skyboxes;

    void Start()
    {
        
        if(PlayerData.CurrentSkin == string.Empty)
        {
            LoadItems.PlayerMat = DefaultSkin;
        }
        else
        {
            LoadItems.PlayerMat = PlayerMats.Find(mat => mat.name.Contains(PlayerData.CurrentSkin));

        }

        LoadItems.Skybox = Skyboxes.Find(skybox => skybox.name == PlayerData.CurrentSkybox);
    }
}
