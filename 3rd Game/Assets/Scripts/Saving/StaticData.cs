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

    public static int ChooseMat(Material MatToAvoid)
    {
        int i = Materials.IndexOf(MatToAvoid);
        int j;

        if(i == 0)
        {         
            j = Random.Range(1, Materials.Count);
        }
        else if(i == Materials.Count - 1)
        {
            j = Random.Range(0, Materials.Count-1);
        }
        else
        {
            j = Random.Range(0 , 2) == 0 ? Random.Range(0, i) : Random.Range(i + 1, Materials.Count);
        }

        return j;
    }


}
