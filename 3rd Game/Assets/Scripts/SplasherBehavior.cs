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

    private float CoolDown;
    private bool Moving;
    private Vector3 Target;
    private MeshRenderer Mesh;
    private int ShotNum;
    private Vector3 StartPos;

    void Start()
    {
        StartPos = transform.position;
        Mesh = Barrel.GetComponent<MeshRenderer>();
        CoolDown = 0;
        ShotNum = 0;
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

                    if (Physics.OverlapBox(Pos, CheckSize, new Quaternion(), GroundLayer).Length > 0)                    
                    {
                        Target = new Vector3(hit.transform.position.x, transform.position.y, transform.position.z);
                        Moving = true;
                        Mesh.material = StaticData.Materials[Random.Range(0, StaticData.Materials.Count)];
                    }
                }
            }
            else
            {
                CoolDown -= Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Target, MovingSpeed);

            if((transform.position - Target).magnitude <= .2f)
            {
                SplashBallBehavior ball = Instantiate(SplashBall, Barrel.position + BarrelOffset, new Quaternion()).GetComponent<SplashBallBehavior>();

                //Setting The Ball's Data
                ball.Speed = BallSpeed;
                ball.MaxDistance = MaxDistance;
                ball.transform.localScale *= ScalingFac;
                ball.transform.GetChild(0).GetComponent<MeshRenderer>().material = Mesh.material;

                ball.start();

                ShotNum++;
                Moving = false;
                CoolDown = FireRate;
            }
        }
       
       
    }
}
