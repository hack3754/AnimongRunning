using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataItem
{
    public int id;
    public string res;
    public float volume;
}

public class SoundData : DataDicBase<int, SoundDataItem>
{
    private List<int> listIdx;

    public override void Init(bool isLocalLoad)
    {
        base.Init(isLocalLoad);
        listIdx = new List<int>();

        string path = string.Empty;

        if (isLocalLoad) path = ResourceManager.Instance.GetKey(ResourceManager.PathType.DATA, "Sound");
        else
            path = "https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=651490087";

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
                listIdx.Add(-1);
                continue;
            }
            listIdx.Add(int.Parse(_row[i]));
            if (m_Dic.ContainsKey(listIdx[i])) continue;
            m_Dic.Add(listIdx[i], new SoundDataItem());
            m_Dic[listIdx[i]].id = listIdx[i];
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
            idx = listIdx[i];
            switch (_row[idx_key])
            {
                case "res": m_Dic[idx].res = _row[i]; break;
                case "volume": m_Dic[idx].volume = int.Parse(_row[i]) / 100f; break;
            }
        }
    }
}
