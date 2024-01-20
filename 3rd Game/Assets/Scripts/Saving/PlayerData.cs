using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //Data for Use
    public static int Money = 900/*0*/;

    public static int CurrentLv = 1;

    public static float Sound = 1;

    public static bool FirstTime = false/*true*/;

    public static bool Vibrations = false;

    public static Dictionary<int, int> LvXStars = new Dictionary<int, int>() { {1 , 0} };

    public static List<int>[] CollectedStarsIndex = new List<int>[30];

    public static string CurrentSkin = string.Empty;

    public static string CurrentSkybox = "Mega Sun";

    public static List<string> Skins = new List<string>();

    public static List<string> Skyboxes = new List<string>() { "Mega Sun" };

    public static Dictionary<string, int> ItemXRemainAds = new Dictionary<string, int>() { { "BasketBall", 2 }, { "FootBall", 2 }, { "VolleyBall", 2 }, { "Earth",2 }, { "Mars",2 }, { "Moon Night",2 }  };

    public static Dictionary<int, bool> LvXTuToUsed = new Dictionary<int, bool>() { { 1, false }, { 2, false}, {7, false } };

    //Data for Save
    public int MoneyData;
    public int CurrentLvData;
    public Dictionary<int, bool> LvXTuToUsedData;
    public Dictionary<int, int> LvXStarsData; 
    public List<int>[] CollectedStarsIndexData;
    public string CurrentSkinData;
    public string CurrentSkyboxData;
    public List<string> SkinsData = new List<string>();
    public List<string> SkyboxesData = new List<string>();
    public Dictionary<string, int> ItemXRemainAdsData;
    public float SoundData;
    public bool VibrationsData;
    public bool FirstTimeData;

    public PlayerData()
    {
        MoneyData = Money;
        CurrentLvData = CurrentLv;
        LvXStarsData = LvXStars;
        SkinsData = Skins;
        SkyboxesData = Skyboxes;
        SoundData = Sound;
        VibrationsData = Vibrations;
        FirstTimeData = FirstTime;
        CurrentSkinData = CurrentSkin;
        CurrentSkyboxData = CurrentSkybox;
        CollectedStarsIndexData = CollectedStarsIndex;
        LvXTuToUsedData = LvXTuToUsed;
        ItemXRemainAdsData = ItemXRemainAds;
    }

    public void Assign()
    {
        Money = MoneyData;
        CurrentLv = CurrentLvData;
        LvXStars = LvXStarsData;
        Skins = SkinsData;
        Skyboxes = SkyboxesData;
        Sound = SoundData;
        Vibrations = VibrationsData;
        FirstTime = FirstTimeData;
        CurrentSkin = CurrentSkinData;
        CurrentSkybox = CurrentSkyboxData;
        CollectedStarsIndex = CollectedStarsIndexData;
        LvXTuToUsed = LvXTuToUsedData;
        ItemXRemainAds = ItemXRemainAdsData;
    }
}
