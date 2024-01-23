using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetItemsForUse : MonoBehaviour
{
    public Skin DefaultSkin;
    public List<Skin> PlayerSkins;
    public List<SkyboxProperties> Skyboxes;    

    void Start()
    {        
        if(PlayerData.CurrentSkin == string.Empty)
        {
            LoadItems.PlayerSkin = DefaultSkin;
        }
        else
        {
            LoadItems.PlayerSkin = PlayerSkins.Find(skin => skin.SkinName.Contains(PlayerData.CurrentSkin));
        }

        LoadItems.Skybox = Skyboxes.Find(skybox => skybox.SkyboxName == PlayerData.CurrentSkybox);
    }
}
