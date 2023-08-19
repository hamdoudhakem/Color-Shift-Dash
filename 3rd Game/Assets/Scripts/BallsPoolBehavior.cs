using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsPoolBehavior : MonoBehaviour, IObsTypes, IColParent
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    public Material NeededMat;     
    [Tooltip("How many Balls having the same color as the player ther will be per line")] [Range(5 , 25)]
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
    private Collider[] cols;
    private int ContactCount;    

    void Start()
    {
        //PlayerLayer = LayerMask.NameToLayer("Player");
        //SoundLayers = PlayerLayer | LayerMask.NameToLayer("Color Obst");
        ContactCount = 0;
        IsIn = false;

        //Variables for coloring balls
        int x = StaticData.ChooseMat(NeededMat), y = StaticData.ChooseMat(NeededMat);    

        int offset = Random.Range(0, transform.GetChild(0).childCount - NumPerLine);
        Direction StartSide = Random.Range(0, 2) == 0 ? Direction.Right : Direction.Left;

        //I give it this transform to stop the bug of this variable being unassigned
        Transform line = transform;

        for(int i = 0; i < transform.childCount; i++)
        {
            if(i < transform.childCount)
            {
                line = transform.GetChild(i);
            }


            ColorLine(line , x , y , offset);            

            if(offset >= line.childCount - NumPerLine)
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

    void ColorLine(Transform line, int x, int y , int offset)
    {
        for (int j = 0; j < line.childCount; j++)
        {
            Transform Ball = line.GetChild(j);
            Ball.GetComponent<Collided>().ColProcess = this;

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
        Gizmos.DrawCube(transform.position, BoxSize * 2);
        Gizmos.DrawCube(transform.position + Offset, BoxSize * 2);
    }

    void Update()
    {
        if (!IsIn)
        {
            cols = Physics.OverlapBox(transform.position, BoxSize, new Quaternion(), PlayerLayer);

            if (cols.Length > 0)
            {
                cols[0].GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;

                IsIn = true;
            }
        }
        else
        {
            cols = Physics.OverlapBox(transform.position + Offset, BoxSize, new Quaternion(), PlayerLayer);

            if (cols.Length > 0)
            {
                cols[0].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
                AudioManager.AudMan.Stop("Drag Balls");
                enabled = false;
            }
        }        
    }

    public void OnCollision(Collision collision)
    {
        if (SoundLayers == (SoundLayers | (1 << collision.gameObject.layer)) && !PlayerInteractions.Dead)
        {
            AudioManager.AudMan.Play("Hit Balls");
            ContactCount++;

            if (collision.transform.position.z < transform.position.z + Offset.z)
            {
                AudioManager.AudMan.Play("Drag Balls");
                Debug.Log("Drag !");

            }
        }
    }

    public void OnExitCollision(Collision collision)
    {
        
    }
}
