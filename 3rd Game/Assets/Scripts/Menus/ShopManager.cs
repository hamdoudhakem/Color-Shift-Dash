using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI SkinsHeader, SkyboxesHeader, MoneyDisplay;

    public Transform Skins;
    public Transform Skyboxes;
    [Tooltip("The Frame Color of and Equiped Item")]
    public Color EquipedCol;

    public GameObject ConfirmBuyPanel;
    public GameObject NoMoneyPanel;

    private TextMeshProUGUI LastEquipedSkin;
    private TextMeshProUGUI LastEquipedSkybox;
    private TextMeshProUGUI ItemToBuy;      

    public void LoadBoughtItems()
    {
        LastEquipedSkin = null;
        LastEquipedSkybox = Skyboxes.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        for (int i = 0; i < Mathf.Max(Skins.childCount , Skyboxes.childCount) ; i++)
        {
            if(i < Skins.childCount)
            {
                Transform skin = Skins.GetChild(i);

                if (PlayerData.Skins.Contains(skin.name))
                {
                    TextMeshProUGUI Display = skin.GetComponentInChildren<TextMeshProUGUI>();

                    if (PlayerData.CurrentSkin == skin.name)
                    {
                        skin.GetComponent<Image>().color = EquipedCol;
                        Display.text = "Equiped";
                        LastEquipedSkin = Display;
                        DisableAdIcon(skin, Display);
                    }
                    else
                    {
                        Display.text = "Equip";
                        DisableAdIcon(skin, Display);
                    }

                }
            }

            if(i < Skyboxes.childCount)
            {
                Transform skybox = Skyboxes.GetChild(i);

                if (PlayerData.Skyboxes.Contains(skybox.name))
                {
                    TextMeshProUGUI Display = skybox.GetComponentInChildren<TextMeshProUGUI>();

                    if (PlayerData.CurrentSkybox == skybox.name)
                    {
                        skybox.GetComponent<Image>().color = EquipedCol;
                        Display.text = "Equiped";
                        LastEquipedSkybox = Display;
                        DisableAdIcon(skybox, Display);
                    }
                    else
                    {
                        Display.text = "Equip";
                        DisableAdIcon(skybox, Display);
                    }

                }
            }
            
        }       
    }

    void DisableAdIcon(Transform Item, TextMeshProUGUI text)
    {
        if (PlayerData.ItemXRemainAds.ContainsKey(Item.name))
        {
            text.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void BuyOrEquipSkin(TextMeshProUGUI Item)
    {
        string skinName = Item.transform.parent.name;

        MainMenuAudioMan.MaAud.SelectOrEquip.Play();

        if (PlayerData.Skins.Contains(skinName))
        {
            if(PlayerData.CurrentSkin == skinName)
            {
                ///To Unequip and use Default
                LastEquipedSkin.text = "Equip";
                LastEquipedSkin.GetComponentInParent<Image>().color = Color.white;

                PlayerData.CurrentSkin = string.Empty;
            }
            else
            {
                ///To Equip
                if(LastEquipedSkin != null)
                {
                    LastEquipedSkin.text = "Equip";
                    LastEquipedSkin.GetComponentInParent<Image>().color = Color.white;
                }               

                Item.text = "Equiped";
                Item.GetComponentInParent<Image>().color = EquipedCol;
                LastEquipedSkin = Item;

                PlayerData.CurrentSkin = skinName;
            }
            
            SaveSystem.Save();
        }
        else
        {
            int Cost = int.Parse(Item.text);

            if (PlayerData.Money >= Cost)
            {
                ItemToBuy = Item;
                ConfirmBuyPanel.SetActive(true);
            }
            else
            {
                NoMoneyPanel.SetActive(true);
            }
            
        }
    }

    public void BuyOrEquipSkybox(TextMeshProUGUI Item)
    {
        string skyboxName = Item.transform.parent.name;

        MainMenuAudioMan.MaAud.SelectOrEquip.Play();

        if (PlayerData.Skyboxes.Contains(skyboxName))
        {           
            LastEquipedSkybox.text = "Equip";
            LastEquipedSkybox.GetComponentInParent<Image>().color = Color.white;                       

            Item.text = "Equiped";
            Item.GetComponentInParent<Image>().color = EquipedCol;
            LastEquipedSkybox = Item;

            PlayerData.CurrentSkybox = skyboxName;
            SaveSystem.Save();
        }
        else
        {
            int Cost = int.Parse(Item.text);

            if (PlayerData.Money >= Cost)
            {
                ItemToBuy = Item;
                ConfirmBuyPanel.SetActive(true);
            }
            else
            {
                NoMoneyPanel.SetActive(true);
            }
            
        }
    }
    
    public void Buy()
    {
        int Cost = int.Parse(ItemToBuy.text);

        ItemToBuy.text = "Equip";

        PlayerData.Money -= Cost;
        MoneyDisplay.text = PlayerData.Money.ToString();

        MainMenuAudioMan.MaAud.Buy.Play();

        string type = ItemToBuy.transform.parent.parent.tag;

        if(type == "Skins")
        {
            PlayerData.Skins.Add(ItemToBuy.transform.parent.name);
        }
        else if (type == "Skyboxes")
        {
            PlayerData.Skyboxes.Add(ItemToBuy.transform.parent.name);
        }

        SaveSystem.Save();
    }
}
