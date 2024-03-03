using UnityEngine;
using TMPro;
using System;
using GooglePlayGames;

public class UIResult : UIObject
{
    public ButtonObject m_BtnAds;
    public ButtonObject m_BtnResult;
    public ButtonObject m_BtnHome;
    public ButtonObject m_BtnRestart;
    public ButtonObject m_BtnShare;
    public ButtonObject m_BtnRanking;

    public GameObject m_ObjBtnContinue;
    public GameObject m_ObjBtnResult;

    public GameObject m_ObjNewRanking;
    public GameObject m_ObjNormalRanking;
    public TMP_Text m_TxtTime;
    public TMP_Text m_TxtScore;
    public TMP_Text m_TxtLastScore;
    public TMP_Text m_TxtGold;

    public void Init()
    {
        m_BtnAds.m_FncOnClick = OnClickAds;

        m_BtnHome.m_FncOnClick = OnClickHome;
        m_BtnRestart.m_FncOnClick = OnClickRestart;

        m_BtnRanking.m_FncOnClick = OnClickRanking;
        m_BtnShare.m_FncOnClick = OnClickShare;
    }

    public void Show(TimeSpan time)
    {
        SetActive(true);
        m_TxtScore.text = ((int)GameData.m_Score).ToString();
        m_TxtTime.text = string.Format("{0}:{1}.{2}", time.Minutes.ToString("00"), time.Seconds.ToString("00"), ((int)(GameTimeSystem.GetTime().Milliseconds * 0.1f)).ToString("00"));
        m_TxtGold.text = GameData.m_Gold.ToString();
        m_ObjNewRanking.SetActive(GameData.m_Score > GameData.m_LocalData.m_Data.Score);
        m_ObjNormalRanking.SetActive(GameData.m_Score <= GameData.m_LocalData.m_Data.Score);

        GameData.m_LocalData.m_Data.SetScore();
        m_TxtLastScore.text = ((int)GameData.m_LocalData.m_Data.Score).ToString();

        GameData.m_LocalData.m_Data.Gold += GameData.m_Gold;
        GameData.m_LocalData.Save();

        GameManager.Instance.m_OutGameUI.SetCoin();

        Social.ReportScore((int)GameData.m_Score, "CgkIxYGzvLsTEAIQAQ", (bool success) => {
            
        });
    }

    public void Hide()
    {
        SetActive(false);
    }

    void OnClickHome()
    {
        GameManager.Instance.GameRestart();
    }

    void OnClickRestart()
    {
        //GameManager.Instance.GameRestart(false);
        GameManager.Instance.GameContinue();
    }

    void OnClickAds()
    {
        AdsManager.Instance.ShowRewardedAd(GetReward);
    }

    void GetReward()
    {
        GameData.m_IsContinue = true;
    }

    void OnClickRanking()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIxYGzvLsTEAIQAQ");
#endif
    }

    void OnClickShare()
    {
        ShareActivity("fadsf", "asfdgadf", "afeef");
    }

    private AndroidJavaObject activity = null;

    private void CreateActivity()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    if(activity == null)
        activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").
            GetStatic<AndroidJavaObject>("currentActivity");
#endif

    }

    public void ShareActivity(string title, string subject, string body)
    {
        CreateActivity();
        AndroidJavaClass intent = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObj = new AndroidJavaObject("android.content.Intent");
        intentObj.Call<AndroidJavaObject>("setAction", intent.GetStatic<string>("ACTION_SEND"));
        intentObj.Call<AndroidJavaObject>("setType", "text/plan");
        intentObj.Call<AndroidJavaObject>("putExtra", intent.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObj.Call<AndroidJavaObject>("putExtra", intent.GetStatic<string>("EXTRA_TEXT"), body);

        AndroidJavaObject chooser = intent.CallStatic<AndroidJavaObject>("createChooser", intentObj, title);
        activity.Call("startActivity", chooser);
    }
}
