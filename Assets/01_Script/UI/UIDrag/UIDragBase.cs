using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragBase : UIBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DragCell[] m_Cells;
    public UIObject m_Content;
    public bool m_IsVertical;
    public float m_DeltaValue;
    public float m_MoveValue;
    public float m_MoveTime;
    List<IDragCellData> m_ListData;

    Vector2 m_Vec2;
    bool m_IsMove;
    float m_BeginValue;
    float m_Pos;
    protected int m_Index;

    public override void Init()
    {
        m_Index = 0;
        m_ListData = new List<IDragCellData>();

        for(int i = 0; i  < m_Cells.Length;i++)
        {
            m_Cells[i].m_FncClickItem = OnClickItem;
        }
    }

    public virtual void Add(IDragCellData data)
    {
        if (m_ListData == null) m_ListData = new List<IDragCellData>();
        m_ListData.Add(data);
    }

    public void InitData()
    {
        for(int i = 0; i < m_ListData.Count;i++)
        {
            if (m_Cells.Length <= i) break;
            m_Cells[i].Init(m_ListData[i]);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (m_IsMove) return;

        if (m_IsVertical)
        {
            m_BeginValue = eventData.position.y;
            m_Pos = m_Content.m_Rect.localPosition.y;
        }
        else
        {
            m_BeginValue = eventData.position.x;
            m_Pos = m_Content.m_Rect.localPosition.x;
        }



    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (m_IsVertical)
        {
            m_Vec2.x = 0;
            m_Vec2.y = eventData.position.y - m_BeginValue;
        }
        else
        {
            m_Vec2.x = eventData.position.x - m_BeginValue;
            m_Vec2.y = 0;
        }

        OnDragEnd(m_Vec2);
    }

    protected virtual void OnDragEnd(Vector2 delta)
    {
        if (m_IsMove) return;

        if (delta.x > m_DeltaValue)
        {
            RightMove();
        }
        else if (delta.x < m_DeltaValue)
        {
            LeftMove();
        }
    }

    protected virtual void RightMove()
    {
        if (m_Index <= 0) return;
        m_IsMove = true;

        m_Pos += m_MoveValue;

        m_Index--;

        Debug.Log("Left : " + m_Index);

        if (m_IsVertical) LeanTween.moveLocalY(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
        else LeanTween.moveLocalX(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
    }

    protected virtual void LeftMove()
    {
        if (m_Index >= m_Cells.Length - 1) return;
        m_IsMove = true;

        m_Pos -= m_MoveValue;

        m_Index++;

        Debug.Log("Right : " + m_Index);

        if (m_IsVertical) LeanTween.moveLocalY(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
        else LeanTween.moveLocalX(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
    }

    protected virtual void EndMove()
    {
        m_IsMove = false;
        
        for(int i = 0;i < m_Cells.Length;i++)
        {
            if (m_ListData.Count <= i) break;
            if (i == m_Index) m_Cells[i].SetCenterData(m_ListData[i]);
            else m_Cells[i].SetMoveData(m_Index, m_ListData[i]);
        }
    }


    //override에서 if(m_Index == data.Index) return; 필수
    protected virtual void OnClickItem(IDragCellData data)
    {
        m_Pos = m_Content.m_Rect.localPosition.x;

        m_IsMove = true;

        if (m_Index < data.Index)
        {
            m_Pos -= m_MoveValue * (data.Index - m_Index);
        }
        else if (m_Index > data.Index)
        {
            m_Pos += m_MoveValue * (m_Index - data.Index);
        }

        m_Index = data.Index;

        if (m_IsVertical) LeanTween.moveLocalY(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
        else LeanTween.moveLocalX(m_Content.m_Obj, m_Pos, m_MoveTime).setOnComplete(EndMove);
    }

    protected void SetSelectPos(IDragCellData data)
    {
        m_Pos = m_Content.m_Rect.localPosition.x;

        m_IsMove = true;

        if (m_Index < data.Index)
        {
            m_Pos -= m_MoveValue * (data.Index - m_Index);
        }
        else if (m_Index > data.Index)
        {
            m_Pos += m_MoveValue * (m_Index - data.Index);
        }

        m_Index = data.Index;

        Vector3 pos = m_Content.transform.localPosition;
        pos.x = m_Pos;
        m_Content.transform.localPosition = pos;
    }
}
