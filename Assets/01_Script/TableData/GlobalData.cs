using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : DataBase
{
	public string ad_id;
	public int ad_cnt;
	public float ad_wait_time;
	public float bg_speed;
	public float jump_max;

    public override void Init()
    {
        base.Init();
		
		ad_id = "";
		ad_cnt = 1;
		ad_wait_time = 1f;

        Load("https://docs.google.com/spreadsheets/d/1s7xA3eH8Gc6dV8gOXOzWg0CjKBeGb5vcdw5UHm155xI/export?format=csv&gid=0");
    }

    protected override void ParseData(string[] _row)
    {
        base.ParseData(_row);
		
		switch (_row[idx_key])
		{
			case "ad_id": ad_id = _row[idx_value]; break;
			case "ad_cnt": ad_cnt = int.Parse(_row[idx_value]); break;
			case "ad_wait_time": ad_wait_time = float.Parse(_row[idx_value]); break;
            case "bg_speed": bg_speed = float.Parse(_row[idx_value]); break;
        }
    }

}
