using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour, IAdCallBack
{
    public TextMeshProUGUI SkinsHeader, SkyboxesHeader, MoneyDisplay;

    public Transform Skins;
    public Transform Skyboxes;
    [Tooltip("The Frame Color of and Equiped Item")]
    public Color EquipedCol;

    public GameObject ConfirmBuyPanel;
    public GameObject NoMoneyPanel;

    [Tooltip("The Offset for the TextMeshPro of the Ad only items after Buying")]
    public Vector2 TextOffsetAfterBuying;

    private TextMeshProUGUI LastEquipedSkin;
    private TextMeshProUGUI LastEquipedSkybox;
    private TextMeshProUGUI ItemToBuy;

    private string ItemToBuyName;
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

                    FixTextForAdOnlyItems(Display, skin.name);                    

                    if (PlayerData.CurrentSkin == skin.name)
                    {
                        skin.GetComponent<Image>().color = EquipedCol;
                        Display.text = "Equiped";
                        LastEquipedSkin = Display;
                    }
                    else
                    {
                        Display.text = "Equip";
                    }

                }
            }

            if(i < Skyboxes.childCount)
            {
                Transform skybox = Skyboxes.GetChild(i);

                if (PlayerData.Skyboxes.Contains(skybox.name))
                {
                    TextMeshProUGUI Display = skybox.GetComponentInChildren<TextMeshProUGUI>();

                    FixTextForAdOnlyItems(Display, skybox.name);

                    if (PlayerData.CurrentSkybox == skybox.name)
                    {
                        Debug.Log("This Display is for skybox = " + skybox.name + "and it's parent is :"+Display.transform.parent);
                        Display.transform.parent.GetComponent<Image>().color = EquipedCol;
                        Display.text = "Equiped";
                        LastEquipedSkybox = Display;
                    }
                    else
                    {
                        Display.text = "Equip";
                    }

                }
            }
            
        }
        
        foreach(KeyValuePair<string, int> ItemXAds in PlayerData.ItemXRemainAds)
        {
            if (ItemXAds.Value > 0)
            {
                //I Take the Transform of the Skin or Skybox if it's null
                Transform val = Skins.Find(ItemXAds.Key);

                if (val == null)
                {
                    val = Skyboxes.Find(ItemXAds.Key);
                }

                TextMeshProUGUI text = val.GetComponentInChildren<TextMeshProUGUI>();

                text.text = "x" + ItemXAds.Value;
            }
        }
    }

    //After Buying an Ad only Item, I Destroy the Ad Icon and Move the TextMeshPro to the right
    private void FixTextForAdOnlyItems(TextMeshProUGUI Display, string ItemName)
    {
        if (PlayerData.ItemXRemainAds.ContainsKey(ItemName))
        {            
            //If He Already Bought the element
            Destroy(Display.transform.GetChild(0).gameObject);
            Display.transform.position += (Vector3)TextOffsetAfterBuying;            
            
        }
    }      

    public void BuyOrEquipSkin(TextMeshProUGUI Item)
    {
        string skinName = Item.transform.parent.name;
        ItemToBuyName = skinName;

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
            if (PlayerData.ItemXRemainAds.ContainsKey(skinName))
            {
                //Buy Using Ads
                LaunchRewarededAd(Item);
            }
            else
            {
                //Buy Using Money
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
    }    

    public void BuyOrEquipSkybox(TextMeshProUGUI Item)
    {
        string skyboxName = Item.transform.parent.parent.name;
        ItemToBuyName = skyboxName;

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
            if(PlayerData.ItemXRemainAds.ContainsKey(skyboxName))
            {
                //Buy Using Ads
                LaunchRewarededAd(Item);
            }
            else
            {
                //Buy Using Money 
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
    }

    private void LaunchRewarededAd(TextMeshProUGUI Item)
    {
        ItemToBuy = Item;
        AdsManager.StartAd(AdTypes.Rewarded_Android, this);
    }

    #region IAdCallBack Interface Methods

    public void Reward()
    {
        int remainAds = --PlayerData.ItemXRemainAds[ItemToBuyName];
                
        if(remainAds > 0)
        {
            ItemToBuy.text = "x" + remainAds;
        }
        else
        {
            //He watched all Ads to unlock this Item

            Destroy(ItemToBuy.transform.GetChild(0).gameObject);
            ItemToBuy.transform.position += (Vector3)TextOffsetAfterBuying;
            ItemToBuy.text = "Equip";

            MainMenuAudioMan.MaAud.Buy.Play();

            UpdatePossesedItems(ItemToBuy);            
        }

        SaveSystem.Save();
    }

    public void ResumeGame()
    {
        
    }

    #endregion

    public void Buy()
    {
        int Cost = int.Parse(ItemToBuy.text);

        ItemToBuy.text = "Equip";

        PlayerData.Money -= Cost;
        MoneyDisplay.text = PlayerData.Money.ToString();

        MainMenuAudioMan.MaAud.Buy.Play();        

        UpdatePossesedItems(ItemToBuy);

        SaveSystem.Save();
    }

    private void UpdatePossesedItems(TextMeshProUGUI ItemToBuy)
    {
        string type = ItemToBuy.transform.parent.parent.tag;

        if (type == "Skins")
        {
            PlayerData.Skins.Add(ItemToBuyName);
        }
        else if (type == "Skyboxes")
        {
            PlayerData.Skyboxes.Add(ItemToBuyName);
        }
    }
}
