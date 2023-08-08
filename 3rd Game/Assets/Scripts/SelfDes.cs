using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDes : MonoBehaviour
{
    [Tooltip("How much Time before this Object Self Destroy (in seconds)")]
    public float time;

    void Start()
    {
        Invoke("Des", time);
    }

    public void Des()
    {
        Destroy(gameObject);
    }

    
}
