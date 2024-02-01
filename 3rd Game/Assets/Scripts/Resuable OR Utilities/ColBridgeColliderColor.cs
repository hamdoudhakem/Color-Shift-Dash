using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the Colliders of the Color Bridge To Help get 
/// the Touched Color from Their Parent (The Color Bridge)
/// </summary>

public class ColBridgeColliderColor : MonoBehaviour, IColliderColor
{    
    public Material GetColor()
    {
        return transform.parent.GetComponent<ColorBridgeBehavior>().GetTouchedColor(transform);
    }
}
