using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColParent 
{
    public void OnCollision(Collision collision);
    public void OnExitCollision(Collision collision);
}
