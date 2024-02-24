using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupSetting : UIPopupSelectController
{
    public GameObject[] m_ObjEnableSound;
    public ButtonObject[] m_ObjDisableSound;
    public ButtonObject m_BtnSave;
    public ButtonObject m_BtnClose;

    int m_SoundSetting;

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < m_ObjDisableSound.Length; i++)
        {
            int index = i;
            m_ObjDisableSound[i].m_FncOnClick = () => OnClickSoundOnOff(index);
        }

        m_BtnSave.m_FncOnClick = OnClickSave;
        m_BtnClose.m_FncOnClick = OnClickClose;
    }

    public override void Show()
    {
        base.Show();

        m_SoundSetting = GameData.m_LocalData.m_Data.m_SoundSetting;

        for (int i = 0; i < m_ObjEnable.Length; i++)
        {
            m_ObjEnableSound[i].SetActive(i == m_SoundSetting);
            m_ObjDisableSound[i].SetActive(i != m_SoundSetting);
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    void OnClickSoundOnOff(int index)//index == 0 : on , index == 1 : off 
    {
        for (int i = 0; i < m_ObjEnable.Length; i++)
        {
            m_ObjEnableSound[i].SetActive(i == index);
            m_ObjDisableSound[i].SetActive(i != index);
        }

        m_SoundSetting = index;
    }

    void OnClickClose()
    {
        Hide();
    }
    void OnClickSave()
    {
        GameData.m_LocalData.m_Data.m_Controller = m_ControllerIndex;
        GameData.m_LocalData.m_Data.m_SoundSetting = m_SoundSetting;
        GameData.m_LocalData.Save();
        Hide();
    }

}
