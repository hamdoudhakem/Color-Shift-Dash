using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBallsBehavior : MonoBehaviour , IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    [Tooltip("The Distance the Player Needs To Be for the Falling to start happening")]
    public float StartDistance;
    [Tooltip("The Offset from this Object Center to the Overlap Box Center")]
    public Vector2 Offset;
    [Tooltip("The Size of the Overlap Box that Will Check for the Player presence")]
    public Vector3 BoxSize;
    [Tooltip("The Delay between the Each Fall of the Ball In Seconds")]
    public float Delay;
    [Tooltip("How Many Falls there Will Be")] [Range(3 , 10)]
    public int FallNum;

    private Rigidbody[] Balls;    
    private float BallDis;
    private float OffsetX;
    [Tooltip("This is Stop The Falling from Being one Sided Like All falling goes Right")]
    private int RightSideIndicator , LeftSideIndicator;
    private bool Started;

    private MeshRenderer[] Meshes;
    private Material Mat;

    void Start()
    {
        Started = false;
        RightSideIndicator = 0;
        LeftSideIndicator = 0;

        //Fill The Balls Array and a Meshes array in case The transparency is used to reset to Start Material
        Balls = new Rigidbody[transform.childCount];
        Meshes = new MeshRenderer[transform.childCount];

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i] = transform.GetChild(i).GetComponent<Rigidbody>();
            Meshes[i] = Balls[i].GetComponent<MeshRenderer>();
        }

        Mat = Meshes[0].material;

        BallDis = Mathf.Abs(Balls[1].position.z - Balls[0].position.z);

        Physics.Raycast(Balls[1].transform.position, Vector3.down ,out RaycastHit hit, 30f, GroundLayer);

        OffsetX = hit.transform.localScale.x / 4;

        for (int i = 0; i < Balls.Length; i++)
        {
            ChooseSide(Balls[i].transform);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * StartDistance + (Vector3)Offset, BoxSize * 2);
    }

    void Update()
    {
        if(!Started && Physics.OverlapBox(transform.position + Vector3.back * StartDistance + (Vector3)Offset ,
                              BoxSize ,new Quaternion() ,PlayerLayer).Length > 0)
        {
            Started = true;
            StartCoroutine(DoIt());
        }

        if (PlayerInteractions.Dead)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DoIt()
    {
        for(int i = 0; i < FallNum; i++)
        {
            if(i < Balls.Length)
            {
                Balls[i].useGravity = true;

                //ChooseSide(Balls[i].transform);

                yield return new WaitForSeconds(Delay);
            }
            else
            {
                Balls[i % Balls.Length].transform.position += new Vector3(0 , 10, BallDis * Balls.Length);

                Meshes[i % Balls.Length].material = Mat;

                ChooseSide(Balls[i % Balls.Length].transform);
                
                yield return new WaitForSeconds(Delay - .4f);
            }            

            
        }
    }

    void ChooseSide(Transform tran)
    {
        if(RightSideIndicator >= 2)
        {            
            GoLeft(tran);            
        }
        else if (LeftSideIndicator >= 2)
        {
            GoRight(tran);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                GoLeft(tran);
            }
            else
            {
                GoRight(tran);
            }
        }
       
    }

    void GoRight(Transform tran)
    {
        LeftSideIndicator = 0;
        tran.localPosition = new Vector3(OffsetX, tran.localPosition.y, tran.localPosition.z);
        RightSideIndicator += 1;
    }

    void GoLeft(Transform tran)
    {
        RightSideIndicator = 0;
        tran.localPosition = new Vector3(-OffsetX, tran.localPosition.y, tran.localPosition.z);
        LeftSideIndicator += 1;
    }
}
