using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharDataItem
{
    public int id;
    public string res;
    public string name;
    public int health;
    public int power;
    public int speed;
    public int vitality;
    public int luck;
    public int price;
}

public class CharData : DataDicBase<int, CharDataItem>
{
    public override void Init(bool isLocalLoad)
    {
        base.Init(isLocalLoad);

        string path = string.Empty;

        if (isLocalLoad) path = ResourceManager.Instance.GetKey(ResourceManager.PathType.DATA, "Character");
        else
            path = "https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=1784063883";

        Load(path, isLocalLoad);

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
            m_Dic.Add(m_ListIdx[i], new CharDataItem());
            m_Dic[m_ListIdx[i]].id = m_ListIdx[i];
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
                case "res": m_Dic[idx].res = _row[i]; break;
                case "name": m_Dic[idx].name = _row[i]; break;
                case "health": m_Dic[idx].health = int.Parse(_row[i]); break;
                case "power": m_Dic[idx].power = int.Parse(_row[i]); break;
                case "speed": m_Dic[idx].speed = int.Parse(_row[i]); break;
                case "vitality": m_Dic[idx].vitality= int.Parse(_row[i]); break;
                case "luck": m_Dic[idx].luck = int.Parse(_row[i]); break;
                case "price": m_Dic[idx].price = int.Parse(_row[i]); break;
            }
        }
    }
}
