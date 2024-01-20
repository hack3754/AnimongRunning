using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OutGameUIMain : UIObject
{
    public UISelectChar m_UISelectChar;
    public UIShop m_UIShop;
    public UIMain m_UIMain;
    public TMP_Text m_TxtCoin;

    public GameObject m_ObjTop;
    public GameObject m_ObjBg;
    public ButtonObject m_BtnBack;

    List<UIBase> m_ListPrevUI;

    bool m_IsMain;
    public void Init()
    {
        m_UIMain.Init(GameStart);
        m_UISelectChar.Init();
        m_UIShop.Init();
        m_BtnBack.m_FncOnClick = OnClickBack;

        m_ListPrevUI = new List<UIBase>();

        m_IsMain = true;

        SetCoin();
    }

    public void Show()
    {
        SetActive(true);
        ShowMain();
    }

    public void Hide()
    {
        SetActive(false);
    }


    public void ShowMain()
    {
        for(int i = 0; i < m_ListPrevUI.Count;i++)
        {
            m_ListPrevUI[i].Hide();
        }

        m_ListPrevUI.Clear();

        SetOutGameUI(true);

        m_UIMain.Show();
    }

    public void SetOutGameUI(bool isMain)
    {
        m_ObjBg.SetActive(!isMain);
        m_ObjTop.SetActive(!isMain);
    }

    /// <summary>
    /// UI Back;
    /// </summary>
    public void ShowPrevUI()
    {
        if (m_ListPrevUI.Count > 0)
        {
            if (m_ListPrevUI[m_ListPrevUI.Count - 1] != null) m_ListPrevUI[m_ListPrevUI.Count - 1].Hide();
            m_ListPrevUI.RemoveAt(m_ListPrevUI.Count - 1);
        }

        if (m_ListPrevUI.Count > 0)
        {
            if (m_ListPrevUI[m_ListPrevUI.Count - 1] != null) m_ListPrevUI[m_ListPrevUI.Count - 1].Show();
        }
        else
        {
            ShowMain();
        }
    }

    public void SetPrevUI(UIBase ui)
    {
        if (ui.m_IsMain == false)
        {
            SetOutGameUI(false);
            if (m_ListPrevUI.Count > 0)
            {
                if (m_ListPrevUI[m_ListPrevUI.Count - 1] != null) m_ListPrevUI[m_ListPrevUI.Count - 1].Hide();
            }
        }
        else
        {
            SetOutGameUI(true);
        }

        if (m_ListPrevUI.Contains(ui) == false) m_ListPrevUI.Add(ui);

        ui.SetActive(true);

    }

    void GameStart()
    {
        Hide();
        GameManager.Instance.GameReadyStart();
    }

    void OnClickBack()
    {
        ShowPrevUI();
    }

    public void SetCoin()
    {
        m_TxtCoin.text = GameData.m_LocalData.m_Data.Gold.ToString();
    }
}
