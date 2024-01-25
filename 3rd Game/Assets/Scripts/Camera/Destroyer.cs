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

    void Start()
    {
        if (FixXAxe)
        {
            StartXPos = transform.position.x + Offset.x;
        }
    }

    void Update()
    {
        var cols = Physics.OverlapBox(new Vector3(FixXAxe ? StartXPos: transform.position.x + Offset.x,
                        Offset.y, transform.position.z + Offset.z), Size, new Quaternion(), ConcernedLayers);

        if(cols.Length > 0)
        {
            for (int i = 0; i< cols.Length; i++)
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
