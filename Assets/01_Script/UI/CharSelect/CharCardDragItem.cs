using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharCardDragData: IDragCellData
{
    public int Index
    {
        get;
        set;
    }

    public CharDataItem m_tData;
    public bool m_IsLock;
}

public class CharCardDragItem : DragCell
{
    CharCardDragData m_Data;
    public Image m_ImgIcon;
    public Image m_ImgChar;
    public GameObject m_ObjLock;
    float m_Rot;

    public override void Init(IDragCellData data)
    {
        base.Init(data);
        m_Data = (CharCardDragData)data;
        m_Rot = m_Data.Index % 2 == 0 ? -15 : 15;
    }

    public override void SetFunction(Action<IDragCellData> fncCenter, Action<IDragCellData> fncMove)
    {
        base.SetFunction(fncCenter, fncMove);
    }

    public override void SetData(IDragCellData data)
    {
        m_Data = (CharCardDragData)data;
    }

    public override void Refresh()
    {
        base.Refresh();
        if (m_Data == null) return;
        m_ObjLock.SetActive(m_Data.m_IsLock);
    }

    public override void SetMoveData(int centerIndex, IDragCellData data)
    {
        base.SetMoveData(centerIndex, m_Data);
        m_Rect.localScale = new Vector3(0.8f, 0.8f, 1);
        m_Rot *= -1;
        m_Rect.rotation = Quaternion.Euler(0, 0, m_Rot);
    }

    public override void SetCenterData(IDragCellData data)
    {
        base.SetCenterData(m_Data);
        m_Rect.localScale = Vector3.one;
        m_Rect.rotation = Quaternion.identity;
    }
}
