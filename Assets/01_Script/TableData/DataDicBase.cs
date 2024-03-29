using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataDicBase<U, T> : DataBase where T : class
{
    protected Dictionary<U, T> m_Dic;
    protected List<int> m_ListIdx;
    
    override public void Init(bool isLocalLoad)
    {
        base.Init(isLocalLoad);
        m_Dic = new Dictionary<U, T>();
        m_ListIdx = new List<int>();
    }
    public Dictionary<U, T> Get() { return m_Dic; }
    public T Get(U id)
    {
        if (m_Dic != null && m_Dic.ContainsKey(id)) return m_Dic[id];
        return null;
    }

    public int GetCount()
    {
        if (m_Dic != null) return m_Dic.Count;

        return 0;
    }
}
