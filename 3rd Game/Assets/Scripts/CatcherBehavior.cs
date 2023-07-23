using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherBehavior : MonoBehaviour
{
    [Tooltip("The Size of the Box Cast that will disable falling objects")]
    public Vector3 BoxSize;

    private Collider[] cols; 

    void Update()
    {
        cols = Physics.OverlapBox(transform.position, BoxSize);

        if (cols.Length > 0)
        {
            foreach(Collider col in cols)
            {
                col.gameObject.SetActive(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, BoxSize * 2);
    }
}
