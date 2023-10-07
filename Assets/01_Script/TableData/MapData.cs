using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapDataItem
{
    public string res;
    public List<int> nextMaps;
}
public class MapData : DataDicBase<int, MapDataItem>
{
    public override void Init()
    {
        base.Init();

        //Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=932822041");
        Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=1808877930");
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
            m_Dic.Add(m_ListIdx[i], new MapDataItem());
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

                case "res": m_Dic[idx].res = string.Format("Assets/04_Prefabs/Map/{0}.prefab", _row[i]); break;
                case "nextMaps":
                    m_Dic[idx].nextMaps = new List<int>();
                    string value = _row[i];
                    string[] maps = _row[i].Split("|");
                    int map = 0;
                    for(int k = 0; k < maps.Length;k++)
                    {
                        if (int.TryParse(maps[k], out map))
                        {
                            if(m_Dic[idx].nextMaps.Contains(map) == false) 
                                m_Dic[idx].nextMaps.Add(map);
                        }
                    }
                    break;
            }
        }
    }
    public string GetRes(int id)
    {
        if(m_Dic.ContainsKey(id))
        {
            return m_Dic[id].res;
        }

        return string.Empty;
    }
}
