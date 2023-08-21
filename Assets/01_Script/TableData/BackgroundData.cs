using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundData : DataBase
{
    public float bg_speed; //°¡¼Óµµ
    public float hp_max;
    public float min_speed;
    public float max_speed;
    public float sprint_time;
    public float sprint_speed;
    public float cam_speed;

    public override void Init()
    {
        base.Init();
        Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=515660359");
    }

    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);

        switch (_row[idx_key])
        {
            case "bg_speed": bg_speed = float.Parse(_row[idx_value]); break;
            case "hp_max": hp_max = float.Parse(_row[idx_value]); break;
            case "min_speed": min_speed = float.Parse(_row[idx_value]); break;
            case "max_speed": max_speed = float.Parse(_row[idx_value]); break;
            case "sprint_time": sprint_time = float.Parse(_row[idx_value]); break;
            case "sprint_speed": sprint_speed = float.Parse(_row[idx_value]); break;
            case "cam_speed": sprint_speed = float.Parse(_row[idx_value]); break;
        }
    }

}
