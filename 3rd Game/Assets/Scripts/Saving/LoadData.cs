using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadData : MonoBehaviour
{
    public TextMeshProUGUI MoneyDis;
    public ShopManager ShopMan;
    public SettingsManager SettingsMan;
    public LvsManager LvsMan;

    void Awake()
    {
        //SaveSystem.Load();

        MoneyDis.text = PlayerData.Money.ToString();

        ShopMan.LoadBoughtItems();

        SettingsMan.LoadSettings();

        LvsMan.LoadLvsData();
    }       

}
