using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplasherBehavior : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    public Transform Barrel;
    public GameObject SplashBall;
    [Tooltip("The Size of the Overlap Box that will check for Ground before changing the splasher position")]
    public Vector3 CheckSize;
    [Tooltip("The Size of the box cast that will check for the player")]
    public Vector3 CastSize;
    [Tooltip("The Speed at which the Cannon will move to position itself")]
    public float MovingSpeed;    
    
    [Tooltip("The Distance at which the player needs to be for the Splasher to start Shooting")]
    public float Range;
    public float FireRate;
    [Tooltip("How Many Shots before this stops working")] [Range(1 , 3)]
    public int ShotNumber;    

    private float CoolDown;
    private bool Moving;
    private Vector3 Target;
    private MeshRenderer Mesh;
    private int ShotNum;

    void Start()
    {
        Mesh = Barrel.GetComponent<MeshRenderer>();
        CoolDown = 0;
        ShotNum = 0;
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
                if (Physics.BoxCast(transform.position + Vector3.back * 1, CastSize, Vector3.back, out RaycastHit hit, new Quaternion(), Range, PlayerLayer))
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
                GameObject ball = Instantiate(SplashBall, Barrel.position + Vector3.back * .5f, new Quaternion());

                //Choosing a Random Color
                ball.transform.GetChild(0).GetComponent<MeshRenderer>().material = Mesh.material;
                ShotNum++;

                Moving = false;
                CoolDown = FireRate;
            }
        }
       
       
    }
}
