using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBridgeBehavior : MonoBehaviour
{
    public Material NeededMat;
    public MeshRenderer[] StartMeshes;
    public MeshRenderer[] SideMehes1;
    public MeshRenderer[] SideMehes2;
    
    void Start()
    {
        int Avoid = StaticData.Materials.IndexOf(NeededMat);
        int j;

        do
        {
            j = Random.Range(0, StaticData.Materials.Count);

        } while (j == Avoid);

        AssignStartMats(StaticData.Materials[Avoid], StaticData.Materials[j]);

        if (Random.Range(0 , 2) == 0)
        {            
            Assign(SideMehes1 , StaticData.Materials[Avoid]);
            Assign(SideMehes2 , StaticData.Materials[j]);
        }
        else
        {
            Assign(SideMehes1, StaticData.Materials[j]);
            Assign(SideMehes2, StaticData.Materials[Avoid]);
        }
    }

    private void AssignStartMats(Material Mat1 , Material Mat2)
    {
        if (Random.Range(0, 2) == 0)
        {
            StartMeshes[0].material = Mat1;
            StartMeshes[1].material = Mat2;
        }
        else
        {
            StartMeshes[0].material = Mat2;
            StartMeshes[1].material = Mat1;
        }
    }

    private void Assign(MeshRenderer[] sideMehes, Material material)
    {
        foreach(MeshRenderer mesh in sideMehes)
        {
            mesh.material = material;
        }
    }
}
