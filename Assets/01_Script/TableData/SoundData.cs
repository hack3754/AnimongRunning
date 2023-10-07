using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataItem
{
    public int id;
    public string res;
}

public class SoundData : DataDicBase<int, SoundDataItem>
{
    public Dictionary<int, SoundDataItem> dic;
    private List<int> listIdx;

    public override void Init()
    {
        base.Init();
        dic = new Dictionary<int, SoundDataItem>();
        listIdx = new List<int>();

        Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=651490087");
        /*
        #if UNITY_EDITOR
                Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
        #else
                Load("https://docs.google.com/spreadsheets/d/1j1df9NRMQL8ZuErrvuAiKUqkC_7uAAw8VR_JJhIBffM/export?format=csv&gid=0");
        #endif
        */
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
            if (dic.ContainsKey(listIdx[i])) continue;
            dic.Add(listIdx[i], new SoundDataItem());
            dic[listIdx[i]].id = listIdx[i];
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
                case "res": dic[idx].res = _row[i]; break;
            }
        }
    }
}
