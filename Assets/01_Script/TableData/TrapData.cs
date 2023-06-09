using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum TrapType
{
    Noraml
}
public class TrapDataItem
{
    public string name;
    public TrapType type; 
}
public class TrapData : DataBase
{
    public Dictionary<int, TrapDataItem> m_Dic;
    public List<int> m_ListIdx = new List<int>();
    public override void Init()
    {
        base.Init();
        m_Dic = new Dictionary<int, TrapDataItem>();


        Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=932822041");
    }

    protected override void ParseDataFirst(string[] _row)
    {
        base.ParseDataFirst(_row);

        for (int i = 0; i < _row.Length; i++)
        {
            if (i == idx_max) break;
            if (i == idx_key || i == idx_desc)
            {
                m_ListIdx.Add(-1);
                continue;
            }
            m_ListIdx.Add(int.Parse(_row[i]));
            if (m_Dic.ContainsKey(m_ListIdx[i])) continue;
            m_Dic.Add(m_ListIdx[i], new TrapDataItem());
        }
    }
    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
        int idx;
        for (int i = 0; i < _row.Length; i++)
        {
            if (i == idx_max) break;
            if (i == idx_key || i == idx_desc) continue;
            idx = m_ListIdx[i];
            switch (_row[idx_key])
            {
                case "name": m_Dic[idx].name = _row[i]; break;
                case "type": m_Dic[idx].type = (TrapType)int.Parse(_row[i]); break;
            }
        }
    }
}
