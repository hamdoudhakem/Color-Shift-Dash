using UnityEngine;

public class BrickWallBehavior : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededColor;  

    void Start()
    {        
        int chosen = Random.Range(0, transform.childCount);

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
                Material OtherMat = StaticData.Materials[StaticData.ChooseMat(NeededColor)];

                //Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : I Will give the {ColorsData.Materials[j].name} color to = {trans.name}");
                for (int y = 0; y < trans.childCount; y++)
                {
                    trans.GetChild(y).GetComponent<MeshRenderer>().material = OtherMat;
                }
            }
        }
    }
   
}
