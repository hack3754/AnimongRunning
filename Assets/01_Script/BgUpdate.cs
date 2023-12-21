using MRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BgUpdate : MonoBehaviour
{
    public Player m_Player;
    public Transform m_TransBg;
    public WayControl[] m_Ways;
    public int m_TitleCount;

    int[] m_WayIndex;
    Vector3 m_Vec3;
    float m_BgValue;

    int m_WayCount = 0;
    private void Awake()
    {
        m_WayIndex = new int[m_Ways.Length];

        for (int i = 0; i < m_Ways.Length; i++)
        {
            m_WayIndex[i] = i;
            m_Ways[i].Init();
        }
    }

    public IEnumerator FirstMapLoad()
    {
        MapDataItem tData = null;
        int id = 0;
        int firstId = 2;
        m_WayCount = 0;
        for (int i = 0; i < m_Ways.Length; i++)
        {
            if (i == 0)
            {
                id = 1;
                tData = DataManager.Instance.m_MapData.Get(1);
            }
            else if(i == m_Ways.Length - 1 )
            {
                //nextId = 0;
                //nextId = DataManager.Instance.m_MapData.GetRandom();
                //tData = DataManager.Instance.m_MapData.Get(nextId);
                DataManager.Instance.m_MapData.TryGetRandom(firstId, out id, out tData);
                if (tData == null) tData = DataManager.Instance.m_MapData.Get(2);
            }
            else
            {
                id = firstId;
                tData = DataManager.Instance.m_MapData.Get(firstId);
            }

            if (tData != null)
            {
                m_Ways[m_WayCount].SetMapData(tData, id);
                yield return ResourceManager.Instance.StartCoroutine(ResourceManager.Instance.CoInstantiate(tData.res, m_Ways[i].m_Trans, OnEndLoad));
                m_WayCount++;
            }
        }
    }

    void OnEndLoad(GameObject obj)
    {
        MapObject map = obj.GetComponent<MapObject>();
        m_Ways[m_WayCount].MapLoad(map);
    }

    public void GameReset()
    {
        for(int i = 0;i < m_Ways.Length;i++)
        {
            m_Ways[i].GameReset();
            m_WayIndex[i] = i;
        }

        m_BgValue = 0;
    }

    public void InitMap()
    {
        for(int i = 0;i < m_Ways.Length;i++)
        {
            m_Ways[i].InitMap();
        }
    }

    public void BgMove(float bgSpeed)
    {
        m_BgValue = Time.deltaTime * bgSpeed;
        //m_TransBg.Translate(-(m_BgValue), 0, 0);

        for(int i = 0;i < m_Ways.Length;i++)
        {
            m_Ways[i].m_Trans.Translate(-(m_BgValue), 0, 0);
        }
        UpdateWay();
    }
    public void BgMoveScore(float bgSpeed)
    {
        m_BgValue = Time.deltaTime * bgSpeed;
        //m_TransBg.Translate(-(m_BgValue), 0, 0);

        for (int i = 0; i < m_Ways.Length; i++)
        {
            m_Ways[i].m_Trans.Translate(-(m_BgValue), 0, 0);
        }
        UpdateWay();

        //UI Score
        SetScore(m_BgValue);
    }


    public void BgStop()
    {
        for (int i = 0; i < m_Ways.Length; i++)
        {
            //m_Ways[i].m_Trans.Translate(0, 0, 0);
        }
    }

    public void UpdateWay()
    {
        if (m_Ways[m_WayIndex[0]].m_Trans.localPosition.x <= -(m_TitleCount * 0.8f))
        {
            m_Vec3 = m_Ways[m_WayIndex[0]].m_Trans.localPosition;
            m_Vec3.x = m_Ways[m_WayIndex[m_WayIndex.Length - 1]].m_Trans.localPosition.x + (m_TitleCount * 0.8f);
            m_Ways[m_WayIndex[0]].m_Trans.localPosition = m_Vec3;

            //Debug.Log(m_WayIndex[0]);
            m_Ways[m_WayIndex[0]].NextMapLoad(m_Ways[m_WayIndex[m_WayIndex.Length - 1]].ID);
            Sort();
        }
    }

    void Sort()
    {
        for (int i = 0; i < m_WayIndex.Length - 1; i++)
        {
            int temp = m_WayIndex[i + 1];
            m_WayIndex[i + 1] = m_WayIndex[i];
            m_WayIndex[i] = temp;
        }
    }

    public void SetScore(float score)
    {
        GameData.m_Score += score;
        GameManager.Instance.m_InGameUI.SetScore((int)GameData.m_Score);
    }
}
