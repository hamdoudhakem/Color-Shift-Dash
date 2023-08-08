using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBridgeBehavior : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededMat;
    public MeshRenderer[] StartMeshes;
    public MeshRenderer[] SideMehes1;
    public MeshRenderer[] SideMehes2;
    [Tooltip("The Middle bridge that I added to this obstacle")]
    public MeshRenderer[] ThirdBridge;

    private Material OtherMat;
    void Start()
    {
        OtherMat = StaticData.Materials[StaticData.ChooseMat(NeededMat)];

        AssignStartMats(NeededMat, OtherMat);

        if(ThirdBridge.Length == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                Assign(SideMehes1, NeededMat);
                Assign(SideMehes2, OtherMat);
            }
            else
            {
                Assign(SideMehes1, OtherMat);
                Assign(SideMehes2, NeededMat);
            }
        }
        else
        {
            int y = Random.Range(0, 3), TrackingNeededMat = 0;            

            //To randomise My starting mesh and Go On from it
            for(int i = 0; i < 3; i++)
            {
                if((y + i) % 3 == 0)
                {
                    Assign(SideMehes1, ref TrackingNeededMat);
                }
                else if ((y + i) % 3 == 1)
                {
                    Assign(SideMehes2, ref TrackingNeededMat);
                }
                else if ((y + i) % 3 == 2)
                {
                    Assign(ThirdBridge ,ref TrackingNeededMat);
                }
            }
        }                     
    }

    private void AssignStartMats(Material Mat1 , Material Mat2)
    {
        int i = Random.Range(0 , 2);

        foreach(MeshRenderer mesh in StartMeshes)
        {
            if(i % 2 == 0)
            {
                mesh.material = Mat1;
            }
            else
            {
                mesh.material = Mat2;
            }

            i++;
        }        
    }

    private void Assign(MeshRenderer[] sideMehes, Material material)
    {
        foreach(MeshRenderer mesh in sideMehes)
        {
            mesh.material = material;
        }
    }

    private void Assign(MeshRenderer[] sideMeshes ,ref int Count)
    {
        //In case i didn't get a single NeededMat
        if(Count == 2)
        {
            Assign(sideMeshes, NeededMat);
        }
        else
        {
            //To make so that i don't get 3 NeededMats (2 Max)
            if (Count > -1 && Random.Range(0 , 2) == 0)
            {
                Assign(sideMeshes, NeededMat);
                Count = -1;
            }
            else
            {                
                Assign(sideMeshes, OtherMat);
                Count++;
            }
                  
        }

    }

    
}
