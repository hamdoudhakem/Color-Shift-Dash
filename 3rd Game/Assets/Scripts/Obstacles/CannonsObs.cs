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
    public GameObject FireEff;
    public LayerMask PlayerLayer;
    [Tooltip("How Much Time Passes between every Shot (in seconds)")]
    public float FireRate;       
    [Tooltip("The Distance from which the player needs to be from the Cannons for them to start Shooting")]
    public float FireDistance;    
    [Tooltip("The Size of the Overlap Box that will prevente the Cannon from Shooting at times")]
    public Vector3 OverBoxSize;
    [Tooltip("The Offset from the Rugs near the cannons that will lead to the firing point")]
    public Vector3 OffsetFirePoint;
    [Tooltip("The Maximum Distance between this Cannon Ball and the Cannon at which the ball is gonna self destroy")]
    public float MaxDistance;
    [Tooltip("The Speed at which the Cannon Ball Goes")]
    public float Speed;
    [Tooltip("How Much this will increase the scale of the projected Cannon Balls")] [Range(.5f , 2)]
    public float UpScalingFac;

    [Header("Destroy Cannon Effect")]
    [Tooltip("The Time it will take the Cannon to be destroyed")]
    public float DestAnimTime;

    [Header("Performances")]
    [Tooltip("how Many times between each player position check to see if I should Disable This Script or not yet")]
    public float PlayerPosCheckDelay;

    [Tooltip("How Many Cannons There Are (Right Now I Have 4 cannons)")]
    private int CannonsNum;
    private int CanBallsPerFire;
    private float TimeLeft;
    [Tooltip("The Distance at which the cannons will not fire as to let the player choose a cannon to destroy")]
    private float SafeDistance;   
    private bool used;
    private List<Transform> ParRugs;
    private Dictionary<Transform, Transform> Rug2x;
    private Transform Player;

    private List<CanBallBehavior> CannonBalls;
    private ParticleSystem[] FireEffs;

    private Transform CannonToDest;
    private bool StartCannonDestAnim;   

    void Start()
    {
        TimeLeft = 0;
        CanBallsPerFire = 2;

        used = false;
        CannonsNum = transform.childCount / 2;

        StartCannonDestAnim = false;

        //Fillin The Rug2x Disctionary with the Rugs and their corresponding Cannons
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

        //Setting Up Stuff and this Needs to Be done After everything Written Up Here
        CreatBaseCanBalls();
        CreatBaseFireEff();
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
                    if (Physics.BoxCast(transform.position + Vector3.up * 4, OverBoxSize,Vector3.back,out RaycastHit hit, new Quaternion(), FireDistance, PlayerLayer))
                    {
                        if(Player == null)
                        {
                            Player = hit.transform;
                            InvokeRepeating("CheckObstPassed", PlayerPosCheckDelay, PlayerPosCheckDelay);                           
                        }

                        TimeLeft = FireRate;

                        int i = Randomize ? Random.Range(0 , ParRugs.Count) : ChooseCloseToPlayer(hit.transform), j;

                        j = (Random.Range(1, 3) + i) % ParRugs.Count;


                        //To see if I will activate one of my Created balls or will creat a new ones
                        int index = 0;

                        do
                        {
                            if (!CannonBalls[index].gameObject.activeSelf)
                            {
                                break;
                            }

                            index += CanBallsPerFire;

                        } while (index < CannonBalls.Count);

                        if(index < CannonBalls.Count)
                        {
                            CanBallBehavior obj = CannonBalls[index];

                            obj.transform.position = ParRugs[i].position + OffsetFirePoint;

                            SetUpCanBall(obj);

                            SetFireEff(obj.transform, 0);

                            obj = CannonBalls[index + 1];

                            obj.transform.position = ParRugs[j].position + OffsetFirePoint;

                            SetUpCanBall(obj);

                            SetFireEff(obj.transform, 1); 
                        }
                        else
                        {
                            CanBallBehavior obj = Instantiate(CanBall, ParRugs[i].position + OffsetFirePoint, new Quaternion()).GetComponent<CanBallBehavior>();

                            SetCanBallVars(obj, false); 
                            SetUpCanBall(obj);

                            SetFireEff(obj.transform, 0);

                            obj = Instantiate(CanBall, ParRugs[j].position + OffsetFirePoint, new Quaternion()).GetComponent<CanBallBehavior>();

                            SetCanBallVars(obj, false);
                            SetUpCanBall(obj);

                            SetFireEff(obj.transform, 1);
                        }                       

                        AudioManager.AudMan.Play("Cannon Fire", true);

                    }
                }

            }
            else
            {
                TimeLeft -= Time.deltaTime;
            }
        }

        if (StartCannonDestAnim)
        {
            CannonToDest.localScale -= (1 / DestAnimTime) * Time.deltaTime * CannonToDest.localScale;

            if (CannonToDest.localScale.magnitude <= 0.1f)
            {
                CannonToDest.gameObject.SetActive(false);
                StartCannonDestAnim = false;
            }
        }        
    }

    #region Setting Up The Cannon Balls

    private void CreatBaseCanBalls()
    {
        CannonBalls = new List<CanBallBehavior>(CannonsNum);

        for (int i = 0; i < CannonsNum; i++)
        {
            CanBallBehavior Ball = Instantiate(CanBall, ParRugs[i].position + OffsetFirePoint, new Quaternion(), transform).GetComponent<CanBallBehavior>();

            SetCanBallVars(Ball, true);            

            Ball.gameObject.SetActive(false);

            CannonBalls.Add(Ball);
        }
    }

    private void SetCanBallVars(CanBallBehavior obj , bool NeededBall)
    {
        obj.transform.localScale *= UpScalingFac;
        obj.NeededCanBall = NeededBall;
        obj.MaxDistance = MaxDistance;
        obj.Speed = Speed;
    }

    private void SetUpCanBall(CanBallBehavior obj)
    {
        obj.Player = Player;

        obj.gameObject.SetActive(true);

        obj.Set();
    }

    #endregion

    #region Firing Effect Related

    void CreatBaseFireEff()
    {
        FireEffs = new ParticleSystem[CanBallsPerFire];

        for(int i =0; i < FireEffs.Length; i++)
        {
            FireEffs[i] = Instantiate(FireEff, transform.position, new Quaternion(), transform).GetComponent<ParticleSystem>();

            FireEffs[i].Stop();
        }
    }  

    void SetFireEff(Transform startPoint, int EffIndex)
    {
        FireEffs[EffIndex].transform.position = startPoint.position;
        FireEffs[EffIndex].Play();        
    }

    int ChooseCloseToPlayer(Transform player)
    {
        int index = 0;
        float min = Mathf.Abs(player.position.x - ParRugs[0].position.x);

        for (int i = 1; i < ParRugs.Count; i++)
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

    #endregion

    #region Destroying Cannons and Disabling This Script

    public void DestCan(Transform Rug)
    {
        if(!used)
        {
            used = true;

            ParRugs.Remove(Rug2x[Rug]);

            //The Cannon needs to be the first child of the parent Rug           
            CannonToDest = Rug2x[Rug].GetChild(0);
            StartCannonDestAnim = true;                       

            AudioManager.AudMan.Play("Cannon Gone", true);

            //for(int i = 0; i < 3; i++)
            //{
            //    Debug.Log($"The Number {i} at pos {ParRugs[i].position} there are {ParRugs.Count}");
            //}
        }
    }

    void CheckObstPassed()
    {
        if(Player == null || Player.position.z - transform.position.z >= 3)
        {
            foreach(ParticleSystem part in FireEffs)
            {
                part.gameObject.SetActive(false);
            }

            enabled = false;
            CancelInvoke("CheckObstPassed");
        }
      
    }

    #endregion
}
