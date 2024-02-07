using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Tooltip("The Layers that will be destroyed upon touch")]
    public LayerMask ConcernedLayers;
    [Tooltip("The Destroyer Overlapbox Offset from the Shake Holder (Except the Y which will be static and in WolrdPoint not relative to this Object)")]
    public Vector3 Offset;
    public Vector3 Size;
    public bool FixXAxe = false;

    private float StartXPos;
    private Collider[] cols = new Collider[10];  //Help When Using Non Alloc versions of Overlaps and Casts

    void Start()
    {
        if (FixXAxe)
        {
            StartXPos = transform.position.x + Offset.x;
        }
    }

    void Update()
    {
        int colsNum = Physics.OverlapBoxNonAlloc(new Vector3(FixXAxe ? StartXPos: transform.position.x + Offset.x,
                        Offset.y, transform.position.z + Offset.z), Size,cols, new Quaternion(), ConcernedLayers);

        if(colsNum > 0)
        {
            for (int i = 0; i< colsNum; i++)
            {
                Destroy(cols[i].gameObject);
            }
        }   
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(new Vector3(transform.position.x + Offset.x, Offset.y,
                            transform.position.z + Offset.z), Size * 2);
    }
}
