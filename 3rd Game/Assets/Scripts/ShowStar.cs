using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStar : MonoBehaviour
{
    public void ShowNewStar()
    {
        transform.GetChild(PlayerInteractions.StarsNum - 1).gameObject.SetActive(true);
    }
}
