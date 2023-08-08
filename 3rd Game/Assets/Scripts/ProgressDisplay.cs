using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressDisplay : MonoBehaviour
{
    public Transform FinishLine;

    private float EndLine;
    private Transform Player;
    private TextMeshProUGUI text;

    void Start()
    {
        EndLine = FinishLine.position.z - FinishLine.GetComponent<Collider>().bounds.extents.z;
        Player = Camera.main.GetComponent<CameraMovement>().Player;
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            text.text = Mathf.Clamp(Player.position.z * 100 / EndLine, 0, 100).ToString("0") + "%";
        }

    }
}
