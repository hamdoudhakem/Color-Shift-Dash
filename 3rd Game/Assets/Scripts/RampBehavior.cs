using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBehavior : MonoBehaviour
{
    [Tooltip("The Overlap Box That will tell the ramp when to disappear")]
    public Vector3 OverlapBox;
    public LayerMask PlayerMask;

   
    void Update()
    {               
        if(Physics.OverlapBox(transform.position + new Vector3(0, 2.2f, 5.1f), OverlapBox, new Quaternion(), PlayerMask).Length > 0)
        {
            Destroy(gameObject);
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position + new Vector3(0 , 2.2f , 5.1f), OverlapBox);
    //}

  
}
