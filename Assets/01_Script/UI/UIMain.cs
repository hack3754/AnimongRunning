using UnityEngine;
using GooglePlayGames;
public class UIMain : UIBase
{
    public ButtonObject m_BtnStart;
    public ButtonObject m_BtnShop;
    public ButtonObject m_BtnRanking;
    public ButtonObject m_BtnCharSelect;
    public ButtonObject m_BtnSetting;
    public ButtonObject m_BtnAD;
    System.Action m_FncStart;
    public void Init(System.Action fncStart)
    {
        base.Init();
        m_FncStart = fncStart;
        m_BtnStart.m_FncOnClick = OnClickStart;
        m_BtnShop.m_FncOnClick = OnClickShop;
        m_BtnRanking.m_FncOnClick = OnClickRanking;
        m_BtnCharSelect.m_FncOnClick = OnClickCharSelect;
        m_BtnAD.m_FncOnClick = OnClickAds;
        m_BtnSetting.m_FncOnClick = OnClickSetting;

    }
    public override void Show()
    {
        base.Show();
    }

    void OnClickStart()
    {
        m_FncStart?.Invoke();
    }

    void OnClickShop()
    {
        GameManager.Instance.m_OutGameUI.m_UIShop.Show();
    }

    void OnClickRanking()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIxYGzvLsTEAIQAQ");
#endif
    }
    void OnClickCharSelect()
    {
        GameManager.Instance.m_OutGameUI.m_UISelectChar.Show();
    }

    void OnClickAds()
    {
        AdsManager.Instance.ShowRewardedAd(GetReward);
    }

    void GetReward()
    {

    }

    void OnClickSetting()
    {
        GameManager.Instance.m_OutGameUI.m_UIPopupSetting.Show();
    }
}
