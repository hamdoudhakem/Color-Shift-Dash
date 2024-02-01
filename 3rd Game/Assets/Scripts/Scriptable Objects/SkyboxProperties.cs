using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "new Skybox",menuName = "Scriptable Objects/New Skybox")]
public class SkyboxProperties : ScriptableObject
{
    public string SkyboxName;

    public Material SkyboxMaterial;

    public VolumeProfile Profile;
}
