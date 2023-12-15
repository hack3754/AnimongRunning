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

    public void Init()
    {
        m_BtnHome.m_FncOnClick = OnClickHome;
    }

    public void Show(int score, TimeSpan time)
    {
        SetActive(true);
        m_TxtScore.text = score.ToString();
        m_TxtTime.text = string.Format("{0}:{1}.{2}", time.Minutes.ToString("00"), time.Seconds.ToString("00"), ((int)(GameTimeSystem.GetTime().Milliseconds * 0.1f)).ToString("00"));
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
