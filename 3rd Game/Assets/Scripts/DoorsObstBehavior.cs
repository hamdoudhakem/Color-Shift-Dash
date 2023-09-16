using System.Collections;
using System.Linq;
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
    [Tooltip("How Many doors the Player will have to pass")] [Range(2 , 10)]
    public int DoorsNum;
    [Header("Sound")]
    public AudioSource AudSource;
    [Tooltip("How Much Time Passes Between Each Call of the function that will Update the Audio Source position")]
    public float AudUpdateTime;

    private Transform Player;
    private DoorMovement[] Doors;
    private float DoorDis;
    private bool StartRemaiDoors;
    private float OffsetX;

    void Start()
    {
        StartRemaiDoors = false;

        //getting each door (I put a -1 because of the Audio Source)
        Doors = new DoorMovement[transform.childCount - 1];

        //Calculating how much the Doors Will offset Right and Left
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 15, GroundLayer);
        OffsetX = (hit.transform.localScale.x / 2) - (PassSize / 2) - .1f;

        //I will only get the doors that are active in case the Number
        //of doors i want is between 2 (Min) and 3 (cause the prefab contents 4 doors)
        for (int i = 0; i < Mathf.Clamp(DoorsNum,2, Doors.Length); i++)
        {
            Transform child = transform.GetChild(i);

            Doors[i] = child.GetComponent<DoorMovement>();            
            Doors[i].DrObs = this;
            
            Doors[i].SetPosition();
            Doors[i].OffsetX = OffsetX;                        
            
        }

        StartingSides();
        DoorDis = Doors[1].transform.position.z - Doors[0].transform.position.z;
    }

    private void StartingSides()
    {
        if (Random.Range(0, 2) == 1)
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

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.back * StartDistance, BoxSize * 2);
    }

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            if (!ScreensEventHandlers.IsPaused)
            {
                AudSource.UnPause();

                if (!StartRemaiDoors)
                {
                    Collider[] cols = Physics.OverlapBox(transform.position + Vector3.back * StartDistance, BoxSize, new Quaternion(), PlayerLayer);

                    if (cols.Length > 0)
                    {
                        Player = cols[0].transform;
                        StartRemaiDoors = true;

                        InvokeRepeating("UpdateAudPos", AudUpdateTime, AudUpdateTime);

                        StartCoroutine(DoIt());
                    }
                }
            }
            else
            {
                AudSource.Pause();
            }
        }
        else                    
        {
            CancelInvoke();
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
                Doors[i % Doors.Length].transform.position += DoorDis * Doors.Length * Vector3.forward;

                Doors[i % Doors.Length].ResetMats();
            }
            
        }
    }

    private void UpdateAudPos()
    {
        if(Player != null)
        {
            DoorMovement door = Min(Doors, door => Mathf.Abs(Player.position.z - door.transform.position.z));

            AudSource.transform.position = door.transform.position;
        }        
    }

    public T Min<T>(T[] hits, System.Func<T, float> selector)
    {
        float Min = selector(hits[0]), NewVal;
        int index = 0;

        for (int i = 1; i < hits.Length; i++)
        {
            NewVal = selector(hits[i]);

            if (NewVal < Min)
            {
                Min = NewVal;
                index = i;
            }
        }

        return hits[index];
    }
    
}

public enum Direction { Left = -1 , Right = 1}