using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : UIObject
{
    protected bool m_IsShow = false;
    protected UIBase m_NextUI;
    public virtual void Init()
    {
        SetActive(false);
        m_IsShow = false;
    }

    public virtual void Show()
    {
        SetActive(true);
    }

    public virtual void Hide()
    {
        SetActive(false);
    }

    public virtual void Refresh()
    {

    }
}
