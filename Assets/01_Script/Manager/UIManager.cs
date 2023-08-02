using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Text m_TxtTime;
    public Text m_TxtScore;
    public UIGauge m_UIHP;
    public void Init()
    {
        m_TxtScore.text = "0";
        m_UIHP.Init();
        m_UIHP.SetValue(1);
    }

    public void SetTime()
    {
        m_TxtTime.text = string.Format("{0}:{1}.{2}", GameTimeSystem.GetTime().Minutes.ToString("00"), GameTimeSystem.GetTime().Seconds.ToString("00"), GameTimeSystem.GetTime().Milliseconds.ToString("00"));
    }

    public void SetScore(int score)
    {
        m_TxtScore.text = score.ToString();
    }

    public void SetHP(float hpValue)
    {
        Debug.Log(hpValue);
        m_UIHP.SetValue(hpValue / DataManager.Instance.m_GlobalData.hp_max);
    }
}
