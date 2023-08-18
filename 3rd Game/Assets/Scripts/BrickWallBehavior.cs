using UnityEngine;

public class BrickWallBehavior : MonoBehaviour, IObsTypes, IColParent
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededColor;

    private LayerMask PlayerLayer;
    private int ContactCount;

    void Start()
    {
        ContactCount = 0;
        PlayerLayer = LayerMask.NameToLayer("Player");

        int chosen = Random.Range(0, transform.childCount);

        for (int i = 0; i< transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);

            if (i == chosen)
            {
               // Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : I Will give the Needed color to = {trans.name}");
                for (int y = 0; y < trans.childCount; y++)
                {
                    Transform child = trans.GetChild(y);
                    child.GetComponent<MeshRenderer>().material = NeededColor;
                    child.GetComponent<Collided>().ColProcess = this;
                }

            }
            else
            {
                Material OtherMat = StaticData.Materials[StaticData.ChooseMat(NeededColor)];

                //Debug.Log($"{(transform.name.Contains("2") ? "Script 2" : "Script 1")} : I Will give the {ColorsData.Materials[j].name} color to = {trans.name}");
                for (int y = 0; y < trans.childCount; y++)
                {
                    Transform child = trans.GetChild(y);
                    child.GetComponent<MeshRenderer>().material = OtherMat;
                    child.GetComponent<Collided>().ColProcess = this;
                }
            }
        }
    }

    public void OnCollision(Collision collision)
    {
        if(collision.gameObject.layer == PlayerLayer && !PlayerInteractions.Dead)
        {
            AudioManager.AudMan.Play("Hit Boxes" , true);
            AudioManager.AudMan.Play("Drag Boxes");
            ContactCount++;           
        }
    }

    public void OnExitCollision(Collision collision)
    {
        if (collision.gameObject.layer == PlayerLayer)
        {
            ContactCount--;
            //Debug.Log("My Contact Count is After : " + ContactCount);

            if (ContactCount < 1)
            {
                AudioManager.AudMan.Stop("Drag Boxes");
                //Debug.Log("So I Will Stop The Drag Boxes sound");
            }
        }
        
    }
}
