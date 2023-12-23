using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectChar : UIDragBase
{
    public GameObject[] m_ObjSpeed;
    public GameObject[] m_ObjHealth;
    public GameObject[] m_ObjLuck;
    public ButtonObject m_BtnSelect;
    public UIObject m_ObjBtnPurchase;
    public ButtonObject m_BtnPurchase;
    public ButtonObject m_BtnAds;
    CharCardDragData m_SelectData;
    List<CharCardDragData> m_Datas = new List<CharCardDragData>();
    public override void Init()
    {
        base.Init();
        
        CharCardDragData data;

        Dictionary<int, CharDataItem> dic = DataManager.Instance.m_CharData.Get();

        int index = 0;
        foreach (var dicData in dic)
        {
            if (index >= m_Cells.Length) break;
            data = new CharCardDragData();
            data.Index = index;
            data.m_tData = dicData.Value;
            m_Cells[index].SetFunction(SetCenter, SetMove);
            Add(data);
            m_Datas.Add(data);

            index++;
        }

        InitData();

        if (m_BtnSelect != null) m_BtnSelect.m_FncOnClick = OnClickSelect;
        if (m_BtnPurchase != null) m_BtnPurchase.m_FncOnClick = OnClickPurchase;
    }

    public override void Show()
    {
        base.Show();
        SetData();
    }

    public override void Hide()
    {
        base.Hide();
    }
    protected override void RightMove()
    {
        base.RightMove();
    }
    public override void Refresh()
    {
        base.Refresh();
        SetData();
    }
    protected override void LeftMove()
    {
        base.LeftMove();
    }
    protected override void EndMove()
    {
        base.EndMove();
    }

    protected override void OnClickItem(IDragCellData data)
    {
        if (m_Index == data.Index) return;

        CharCardDragData itemData = (CharCardDragData)data;
        SetCenter(itemData);

        base.OnClickItem(data);

    }

    void SetData()
    {
        for(int i = 0;i < m_Datas.Count;i++)
        {
            m_Datas[i].m_IsLock = false;//GameData.m_LocalData.m_Data.IsLock(m_Datas[i].m_tData.id);
            if(m_Datas[i].m_tData.id == GameData.m_LocalData.m_Data.SelectCharId)
            {
                m_Index = m_Datas[i].Index;
                m_SelectData = m_Datas[i];
            }
            m_Cells[i].Refresh();
        }

        if(m_SelectData != null)
        {
            SetCenter(m_SelectData);
        }

        EndMove();
    }

    void SetCenter(IDragCellData data)
    {
        CharCardDragData itemData = (CharCardDragData)data;

        m_SelectData = itemData;

        m_ObjBtnPurchase.SetActive(itemData.m_IsLock);
        m_BtnSelect.SetActive(!itemData.m_IsLock);

        //Speed
        for(int i = 0;i < itemData.m_tData.speed;i++)
        {
            m_ObjSpeed[i].SetActive(true);
        }

        for (int i = itemData.m_tData.speed; i < m_ObjSpeed.Length; i++)
        {
            m_ObjSpeed[i].SetActive(false);
        }


        //Health
        for (int i = 0; i < itemData.m_tData.health; i++)
        {
            m_ObjHealth[i].SetActive(true);
        }

        for (int i = itemData.m_tData.health; i < m_ObjHealth.Length; i++)
        {
            m_ObjHealth[i].SetActive(false);
        }


        //Luck
        for (int i = 0; i < itemData.m_tData.luck; i++)
        {
            m_ObjLuck[i].SetActive(true);
        }

        for (int i = itemData.m_tData.luck; i < m_ObjLuck.Length; i++)
        {
            m_ObjLuck[i].SetActive(false);
        }

    }

    void SetMove(IDragCellData data)
    {
        CharCardDragData itemData = (CharCardDragData)data;
    }

    void OnClickPurchase()
    {

    }
    void OnClickSelect()
    {
        if (m_SelectData != null)
        {
            GameData.m_LocalData.m_Data.SelectCharId = m_SelectData.m_tData.id;
            GameManager.Instance.m_Running.m_Player.SetPlayer();
            GameManager.Instance.m_OutGameUI.ShowPrevUI();
        }
    }
}
