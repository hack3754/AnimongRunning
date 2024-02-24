using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataBase : MonoBehaviour
{
    //데이터 키 값이 좌로 설정 되어 있는지
    protected bool is_key_right = false;

    private const char CHAR_ENTER = '\n';
    private const char CHAR_COMMA = ',';

    protected int idx_key;
    protected int idx_value;
	protected int idx_desc;
	protected int idx_max;
    virtual public void Init(bool isLocalLoad)
    {
      
    }

    virtual public void Load(string _path, bool isLocalLoad)
    {
        if (isLocalLoad)
        {
            ResourceManager.Instance.LoadAsync<TextAsset>(_path, (assets, isSucess) =>
            {
                if (isSucess)
                {
                    DataLoad(assets.text);
                }
            });
        }
        else
            StartCoroutine(DoLoad(_path));

    }

    private IEnumerator DoLoad(string _path)
	{		
		UnityWebRequest www;
		www = UnityWebRequest.Get(_path);
		yield return www.SendWebRequest();

		if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log("시트 불러오는 과정에서 에러");
			yield break;
		}
#if UNITY_EDITOR
		Debug.Log(www.downloadHandler.text);
#endif
        DataLoad(www.downloadHandler.text);

        www.Dispose();
		yield break;
	}

	public void DataLoad(string txt)
	{
        string[] cell = txt.Split(CHAR_ENTER);
        string[] row;
        int i, j;

        idx_key = 0;
        idx_value = 0;

        for (i = 0; i < cell.Length; i++)
        {
            cell[i] = cell[i].Trim();
            row = cell[i].Split(CHAR_COMMA);

            if (i == 0)
            {
                for (j = 0; j < row.Length; j++)
                {
                    if (row[j].Contains("#")) idx_desc = j;
                    if (row[j].Contains("key")) idx_key = j;
                    if (row[j].Contains("value")) idx_value = j;
                    if (string.IsNullOrEmpty(row[j])) break;
                }
                idx_max = j;
                ParseDataFirst(row);
                continue;
            }
            if (row.Length <= idx_key) continue;
            if (row[idx_key] == "desc") continue;

            ParseData(row);
        }

        DataManager.Instance.CheckLoad();
    }

	protected virtual void ParseDataFirst(string[] _row)
	{

	}
    protected virtual void ParseData(string[] _row)
    {

    }
}
