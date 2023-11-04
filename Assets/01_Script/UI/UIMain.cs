using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : UIBase
{
    public ButtonObject m_BtnStart;
    System.Action m_FncStart;
    public void Init(System.Action fncStart)
    {
        base.Init();
        m_FncStart = fncStart;
        m_BtnStart.m_FncOnClick = OnClickStart;

    }
    public override void Show()
    {
        base.Show();
    }

    public void OnClickStart()
    {
        m_FncStart?.Invoke();
    }
}
