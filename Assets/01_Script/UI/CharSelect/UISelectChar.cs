using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectChar : UIDragBase
{
    public ButtonObject m_BtnSelect;
    public UIObject m_ObjBtnPurchase;
    public ButtonObject m_BtnPurchase;
    public ButtonObject m_BtnAds;
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

        Show();
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

    void SetData()
    {
        for(int i = 0;i < m_Datas.Count;i++)
        {
            m_Datas[i].m_IsLock = GameData.m_LocalData.m_Data.IsLock(m_Datas[i].m_tData.id);
            m_Cells[i].Refresh();
        }

        EndMove();
    }

    void SetCenter(IDragCellData data)
    {
        CharCardDragData itemData = (CharCardDragData)data;
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

    }
}
