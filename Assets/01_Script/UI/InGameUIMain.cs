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
        m_UIMian.m_Obj.SetActive(true);
    }

    public void GameReset()
    {
        SetActive(false);
        m_ObjBG.SetActive(false);
        m_UIResult.Hide();
        m_UIMian.GameReset();
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
        GameData.m_LocalData.m_Data.SetScore();

        m_ObjBG.SetActive(true);
        m_UIMian.m_Obj.SetActive(false);
        m_UIResult.Show(time);
    }
}
