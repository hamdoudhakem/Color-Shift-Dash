using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialCheckPo;

    private int CurPoint;

    /// <summary>
    /// Maybe I Will Trigger A Specific Cutscene Following Following the Player Reaching a certain Point
    /// </summary>

    void Awake()
    {
        if (!PlayerData.FirstTime)
        {
            Destroy(TutorialCheckPo);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurPoint = 0;
    }

    void Update()
    {
        
    }
}
