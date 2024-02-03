using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

[System.Serializable]
public class LocalData
{
	List<int> m_OpenChar;
	public int SelectCharId { get; set; }
    public int Gold { get; set; }
	public float Score { get; set; }

    public System.DateTime m_SelectTime;
    public LocalData()
    {

    }

    public void Init()
    {
		if(m_OpenChar == null) m_OpenChar = new List<int>();
        SelectCharId = 1;
    }

	public void FirstInit()
	{
        m_OpenChar.Add(1);
    }

	public bool IsLock(int id)
	{
		if (m_OpenChar == null)
		{
			Init();
			FirstInit();
		}
		return m_OpenChar.Contains(id) ==false;
	}

	public void AddOpenChar(int id)
	{
		if (m_OpenChar.Contains(id) == false) m_OpenChar.Add(id);
	}
	public void SetScore()
	{
		if(GameData.m_Score > Score)
		{
			Score = GameData.m_Score;
        }
	}
}

public class LocalDataSave
{
	Dictionary<string, LocalData> m_LocalData;
	public LocalData m_Data;
	public bool m_IsInit;

	readonly string LOCAL_DATA = "LOCAL_DATA";

    public void Init()
	{
		if (m_IsInit) return;

		InitData("PlayerID");

		m_IsInit = true;
	}

	public void Save()
	{
		SaveData();
	}

	public void ResetData(string userName)
	{
		if (m_LocalData != null) m_LocalData.Clear();
		else m_LocalData = new Dictionary<string, LocalData>();
		m_LocalData.Add(userName, m_Data);
	}

	void InitData(string userName)
	{
		Dictionary<string, LocalData> obj = null;
		if (PlayerPrefs.HasKey(LOCAL_DATA))
		{
			byte[] bytes = System.Convert.FromBase64String(PlayerPrefs.GetString(LOCAL_DATA));
			try
			{
				using (var ms = new MemoryStream(bytes))
				{
					object objData = new BinaryFormatter().Deserialize(ms);
					if (objData != null)
					{
						obj = (Dictionary<string, LocalData>)objData;
					}
				}
			}
			catch
			{
				PlayerPrefs.DeleteKey(LOCAL_DATA);
				obj = null;
			}
		}

		bool IsNew = true;



		if (obj != null)
		{
			m_LocalData = obj;
			if (m_LocalData.ContainsKey(userName))
			{
				IsNew = false;
				m_Data = m_LocalData[userName];                
                m_Data.Init();
			}
		}
		else
		{
			m_LocalData = new Dictionary<string, LocalData>();
		}

		if (IsNew)
		{
			m_Data = new LocalData();
			m_Data.Init();
            m_Data.FirstInit();

			m_LocalData.Add(userName, m_Data);
		}
	}

	public void SelectCharID()
	{
		if (DataManager.Instance.m_CharData == null) return;
		if(m_Data.m_SelectTime.Year < System.DateTime.Now.Year || m_Data.m_SelectTime.Month < System.DateTime.Now.Month || m_Data.m_SelectTime.Day < System.DateTime.Now.Day)
		{
			m_Data.SelectCharId = UnityEngine.Random.Range(0, DataManager.Instance.m_CharData.Get().Count) + 1;
            m_Data.m_SelectTime = System.DateTime.Now;
			SaveData();
        }
	}

	public void SaveData()
	{
		if (m_LocalData != null)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					new BinaryFormatter().Serialize(ms, m_LocalData);
					PlayerPrefs.SetString(LOCAL_DATA, System.Convert.ToBase64String(ms.ToArray()));
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				Debug.Log("LocalData Non Save : " + e.ToString());
			}
		}
	}
}
