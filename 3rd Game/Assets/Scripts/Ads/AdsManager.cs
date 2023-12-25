using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private static AdsManager Ads { get; set; }

    [SerializeField] private string GameIDAndroid;

    [SerializeField] private bool testMode = true;

    private IAdCallBack CallBack;

    /// <summary>
    /// Each Lv have it's Own Ads Manager Connected to the Specific Resume Game Event (Meaning
    /// each Lv have it's Event Assigned so if I use "Don't Destroy on load"
    /// it won't be assigned in the Next Lv)
    /// </summary>

    void Start()
    {
        Ads = this;

        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(GameIDAndroid, testMode);           
        }        

        Advertisement.AddListener(this);

        DontDestroyOnLoad(gameObject);
    }

    public static void StartAd(AdTypes adtype, IAdCallBack callBack = null)
    {
        if(Advertisement.isInitialized)
        {
            string adUnit;

            switch (adtype)
            {
                case AdTypes.Interstitial_Android:
                    adUnit = "Interstitial_Android";
                        break;
                case AdTypes.Rewarded_Android:
                    adUnit = "Rewarded_Android";
                        break;
                case AdTypes.Banner_Android:
                    adUnit = "Banner_Android";
                        break;
                default:
                    adUnit = "Interstitial_Android";
                        break;
            }           
                       
            if (Advertisement.IsReady(adUnit))
            {
                if (adtype == AdTypes.Banner_Android)
                {
                    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_LEFT);
                    Advertisement.Banner.Show(adUnit);
                }
                else
                {
                    Advertisement.Show(adUnit);
                    Ads.CallBack = callBack;
                }
                
            }
        }
    }

    public static void HideBanner()
    {
        //Hide it when entering a lv
        Advertisement.Banner.Hide();
    }

    #region IUnityAdsListener Methods

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //Debug.Log("Ad Finished and ID is : " + placementId);

        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            //For Reward Ads that were Played till the End

            Debug.Log("Reward");
            CallBack.Reward();
        }
        else if (placementId == "Interstitial_Android")
        {
            //For Interstitial Ads that were eather Skipped or finished Normally

            Debug.Log("Finshed Ad and Now UnPaused !");
            CallBack.ResumeGame();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        //For when the Ad didn't load or something went wrong

        Debug.Log("ERROR : " + message);
        CallBack.Error();
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }   

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    #endregion
}

public enum AdTypes { Interstitial_Android, Rewarded_Android, Banner_Android }
