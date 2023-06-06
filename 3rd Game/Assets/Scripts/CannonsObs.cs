using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsObs : MonoBehaviour
{
    ///This code is based on the fact that the 4 fist childs are the cannons parents
    ///then comes the 4 triggers

    [Tooltip("The Cannon Ball Game Object")]
    public GameObject CanBall;
    public LayerMask PlayerLayer;
    [Tooltip("How Much Time Passes between every Shot (in seconds)")]
    public float FireRate;    
    [Tooltip("How Many Cannons There Are (Right Now I Have 4 cannons)")] 
    public int CannonsNum;
    [Tooltip("The Distance from which the player needs to be from the Cannons for them to start Shooting")]
    public float FireDistance;    
    [Tooltip("The Size of the Overlap Box that will prevente the Cannon from Shooting at times")]
    public Vector3 OverBoxSize;

    private float TimeLeft;
    [Tooltip("The Distance at which the cannons will not fire as to let the player choose a cannon to destroy")]
    private float SafeDistance;   
    private bool used;
    private List<Transform> ParRugs;
    private Dictionary<Transform, Transform> Rug2x;

    void Start()
    {
        TimeLeft = 0;
        used = false;
        ParRugs = new List<Transform>();
        Rug2x = new Dictionary<Transform, Transform>();

        Dictionary<int , int > ObjXmats = new Dictionary<int, int>(CannonsNum);
        List<int> objIndexs = new List<int>(CannonsNum);

        int j;    

        for (int i = 0; i < transform.childCount; i++)
        {
            if(i < CannonsNum)
            {                
                do
                {
                    j = Random.Range(0, StaticData.Materials.Count);

                } while (ObjXmats.ContainsValue(j));

                objIndexs.Add(i);
                ObjXmats.Add(i , j);

                Transform obj = transform.GetChild(i);

                obj.GetComponent<MeshRenderer>().material = StaticData.Materials[j];

                ParRugs.Add(obj);                               
            }
            else
            {
                j = Random.Range(0, objIndexs.Count);

                Transform obj = transform.GetChild(i);

                obj.GetComponent<MeshRenderer>().material = StaticData.Materials[ObjXmats[objIndexs[j]]];

                Rug2x.Add(obj, transform.GetChild(objIndexs[j]));

                objIndexs.RemoveAt(j);
            }
           
        }

        SafeDistance = ParRugs[0].position.z - transform.GetChild(CannonsNum).position.z;
        
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.gray;
    //    Gizmos.DrawCube(transform.position + Vector3.up * 4, OverBoxSize);
    //    Gizmos.DrawCube(transform.position + new Vector3(0, 4, -FireDistance), OverBoxSize);

    //    //The Overlap Box for the safe zone
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawCube(transform.position + new Vector3(0, 4, -SafeDistance), OverBoxSize);
    //}

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            if (Physics.BoxCast(transform.position + Vector3.up * 4, OverBoxSize, Vector3.back, new Quaternion(), FireDistance, PlayerLayer))
            {
                //To check if enough time has passed between this shot and the last one
                if (TimeLeft <= 0)
                {
                    //To see if the player is now pressing 1 of the triggers to disable a cannon
                    if (Physics.OverlapBox(transform.position + new Vector3(0, 4, -SafeDistance), OverBoxSize, new Quaternion(), PlayerLayer).Length <= 0)
                    {
                        TimeLeft = FireRate;

                        int i = Random.Range(0, ParRugs.Count), j;

                        j = (Random.Range(1, 2) + i) % ParRugs.Count;

                        Instantiate(CanBall, ParRugs[i].position + new Vector3(-0.1f, 1), new Quaternion());

                        Instantiate(CanBall, ParRugs[j].position + new Vector3(-0.1f, 1), new Quaternion());
                    }
                }
                else
                {
                    TimeLeft -= Time.deltaTime;
                }
            }
        }      
    }

    public void DestCan(Transform Rug)
    {
        if(used == false)
        {
            used = true;

            //The Cannon needs to be the first child of the parent Rug
            Rug2x[Rug].GetChild(0).gameObject.SetActive(false);

            ParRugs.Remove(Rug2x[Rug]);

            for(int i = 0; i < 3; i++)
            {
                Debug.Log($"The Number {i} at pos {ParRugs[i].position} there are {ParRugs.Count}");
            }
        }
    }


}
