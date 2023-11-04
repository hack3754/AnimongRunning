using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIMain : UIObject
{
    public UIInGame m_UIMian;
    public void Init()
    {
        m_UIMian.Init();
    }

    public void Show()
    {
        SetActive(true);
    }

    public void Reset()
    {
        m_UIMian.Reset();
    }

    public void SetTime()
    {
        m_UIMian.SetTime();
    }

    public void SetScore(int score)
    {
        m_UIMian.SetScore(score);
    }

    public void SetHP(float hpValue)
    {
        m_UIMian.SetHP(hpValue);
    }
}
