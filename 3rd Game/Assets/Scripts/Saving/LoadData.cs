using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class LoadData : MonoBehaviour
{
    public TextMeshProUGUI MoneyDis;
    public ShopManager ShopMan;
    public SettingsManager SettingsMan;
    public LvsManager LvsMan;    

    [Space]
    [Tooltip("How Much Delay Between each Banner Loaded Check and if it's Loaded I Show the banner")]
    public float BannerDelay;

    void Awake()
    {
        //SaveSystem.Load();

        MoneyDis.text = PlayerData.Money.ToString();

        ShopMan.LoadBoughtItems();        

        LvsMan.LoadLvsData();

        SettingsMan.LoadSettings();     
    }

    void Start()
    {
        //I Put this On "Start" Bacause I can't change the AudioMixer value on "Awake" (Unity Problem)
        SettingsMan.LoadSettings();

        StartCoroutine(StartBanner(AdTypes.Banner_Android));
    }

    IEnumerator StartBanner(AdTypes Bannertype)
    {
        do
        {
            yield return new WaitForSeconds(BannerDelay);

            AdsManager.StartAd(Bannertype);

        } while (!Advertisement.Banner.isLoaded);

    }
}
