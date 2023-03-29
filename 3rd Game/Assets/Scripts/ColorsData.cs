using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsData : MonoBehaviour
{
    public List<Material> materials;

    public static List<Material> Materials;

    void Awake()
    {
        Materials = materials;
    }

    
}
