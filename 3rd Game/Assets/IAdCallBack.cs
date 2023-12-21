using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdCallBack
{
    
    public void Reward();  //For Rewarded Ads generally Called when the Ad Finishes Normally
    public void ResumeGame();  //For Interstitial Ads generally Called when the Ad is eather skipped or finishes
}
