using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public List<Material> materials;

    public static List<Material> Materials;

    void Awake()
    {
        Materials = materials;
    }

    
}
