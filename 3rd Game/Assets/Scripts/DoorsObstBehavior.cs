using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsObstBehavior : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public LayerMask PlayerLayer, GroundLayer;
    [Tooltip("The Distance that the player needs to be from this Obs to start moving the Doors")]
    public float StartDistance;
    [Tooltip("The SIze of the Overlap Box that will Check for the player")]
    public Vector3 BoxSize;
    [Tooltip("The Size of the Passage in a door")]
    public float PassSize;
    [Tooltip("The Movement Speed of the Doors")]
    public float Speed;
    [Tooltip("THe Time Between every Door Repositioning in seconds")]
    public float Delay;
    [Tooltip("How Many doors the Player will have to pass")] [Range(1 , 10)]
    public int DoorsNum;

    //[Tooltip("How Many doors will move from the started Before the player Reached the Start Distance")]
    //public int StartingDoorsNum;

    private DoorMovement[] Doors;
    private float DoorDis;
    private bool Stop;
    private float OffsetX;
    
    void Start()
    {
        Stop = false;

        //getting each door 
        Doors = new DoorMovement[transform.childCount];

        //Calculating how much the Doors Will offset Right and Left
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 15, GroundLayer);
        OffsetX = (hit.transform.localScale.x / 2) - (PassSize / 2) - .1f;


        //I will only get the doors that are active in case the Number
        //of doors i want is between 2 (Min) and 3 (cause the prefab contents 4 doors)
        for (int i = 0; i < Doors.Length; i++)
        {
            Transform child = transform.GetChild(i);

            Doors[i] = child.GetComponent<DoorMovement>();            
            Doors[i].DrObs = this;

            if (child.gameObject.activeSelf)
            {
                Doors[i].SetPosition();
                Doors[i].OffsetX = OffsetX;
            }                     
            
        }

        StartingSides();
        DoorDis = Doors[1].transform.position.z - Doors[0].transform.position.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * StartDistance, BoxSize * 2);
    }
    void Update()
    {
        if(!Stop && Physics.OverlapBox(transform.position + Vector3.back * StartDistance ,
                               BoxSize , new Quaternion() ,PlayerLayer).Length > 0)
        {
            Stop = true;
            StartCoroutine(DoIt());
        }

        if (PlayerInteractions.Dead)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DoIt()
    {
        for(int i = 2; i < DoorsNum; i++)
        {
            yield return new WaitForSeconds(Delay);

            if(i < Doors.Length)
            {
                if (Random.Range(0, 2) == 0)
                    Doors[i].Side = Direction.Right;
                else
                    Doors[i].Side = Direction.Left;


                Doors[i].enabled = true;
            }
            else
            {
                Doors[i % Doors.Length].transform.position += Vector3.forward * DoorDis * Doors.Length;

                Doors[i % Doors.Length].ResetMats();
            }
            
        }
    }

    private void StartingSides()
    {
        if(Random.Range(0 , 2) == 1)
        {               
            Doors[0].Side = Direction.Left;
            Doors[1].Side = Direction.Right;
        }
        else
        {
            Doors[1].Side = Direction.Left;
            Doors[0].Side = Direction.Right;
        }

        Doors[0].enabled = true; 
        Doors[1].enabled = true; 
    }
}

public enum Direction { Left = -1 , Right = 1}