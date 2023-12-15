using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : UIBase
{
    public ButtonObject m_BtnStart;
    public ButtonObject m_BtnShop;
    public ButtonObject m_BtnRanking;
    public ButtonObject m_BtnCharSelect;
    //public ButtonObject m_BtnCharSetting;
    //public ButtonObject m_BtnAD;
    System.Action m_FncStart;
    public void Init(System.Action fncStart)
    {
        base.Init();
        m_FncStart = fncStart;
        m_BtnStart.m_FncOnClick = OnClickStart;
        m_BtnShop.m_FncOnClick = OnClickShop;
        m_BtnRanking.m_FncOnClick = OnClickRanking;
        m_BtnCharSelect.m_FncOnClick = OnClickCharSelect;

    }
    public override void Show()
    {
        base.Show();
    }

    void OnClickStart()
    {
        m_FncStart?.Invoke();
    }

    void OnClickShop()
    {
        GameManager.Instance.m_OutGameUI.m_UIShop.Show();
    }

    void OnClickRanking()
    {

    }
    void OnClickCharSelect()
    {
        GameManager.Instance.m_OutGameUI.m_UISelectChar.Show();
    }

}
