using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupSelectController : UIObject
{
    public GameObject[] m_ObjEnable;
    public ButtonObject[] m_ObjDisable;
    public ButtonObject m_BtnSelect;

    protected int m_ControllerIndex;
    public virtual void Init()
    {
        if (m_BtnSelect != null) m_BtnSelect.m_FncOnClick = OnChanageController;
        for(int i = 0; i < m_ObjDisable.Length;i++)
        {
            int index = i;
            m_ObjDisable[i].m_FncOnClick = () => OnClickSelectController(index);
        }
    }

    public virtual void Show()
    {
        SetActive(true);

        m_ControllerIndex = GameData.m_LocalData.m_Data.m_Controller;

        for (int i = 0; i < m_ObjEnable.Length; i++)
        {
            m_ObjEnable[i].SetActive(i == GameData.m_LocalData.m_Data.m_Controller);
            m_ObjDisable[i].SetActive(i != GameData.m_LocalData.m_Data.m_Controller);
        }

    }

    public virtual void Hide()
    {
        SetActive(false);
    }

    void OnClickSelectController(int index)
    {
        m_ControllerIndex = index;
        for (int i = 0;i < m_ObjEnable.Length;i++)
        {
            m_ObjEnable[i].SetActive(i == index);
            m_ObjDisable[i].SetActive(i != index);
        }
    }

    void OnChanageController()
    {
        GameData.m_LocalData.m_Data.m_IsSelectController = true;
        GameData.m_LocalData.m_Data.m_Controller = m_ControllerIndex;
        GameData.m_LocalData.Save();
        Hide();
    }
}
