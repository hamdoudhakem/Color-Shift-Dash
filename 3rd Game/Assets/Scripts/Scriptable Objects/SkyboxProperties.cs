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

    public Vector3 LightRotation;

    public Color LightColor = new Color(0.9150943f, 0.910242f, 0.6776878f);
}
