using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Skin",menuName = "Scriptable Objects/New Skin")]
public class Skin : ScriptableObject
{
    public string SkinName;

    public Mesh mesh;

    public bool CustMaterials;

    public Material[] Materials;

}
