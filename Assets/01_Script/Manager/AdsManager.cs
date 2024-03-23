using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEditor.Rendering;
public enum AdsType
{
    Gold,
    CharSelect,
    Continue,
}

public class AdsManager : MSingleton<AdsManager>
{
    public bool IsTest;
    private string m_AdUnityId;
    private RewardedAd m_RewardedAd;
    Action m_FncGetReward;
    // Start is called before the first frame update

    void Start()
    {
        MobileAds.Initialize(GoogleAdsInit);
    }

    public void SetFncReward(Action fncGetReward)
    {
        m_FncGetReward = fncGetReward;
    }    

    void GoogleAdsInit(InitializationStatus status)
    {
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (m_RewardedAd != null)
        {
            m_RewardedAd.Destroy();
            m_RewardedAd = null;
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(m_AdUnityId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    return;
                }

                m_RewardedAd = ad;
                RegisterEventHandlers(m_RewardedAd);
            });
    }

    public void ShowRewardedAd(Action fncGetReward, AdsType adsType)
    {
#if UNITY_ANDROID
        if (IsTest)
        {
            m_AdUnityId = "ca-app-pub-3940256099942544/5224354917";
        }
        else
        {
            switch (adsType)
            {
                case AdsType.Gold: m_AdUnityId = "ca-app-pub-1490810296230779/3933294932"; break;
                case AdsType.CharSelect: m_AdUnityId = "ca-app-pub-1490810296230779/1306547469"; break;
                case AdsType.Continue: m_AdUnityId = "ca-app-pub-1490810296230779/5588570657"; break;
                default: m_AdUnityId = "ca-app-pub-3940256099942544/5224354917"; break;
            }
        }
#elif UNITY_IPHONE                                                
        if (IsTest)
        {
            m_AdUnityId = "ca-app-pub-3940256099942544/1712485313";
        }
#else
        m_AdUnityId = "unused";
#endif

        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (m_RewardedAd != null && m_RewardedAd.CanShowAd())
        {
            m_FncGetReward = fncGetReward;
            m_RewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            Debug.Log("ADS Load Fail");
            LoadRewardedAd();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            m_FncGetReward?.Invoke();
            m_FncGetReward = null;
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
           
        };
        
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
         
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadRewardedAd();
        };
    }
}
