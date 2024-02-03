using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBridgeBehavior : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededMat;

    [Space]   
    public TransXMatIndexDictionary TransXMatIndex;
    [Tooltip("If this is False then The Other Side materials (where I can Have the Third Bridge) won't change there current material at Runtime")]
    public bool ChangeOtherSideMat;
    public Transform[] StartMeshes;
    public Transform[] SideMehes1;
    public Transform[] SideMehes2;
    [Tooltip("The Middle bridge that I added to this obstacle")]
    public Transform[] ThirdBridge;
    
    private MeshRenderer mesh;       
        
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        Material OtherMat = StaticData.Materials[StaticData.ChooseMat(NeededMat)];

        //Chossing which starting material will get the NeededMat
        int ChosenStartMesh = Random.Range(0, StartMeshes.Length);

        //Chossing which other material will get the NeededMat
        Transform[] ChosenOtherMesh = SideMehes1;

        int y = (ThirdBridge.Length > 0 ? Random.Range(0, 3) : Random.Range(0, 2));

        switch (y)
        {
            case 0:
                ChosenOtherMesh = SideMehes1;
                break;
            case 1:
                ChosenOtherMesh = SideMehes2;
                break;
            case 2:
                ChosenOtherMesh = ThirdBridge;
                break;
        }

        //Getting the Mat Array to be able to change the materials
        Material[] mats = mesh.materials;

        //Assigning the Needed mat to one of the Starting materials
        mats[TransXMatIndex[StartMeshes[ChosenStartMesh]]].color = NeededMat.color;

        
        if (ChangeOtherSideMat)
        {
            Debug.Log("I will change the Transform : " + ChosenOtherMesh[0].name + " which mat is number : " + TransXMatIndex[ChosenOtherMesh[0]]);
            //Assigning the NeededMat to a Material in the other Side 
            mats[TransXMatIndex[ChosenOtherMesh[0]]].color = NeededMat.color;

            //I created an array of the indexes of the materials
            //that have the NeededMat to only change the other Materials to the Other Mat
            List<int> NeededMatIndex = new List<int>() { TransXMatIndex[StartMeshes[ChosenStartMesh]], TransXMatIndex[ChosenOtherMesh[0]] };

            for (int i = 0; i < mats.Length; i++)
            {
                //I'm Skipping the Middle Part Material (the Grounded/Neutral one)
                if(i == 2)
                {
                    continue;
                }

                if (!NeededMatIndex.Contains(i))
                {
                    mats[i].color = OtherMat.color;
                }                
                                
            }
        }
        else
        {
            //Changing the Second Starting Material to Other Mat (after assigning NeededMat to the other one)
            mats[TransXMatIndex[StartMeshes[(ChosenStartMesh + 1) % StartMeshes.Length]]].color = OtherMat.color;
        }


    }

    public Material GetTouchedColor(Transform TouchedChild)
    {
        return mesh.materials[TransXMatIndex[TouchedChild]];
    }
        
}

[System.Serializable]
public class TransXMatIndexDictionary : SerializableDictionaryBase<Transform, int> { }