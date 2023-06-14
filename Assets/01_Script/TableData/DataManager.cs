﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
	public static DataManager ins;

	//public PopupLoaing loading;
	public GameObject go;

	private bool is_load;
	private int load_cnt;
	private bool is_first_load;
	private bool is_restart;


	public GlobalData m_GlobalData;
	public TrapData m_TrapData;

	private List<DataBase> m_ListData;


    public void Awake()
	{
		if (ins != null && ins != this)
		{
			Destroy(gameObject);
			return;
		}

		ins = this;
		DontDestroyOnLoad(gameObject);

		load_cnt = 0;
		is_load = false;
		is_first_load = false;

        m_GlobalData = go.AddComponent<GlobalData>();
		m_TrapData = go.AddComponent<TrapData>();

		m_ListData = new List<DataBase>() { 
			m_GlobalData, 
			m_TrapData 
		};
        
	}

	public void LoadFirstLoad()
	{
		if (is_first_load == true) return;
		LoadData();
	}

	public void LoadData(bool restart = false)
	{
		if (is_load == true) return;
		
		is_restart = restart;

		//loading.obj.SetActive(true);
		UnityWebRequest.ClearCookieCache();
		is_load = true;
		load_cnt = 0;
		
		for(int i =0; i < m_ListData.Count; i++)
		{
            m_ListData[i].Init();
		}
	}

	public void CheckLoad()
	{
		load_cnt++;

		if (load_cnt < m_ListData.Count) return;

		//loading.obj.SetActive(false);
		is_load = false;
		is_first_load = true;

		//Debug.Log("*** " + user.dic[2].value.ContainsKey(SkillData.move_speed));
		//Debug.Log("*** " + user.dic[2].value[SkillData.move_speed]);
	

		//if (PlayManager.ins != null) PlayManager.ins.Init(false);
	}
}
