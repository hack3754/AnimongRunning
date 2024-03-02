using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameUIMain : UIObject
{
    public UIInGame m_UIMian;
    public UIResult m_UIResult;
    public GameObject m_ObjBG;
    public void Init()
    {
        m_UIMian.Init();
        m_UIResult.Init();
        m_ObjBG.SetActive(false);
        m_UIResult.Hide();
    }

    public void Show()
    {
        SetActive(true);
        m_UIMian.SetActive(true);
        m_UIMian.SetController();
    }

    public void GameReset()
    {
        SetActive(false);
        m_ObjBG.SetActive(false);
        m_UIResult.Hide();
        m_UIMian.GameReset();
    }

    public void GameContinue()
    {
        m_ObjBG.SetActive(false);
        m_UIMian.SetActive(true);
        m_UIMian.GameContinue();
        m_UIResult.Hide();
    }

    public void SetTime()
    {
        m_UIMian.SetTime();
    }

    public void SetScore(int score)
    {
        m_UIMian.SetScore(score);
    }

    public void SetGold(int gold)
    {
        m_UIMian.SetGold(gold);
    }

    public void SetHP(float hpValue)
    {
        m_UIMian.SetHP(hpValue);
    }

    public void ShowResult(System.TimeSpan time)
    {
        m_ObjBG.SetActive(true);
        m_UIMian.SetActive(false);
        m_UIResult.Show(time);
    }
}
