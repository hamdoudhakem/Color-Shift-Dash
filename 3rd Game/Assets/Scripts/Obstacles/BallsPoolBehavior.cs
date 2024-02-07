using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsPoolBehavior : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededMat;
    [Tooltip("How many Balls having the same color as the player ther will be per line")]
    [Range(5, 25)]
    public int NumPerLine;

    [Header("Constrains Player Y axe")]
    public LayerMask PlayerLayer;
    [Tooltip("The Offset of the OverLap Box center from this object position To Check if the Player is GONE not if he's COMING.")]
    public Vector3 Offset;
    [Tooltip("The Size of the Overlap Box tha will detect the Player")]
    public Vector3 BoxSize;

    [Header("Sound")]
    [Tooltip("The Layers That when in contact with this object will start emitting sounds")]
    public LayerMask SoundLayers;

    private bool IsIn;
    private Collider[] lastCols;
    private Collider[] PlayerCol = new Collider[1];  //Help When Using Non Alloc version of OverlapBox
    [HideInInspector] public int MovingBalls {

        get => movingBalls;

        set
        {
            movingBalls = value;
            CheckDragSound();
        } 
    }
    private int movingBalls;

    void Start()
    {        
        movingBalls = 0;
        IsIn = false;

        //Variables for coloring balls
        int x = StaticData.ChooseMat(NeededMat), y = StaticData.ChooseMat(NeededMat);

        int offset = Random.Range(0, transform.GetChild(0).childCount - NumPerLine);
        Direction StartSide = Random.Range(0, 2) == 0 ? Direction.Right : Direction.Left;

        //I give it this transform to stop the bug of this variable being unassigned
        Transform line = transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < transform.childCount)
            {
                line = transform.GetChild(i);
            }


            ColorLine(line, x, y, offset);

            if (offset >= line.childCount - NumPerLine)
            {
                StartSide = Direction.Left;
            }
            else if (offset == 0)
            {
                StartSide = Direction.Right;
            }

            offset += (int)StartSide;

        }

    }

    void ColorLine(Transform line, int x, int y, int offset)
    {
        for (int j = 0; j < line.childCount; j++)
        {
            Transform Ball = line.GetChild(j);
            Ball.GetComponent<BallMoving>().BallsPool = this;

            if (j < offset)
            {
                Ball.GetComponent<MeshRenderer>().material = StaticData.Materials[x];
            }
            else if (offset <= j && j < offset + NumPerLine)
            {
                Ball.GetComponent<MeshRenderer>().material = NeededMat;
            }
            else
            {
                Ball.GetComponent<MeshRenderer>().material = StaticData.Materials[y];
            }
        }
    }

    void OnDrawGizmos()
    {      
        Gizmos.DrawCube(transform.position + Offset, BoxSize * 2);
    }

    void Update()
    {
        if (!ScreensEventHandlers.IsPaused)
        {
            //This Code Will Freeze or Defreeze the Player Y axe Movement to Stop him from Going Above the Balls

            int colsNum = Physics.OverlapBoxNonAlloc(transform.position + Offset, BoxSize, PlayerCol, new Quaternion(), PlayerLayer);

            if (!IsIn)
            {
                if (colsNum > 0)
                {
                    PlayerCol[0].GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;

                    IsIn = true;
                }
            }
            else
            {
                if (colsNum <= 0)
                {
                    if (lastCols[0] != null)
                    {
                        lastCols[0].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
                    }
                    enabled = false;
                }
            }

            lastCols = PlayerCol;
        }        
    }

    void CheckDragSound()
    {
        if(MovingBalls > 0)
        {
            AudioManager.AudMan.Play("Drag Balls");
           // Debug.Log("Draging");
        }
        else
        {
            AudioManager.AudMan.Stop("Drag Balls");
            //Debug.Log("Stoped Draging");
        }
    }

}