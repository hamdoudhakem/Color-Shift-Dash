using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingEffect : MonoBehaviour
{
    [Tooltip("How much Time Between every Change In the Loading Text")]
    public float Time;

    private TextMeshProUGUI Display;    

    void Start()
    {
        Display = GetComponent<TextMeshProUGUI>();

        InvokeRepeating("Change", 0 , Time);
    }

    void Change()
    {
        if (Display.text == " Loading ...") 
        {
            Display.text = " Loading ";
        }
        else
        {
            Display.text += ".";
        }
    }
}
