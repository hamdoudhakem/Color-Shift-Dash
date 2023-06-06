using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWallBehavior : MonoBehaviour
{
    public Material NeededColor;

    void Start()
    {        
        int chosen = Random.Range(0, transform.childCount);
        int Prohibated = StaticData.Materials.FindIndex(mat => mat == NeededColor);
        //Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : prohibited Index is = {Prohibated}");

        for (int i = 0; i< transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);

            if (i == chosen)
            {
               // Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : I Will give the Needed color to = {trans.name}");
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
                    j = Random.Range(0, StaticData.Materials.Count);

                } while (j == Prohibated);

                //Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : I Will give the {ColorsData.Materials[j].name} color to = {trans.name}");
                for (int y = 0; y < trans.childCount; y++)
                {
                    trans.GetChild(y).GetComponent<MeshRenderer>().material = StaticData.Materials[j];
                }
            }
        }
    }
   
}
