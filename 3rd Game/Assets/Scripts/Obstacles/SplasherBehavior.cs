using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplasherBehavior : MonoBehaviour ,IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get;  set; }

    [Header("Layers")]
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;

    [Header("Objects")]
    public Transform GroundCheck;
    public Transform Barrel;
    public GameObject SplashBall;

    [Header("Vectors")]
    [Tooltip("The Size of the Overlap Box that will check for Ground before changing the splasher position")]
    public Vector3 CheckSize;
    [Tooltip("The Size of the box cast that will check for the player")]
    public Vector3 CastSize;

    [Header("Ball's Variables")]
    [Tooltip("The Offset of the Spawnined Ball from the Barrel Position")]
    public Vector3 BarrelOffset;
    [Tooltip("The Speed of the launched Ball")] [Range(0f, 50)]
    public float BallSpeed;
    [Tooltip("The Distance between the Splasher and the Ball to destroy this Ball")]
    public float MaxDistance;
    [Tooltip("How Much this will increase or decrease the scale of the projected Balls")]
    [Range(0, 2)]
    public float ScalingFac;

    [Header("Splasher's Variables")]
    [Tooltip("The Speed at which the Cannon will move to position itself")]
    public float MovingSpeed;
    [Tooltip("The Distance at which the player needs to be for the Splasher to start Shooting")]
    public float Range;
    public float FireRate;
    [Tooltip("How Many Shots before this stops working")] [Range(1 , 6)]
    public int ShotNumber;
    [Tooltip("How Many Ball's Should I Instantiate at the start that I will keep recycling")] [Range(1, 6)]
    public int InitialBallsNum;

    [Header("Performances")]
    [Tooltip("The Delay between each Player position check to see if i should Disable this Script or not")]
    public float PlayerPosCheckDelay;

    private Transform Player;
    private float CoolDown;
    private bool Moving;
    private Vector3 Target;
    private MeshRenderer Mesh;
    private int ShotNum;
    private Vector3 StartPos;

    private List<SplashBallBehavior> BaseBalls;

    void Start()
    {
        StartPos = transform.position;
        Mesh = Barrel.GetComponent<MeshRenderer>();
        CoolDown = 0;
        ShotNum = 0;

        CreatBaseBalls();
    }    

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * (Range + 1), CastSize * 2);
    }

    void Update()
    {
        if(ShotNum >= ShotNumber)
        {
            enabled = false;
            return;
        }

        if (!Moving)
        {
            if (CoolDown <= 0)
            {
                if (Physics.BoxCast(StartPos + Vector3.back, CastSize, Vector3.back, out RaycastHit hit, new Quaternion(), Range, PlayerLayer))
                {
                    Vector3 Pos = new Vector3(hit.transform.position.x, GroundCheck.position.y, transform.position.z);

                    if(Player == null)
                    {
                        Player = hit.transform;
                        InvokeRepeating("CheckObstPassed", PlayerPosCheckDelay, PlayerPosCheckDelay);
                    }

                    if (Physics.OverlapBox(Pos, CheckSize, new Quaternion(), GroundLayer).Length > 0)
                    {
                        Target = new Vector3(hit.transform.position.x, transform.position.y, transform.position.z);
                        Mesh.material = StaticData.Materials[Random.Range(0, StaticData.Materials.Count)];

                        Moving = true;
                        AudioManager.AudMan.Play("Splasher Moving");
                    }
                }
            }
            else
            {
                CoolDown -= Time.deltaTime;
            }
        }

    }

    void FixedUpdate()
    {
        if (Moving)
        {
            transform.position = Vector3.Lerp(transform.position, Target, MovingSpeed);

            if ((transform.position - Target).magnitude <= .2f)
            {
                //To see if I Will Instantiate a Ball or use an already instantiated one
                SplashBallBehavior ball = BaseBalls.Find(ball => !ball.gameObject.activeSelf);

                if (ball != null)
                {
                    ball.gameObject.SetActive(true);

                    ball.transform.position = Barrel.position + BarrelOffset;
                }
                else
                {
                    ball = Instantiate(SplashBall, Barrel.position + BarrelOffset, new Quaternion()).GetComponent<SplashBallBehavior>();

                    SetBallVars(ball, false);
                }

                ball.transform.GetChild(0).GetComponent<MeshRenderer>().material = Mesh.material;

                ball.start();

                ShotNum++;
                Moving = false;
                CoolDown = FireRate;
                AudioManager.AudMan.Stop("Splasher Moving");
                AudioManager.AudMan.Play("Splasher Fire", true);
            }
        }
    }

    void CreatBaseBalls()
    {
        BaseBalls = new List<SplashBallBehavior>(InitialBallsNum);

        for(int i = 0; i < InitialBallsNum; i++)
        {
            SplashBallBehavior ball = Instantiate(SplashBall, Barrel.position + BarrelOffset, new Quaternion()).GetComponent<SplashBallBehavior>();

            ball.gameObject.SetActive(false);

            SetBallVars(ball, true);

            BaseBalls.Add(ball);
        }
    }

    void SetBallVars(SplashBallBehavior ball, bool Needed)
    {
        ball.IsNeeded = Needed;
        ball.Speed = BallSpeed;
        ball.MaxDistance = MaxDistance;
        ball.transform.localScale *= ScalingFac;
    }

    void CheckObstPassed()
    {
        if(Player == null || Player.position.z - transform.position.z >= 3)
        {
            enabled = false;
            CancelInvoke("CheckObstPassed");
        }
    }

}
