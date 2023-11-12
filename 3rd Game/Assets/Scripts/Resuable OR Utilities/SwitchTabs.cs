using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchTabs : MonoBehaviour
{
    [field: SerializeField]
    public GameObject CurrentTab { get; private set; }

    public GameObjectXText TabXText;

    [Header("Colors")]
    public Color CurrentTabColor;
    public Color OtherTabsColor;

    public void SwitchTab(GameObject TargetTab)
    {
        if(CurrentTab != TargetTab)
        {
            CurrentTab.SetActive(false);
            TabXText[CurrentTab].color = OtherTabsColor;

            TargetTab.SetActive(true);
            TabXText[TargetTab].color = CurrentTabColor;

            CurrentTab = TargetTab;
        }       
    }
}

[Serializable]
public class GameObjectXText : SerializableDictionaryBase<GameObject, TextMeshProUGUI> { }