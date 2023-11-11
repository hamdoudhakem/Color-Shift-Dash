using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTabs : MonoBehaviour
{
    public GameObject CurrentTab;

    public void SwitchTab(GameObject TargetTab)
    {
        CurrentTab.SetActive(false);
        TargetTab.SetActive(true);
        CurrentTab = TargetTab;
    }
}
