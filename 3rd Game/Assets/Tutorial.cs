using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public Transform Player;
    public Transform TutorialCheckPo;

    [Tooltip("I Will Invoke this Event to set up the basics for showing the tutorial (black background, features ...)")]
    public UnityEvent BaseSetUp;

    private Transform CurPoint;
    private int CurPointIndex;
    private bool InTuto;

    /// <summary>
    /// Maybe I Will Trigger A Specific Cutscene Following Following the Player Reaching a certain Point
    /// </summary>

    void Awake()
    {
        if (!PlayerData.FirstTime)
        {
            Destroy(TutorialCheckPo.gameObject);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InTuto = false;
        CurPointIndex = 0;
        CurPoint = TutorialCheckPo.transform.GetChild(CurPointIndex);
    }

    void Update()
    {
        if (!InTuto)
        {
            if (Player.transform.position.z - CurPoint.position.z >= 0)
            {
                InTuto = true;

                BaseSetUp.Invoke();
            }
        }
       
    }
}
