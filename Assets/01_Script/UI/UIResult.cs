using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIResult : UIObject
{
    public ButtonObject m_BtnHome;
    public TMP_Text m_TxtTime;
    public TMP_Text m_TxtScore;
    public TMP_Text m_TxtLastScore;
    public TMP_Text m_TxtGold;

    public void Init()
    {
        m_BtnHome.m_FncOnClick = OnClickHome;
    }

    public void Show(TimeSpan time)
    {
        SetActive(true);
        m_TxtScore.text = ((int)GameData.m_Score).ToString();
        m_TxtTime.text = string.Format("{0}:{1}.{2}", time.Minutes.ToString("00"), time.Seconds.ToString("00"), ((int)(GameTimeSystem.GetTime().Milliseconds * 0.1f)).ToString("00"));
        m_TxtGold.text = GameData.m_Gold.ToString();
        m_TxtLastScore.text = ((int)GameData.m_LocalData.m_Data.Score).ToString();
    }

    public void Hide()
    {
        SetActive(false);
    }

    void OnClickHome()
    {
        GameManager.Instance.GameRestart();
    }
}
