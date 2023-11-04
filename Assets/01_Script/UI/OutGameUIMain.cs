using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameUIMain : UIObject
{
    public UISelectChar m_UISelectChar;
    public UIMain m_UIMain;
    List<UIBase> m_ListPrevUI;

    bool m_IsMain;
    public void Init()
    {
        m_UISelectChar.Init();
        m_UIMain.Init(GameStart);

        m_ListPrevUI = new List<UIBase>();

        m_IsMain = true;
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


    public void HidePrevUI(UIBase ui)
    {
        if (m_ListPrevUI.Count > 0)
        {
            if (m_ListPrevUI[m_ListPrevUI.Count - 1].Equals(ui) == false)
                m_ListPrevUI[m_ListPrevUI.Count - 1].Hide();
        }

        if (m_ListPrevUI.Count <= 0) ShowMain();

    }
    public void ShowMain()
    {
        for(int i = 0; i < m_ListPrevUI.Count;i++)
        {
            m_ListPrevUI[i].Hide();
        }

        m_ListPrevUI.Clear();

        m_UIMain.Show();
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

    public void HidePrevUI()
    {
        if (m_ListPrevUI.Count > 0)
        {
            if (m_ListPrevUI[m_ListPrevUI.Count - 1] != null) m_ListPrevUI[m_ListPrevUI.Count - 1].Hide();
            m_ListPrevUI.RemoveAt(m_ListPrevUI.Count - 1);
        }
    }


    public void SetPrevUI(UIBase ui)
    {
        if (m_ListPrevUI.Contains(ui) == false) m_ListPrevUI.Add(ui);
    }

    void GameStart()
    {
        Hide();
        GameManager.Instance.GameReadyStart();
    }
}
