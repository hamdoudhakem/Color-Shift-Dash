using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FallingBallsBehavior : MonoBehaviour , IObsTypes, IColParent
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public LayerMask PlayerLayer;
    public LayerMask GroundLayer;
    public GameObject FallenEffect;
    [Tooltip("The Distance the Player Needs To Be for the Falling to start happening")]
    public float StartDistance;
    [Tooltip("The Offset from this Object Center to the Overlap Box Center")]
    public Vector2 Offset;
    [Tooltip("The Size of the Overlap Box that Will Check for the Player presence")]
    public Vector3 BoxSize;
    [Tooltip("The Distance the player will need to be from a falling ball to hear it's fallen sound")]
    public float SoundRadius;
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

    private ParticleSystem[] Effects;
    private int BaseEffectsInsta, CurEffIndex;
    private Transform Player;
    private float BallHalfSize;
 
    void Start()
    {
        Started = false;
        RightSideIndicator = 0;
        LeftSideIndicator = 0;
        BaseEffectsInsta = 2;
        CurEffIndex = 0;
        BallHalfSize = (transform.GetChild(0).lossyScale.y / 2) - .1f;

        //Find and Assign The Offset
        Physics.Raycast(transform.GetChild(1).position, Vector3.down, out RaycastHit hit, 30f, GroundLayer);

        OffsetX = hit.transform.localScale.x / 4;

        //Fill The Balls Array and a Meshes array in case The transparency is used to reset to Start Material
        Balls = new Rigidbody[transform.childCount];
        Meshes = new MeshRenderer[transform.childCount];        

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i] = transform.GetChild(i).GetComponent<Rigidbody>();
            Meshes[i] = Balls[i].GetComponent<MeshRenderer>();
            ChooseSide(Balls[i].transform);

            Balls[i].GetComponent<Collided>().ColProcess = this;
        }

        Mat = Meshes[0].material;

        BallDis = Mathf.Abs(Balls[1].position.z - Balls[0].position.z);

        CreatBaseEffects();
    }   

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * StartDistance + (Vector3)Offset, BoxSize * 2);
    }

    void Update()
    {
        if(!Started)
        {
            Collider[] cols = Physics.OverlapBox(transform.position + Vector3.back * StartDistance + (Vector3)Offset,
                              BoxSize, new Quaternion(), PlayerLayer);

            if(cols.Length > 0)
            {
                Player = cols[0].transform;
                Started = true;
                StartCoroutine(DoIt());
            }
           
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

    #region Positioning Balls Methods

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

    #endregion

    #region IColParent Interface Methods

    public void OnCollision(Collision collision)
    {
        int TouchedLayer = (int)Mathf.Pow(2,collision.gameObject.layer);

        if (TouchedLayer == PlayerLayer || TouchedLayer == GroundLayer)
        {
            ContactPoint cont = collision.GetContact(0);
            Transform currBall = cont.thisCollider.transform;
           
            //To See if I *LANDED* Only then will I Have the Right to shake and make sound
            if (cont.point.y < currBall.position.y - BallHalfSize)
            {
                //To See if the player Is Too fare away to make any sound or shake
                if(Player != null && Player.position.z - currBall.position.z < SoundRadius)
                {
                    AudioManager.AudMan.Play("Ball Falls", true);
                    CameraShaker.Instance.ShakeOnce(4, 4, .1f, .5f);
                    ActivateEff(collision.GetContact(0).point);
                }
                
            }
            
        }
    }

    public void OnExitCollision(Collision collision)
    {

    }

    #endregion

    #region Fall Effect Methods

    void CreatBaseEffects()
    {
        Effects = new ParticleSystem[BaseEffectsInsta];

        for (int i = 0; i < BaseEffectsInsta; i++)
        {
            ParticleSystem part = Instantiate(FallenEffect, transform.position, new Quaternion(),transform).GetComponent<ParticleSystem>();

            part.gameObject.SetActive(false);

            part.transform.Rotate(-90, 0, 0);

            Effects[i] = part;
        }
    }

    void ActivateEff(Vector3 pos)
    {
        ParticleSystem part = Effects[CurEffIndex % BaseEffectsInsta];

        CurEffIndex++;

        part.transform.position = pos + Vector3.up * .12f;

        part.gameObject.SetActive(true);

        part.Play();
    }

    #endregion
}
