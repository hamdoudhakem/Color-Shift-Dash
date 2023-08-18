using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collided : MonoBehaviour
{
    [HideInInspector] public IColParent ColProcess;

    private bool AlreadyIn1, AlreadyIn2;

    void Start()
    {
        AlreadyIn1 = false;
        AlreadyIn2 = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!AlreadyIn1)
        {
            AlreadyIn1 = true;

            if (ColProcess != null)
            {
                ColProcess.OnCollision(collision);
            }

            AlreadyIn1 = false;
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        if (!AlreadyIn2)
        {
            AlreadyIn2 = true;

            if (ColProcess != null)
            {
                ColProcess.OnExitCollision(collision);
            }

            AlreadyIn2 = false;
        }
    }
}

