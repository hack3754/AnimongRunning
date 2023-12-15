using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum TrapType
{
    None,
    Slow,
    Score,
    Gold,
    HpRecovery,
    SpdUp,
    Death,
    Stop,
    Blow,
    DotDmg,
    DmgStop,
    Dmg,
    Rainboots,
    Helmet,
}
public class TrapDataItem
{
    public string res;
    public TrapType type;
    public TrapType type2;
    public string prefab;
    public bool IsUnder;
    public bool IsTrap;
    public int time;
    public int time2;
    public int value;
    public int value2;
}
public class TrapData : DataDicBase<int, TrapDataItem>
{
    public override void Init()
    {
        base.Init();
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
            if (string.IsNullOrEmpty(_row[i])) continue;
            idx = m_ListIdx[i];
            switch (_row[idx_key])
            {
                
                case "res": m_Dic[idx].res = _row[i]; break;
                case "type": m_Dic[idx].type = (TrapType)Enum.Parse(typeof(TrapType), _row[i]); break;
                case "type2": m_Dic[idx].type2 = (TrapType)Enum.Parse(typeof(TrapType), _row[i]); break;
                case "IsUnder": m_Dic[idx].IsUnder = int.Parse(_row[i]) == 1; break;
                case "time": m_Dic[idx].time = int.Parse(_row[i]); break;
                case "time2": m_Dic[idx].time2 = int.Parse(_row[i]); break;
                case "value": m_Dic[idx].value = int.Parse(_row[i]); break;
                case "value2": m_Dic[idx].value2 = int.Parse(_row[i]); break;
                case "IsTrap": m_Dic[idx].IsTrap = int.Parse(_row[i]) == 1; break;
                    
            }
        }
    }

    public TrapDataItem GetData(int id)
    {
        if(m_Dic.ContainsKey(id)) return m_Dic[id];

        return null;
    }
}
