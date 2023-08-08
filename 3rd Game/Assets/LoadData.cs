using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadData : MonoBehaviour
{
    public TextMeshProUGUI MoneyDis;
    public ShopManager ShopMan;
    public SettingsManager SettingsMan;

    void Awake()
    {
        SaveSystem.Load();

        MoneyDis.text = PlayerData.Money.ToString();

        ShopMan.LoadBoughtItems();

        SettingsMan.LoadSettings();
    }
    
}
