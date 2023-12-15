using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : UIObject
{
    public bool m_IsMain;
    protected bool m_IsShow = false;
    protected UIBase m_NextUI;
    public virtual void Init()
    {
        SetActive(false);
        m_IsShow = false;
    }

    public virtual void Show()
    {
        GameManager.Instance.m_OutGameUI.SetPrevUI(this);

    }

    public virtual void Hide()
    {
        SetActive(false);
    }

    public virtual void Refresh()
    {

    }
}
