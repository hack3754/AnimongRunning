using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragCellData
{
    public int Index
    {
        get;
        set;
    }
}

public class DragCell : UIObject
{
    protected System.Action<IDragCellData> m_FncCenter;
    protected System.Action<IDragCellData> m_FncMove;
    public System.Action<IDragCellData> m_FncClickItem;

    public virtual void Init(IDragCellData data)
    {

    }
    public virtual void SetFunction(System.Action<IDragCellData> fncCenter, System.Action<IDragCellData> fncMove)
    {
        m_FncCenter = fncCenter;
        m_FncMove = fncMove;
    }
    public virtual void SetData(IDragCellData data)
    {
        
    }

    public virtual void Refresh()
    {

    }

    public virtual void SetMoveData(int centerIndex, IDragCellData data)
    {
        m_FncMove.Invoke(data);
    }
    public virtual void SetCenterData(IDragCellData data)
    {
        m_FncCenter?.Invoke(data);
    }
}
