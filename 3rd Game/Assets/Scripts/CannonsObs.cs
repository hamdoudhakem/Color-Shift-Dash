using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsObs : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    ///This code is based on the fact that the 4 fist childs are the cannons parents
    ///then comes the 4 triggers
    public bool Randomize;
    [Space]

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
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    public float MaxDistance;
    [Tooltip("The Speed at which the Cannon Ball Goes")]
    public float Speed;
    [Tooltip("How Much this will increase the scale of the projected Cannon Balls")] [Range(1 , 2)]
    public float UpScalingFac;
    [Header("Sound")]
    public AudioSource Fire;
    public AudioSource Crashed;

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawCube(transform.position + Vector3.up * 4, OverBoxSize * 2);
        Gizmos.DrawCube(transform.position + new Vector3(0, 4, -FireDistance), OverBoxSize * 2);

        //The Overlap Box for the safe zone
        Gizmos.color = Color.cyan;
        //Gizmos.DrawCube(transform.position + new Vector3(0, 4, -SafeDistance), OverBoxSize * 2);
    }

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            //To check if enough time has passed between this shot and the last one
            if (TimeLeft <= 0)
            {
                //To see if the player is now pressing 1 of the triggers to disable a cannon
                if (Physics.OverlapBox(transform.position + new Vector3(0, 4, -SafeDistance), OverBoxSize, new Quaternion(), PlayerLayer).Length <= 0)
                {
                    if (Physics.BoxCast(transform.position + Vector3.up * 4, OverBoxSize, Vector3.back, out RaycastHit hit, new Quaternion(), FireDistance, PlayerLayer))
                    {
                        Fire.Play();
                        TimeLeft = FireRate;

                        int i = Randomize ? Random.Range(0 , ParRugs.Count) : ChooseCloseToPlayer(hit.transform), j;

                        j = (Random.Range(1, 3) + i) % ParRugs.Count;

                        CanBallBehavior obj = Instantiate(CanBall, ParRugs[i].position + new Vector3(-0.1f, 1), new Quaternion()).GetComponent<CanBallBehavior>();

                        obj.transform.localScale *= UpScalingFac;
                        obj.MaxDistance = MaxDistance;
                        obj.Crashed = Crashed;
                        obj.Speed = Speed;                        
                        obj.Set();

                        obj = Instantiate(CanBall, ParRugs[j].position + new Vector3(-0.1f, 1), new Quaternion()).GetComponent<CanBallBehavior>();

                        obj.transform.localScale *= UpScalingFac;
                        obj.MaxDistance = MaxDistance;
                        obj.Crashed = Crashed;
                        obj.Speed = Speed;
                        obj.Set();

                    }
                }

            }
            else
            {
                TimeLeft -= Time.deltaTime;
            }
        }      
    }

    int ChooseCloseToPlayer(Transform player)
    {
        int index = 0;
        float min = Mathf.Abs(player.position.x - ParRugs[0].position.x);

        for(int i = 1; i < ParRugs.Count; i++)
        {
            float dif = Mathf.Abs(player.position.x - ParRugs[i].position.x);

            if (dif < min)
            {
                min = dif;
                index = i;
            }
        }

        return index;
    }

    public void DestCan(Transform Rug)
    {
        if(used == false)
        {
            used = true;

            //The Cannon needs to be the first child of the parent Rug
            Rug2x[Rug].GetChild(0).gameObject.SetActive(false);

            ParRugs.Remove(Rug2x[Rug]);

            //for(int i = 0; i < 3; i++)
            //{
            //    Debug.Log($"The Number {i} at pos {ParRugs[i].position} there are {ParRugs.Count}");
            //}
        }
    }


}
