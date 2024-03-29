using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundData : DataBase
{
    public float bg_speed; //���ӵ�
    public float hp_max;
    public float min_speed;
    public float max_speed;
    public float max_speed_up;
    public float sprint_time;
    public float sprint_speed;
    public float cam_max_speed;
    public float cam_speed;

    public float hp_value;
    public float speed_value;

    public override void Init(bool isLocalLoad)
    {
        base.Init(isLocalLoad);
        string path = string.Empty;

        if (isLocalLoad) path = ResourceManager.Instance.GetKey(ResourceManager.PathType.DATA, "Background");
        else
            path = "https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=515660359";
        

        Load(path, isLocalLoad);
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
            case "max_speed_up": max_speed_up = float.Parse(_row[idx_value]); break;
            case "sprint_time": sprint_time = float.Parse(_row[idx_value]); break;
            case "sprint_speed": sprint_speed = float.Parse(_row[idx_value]); break;
            case "cam_max_speed": cam_max_speed = float.Parse(_row[idx_value]); break;
            case "cam_speed": cam_speed = float.Parse(_row[idx_value]); break;
            case "hp_value": hp_value = float.Parse(_row[idx_value]); break;
            case "speed_value": speed_value = float.Parse(_row[idx_value]); break;
        }
    }

}
