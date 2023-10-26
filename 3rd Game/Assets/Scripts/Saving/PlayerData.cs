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

    public static Dictionary<int, int> LvXStars = new Dictionary<int, int>() { {1 , 0} };

    public static List<int>[] CollectedStarsIndex = new List<int>[30];

    public static string CurrentSkin = string.Empty;

    public static string CurrentSkybox = "Mega Sun";

    public static List<string> Skins = new List<string>();

    public static List<string> Skyboxes = new List<string>() { "Mega Sun" };    

    //Data for Save
    public int MoneyData;
    public int CurrentLvData;
    public Dictionary<int, int> LvXStarsData = new Dictionary<int, int>(); 
    public List<int>[] CollectedStarsIndexData = new List<int>[30];
    public string CurrentSkinData;
    public string CurrentSkyboxData;
    public List<string> SkinsData = new List<string>();
    public List<string> SkyboxesData = new List<string>();
    public float SoundData;

    public PlayerData()
    {
        MoneyData = Money;
        CurrentLvData = CurrentLv;
        LvXStarsData = LvXStars;
        SkinsData = Skins;
        SkyboxesData = Skyboxes;
        SoundData = Sound;
        CurrentSkinData = CurrentSkin;
        CurrentSkyboxData = CurrentSkybox;
        CollectedStarsIndexData = CollectedStarsIndex;
    }

    public void Assign()
    {
        Money = MoneyData;
        CurrentLv = CurrentLvData;
        LvXStars = LvXStarsData;
        Skins = SkinsData;
        Skyboxes = SkyboxesData;
        Sound = SoundData;
        CurrentSkin = CurrentSkinData;
        CurrentSkybox = CurrentSkyboxData;
        CollectedStarsIndex = CollectedStarsIndexData;
    }
}
