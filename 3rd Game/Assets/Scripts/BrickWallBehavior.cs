using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWallBehavior : MonoBehaviour
{
    public Material NeededColor;

    void Awake()
    {      
        int chosen = Random.Range(0, transform.childCount);
        int Prohibated = ColorsData.Materials.FindIndex(mat => mat == NeededColor);

        for(int i = 0; i< transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);

            if (i == chosen)
            {
                for (int y = 0; y < trans.childCount; y++)
                {
                    trans.GetChild(y).GetComponent<MeshRenderer>().material = NeededColor;
                }

            }
            else
            {             
                //Not to repeat the Needed color 
                int j;
                do
                {
                    j = Random.Range(0, ColorsData.Materials.Count);

                } while (j == Prohibated);

                for (int y = 0; y < trans.childCount; y++)
                {
                    trans.GetChild(y).GetComponent<MeshRenderer>().material = ColorsData.Materials[j];
                }
            }
        }
    }
   
}
