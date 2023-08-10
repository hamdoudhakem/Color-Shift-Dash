using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStar : MonoBehaviour
{
    public void ShowNewStar(int index)
    {
        transform.GetChild(index - 1).gameObject.SetActive(true);
    }
}
