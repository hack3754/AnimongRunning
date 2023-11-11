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
    float m_Score;

    int m_WayCount = 0;
    private void Awake()
    {
        m_WayIndex = new int[m_Ways.Length];
        for (int i = 0; i < m_WayIndex.Length; i++)
        {
            m_WayIndex[i] = i;
        }
    }

    public IEnumerator FirstMapLoad()
    {
        MapDataItem tData = null;
        int nextId = 0;
        m_WayCount = 0;
        for (int i = 0; i < m_Ways.Length; i++)
        {
            if (i == 0)
            {
                tData = DataManager.Instance.m_MapData.Get(i + 1);
            }
            else
            {
                nextId = 0;
                if (tData != null)
                {
                    nextId = tData.nextMaps[UnityEngine.Random.Range(0, tData.nextMaps.Count)];
                    tData = DataManager.Instance.m_MapData.Get(nextId);
                }

                if (tData == null || nextId == 0) tData = DataManager.Instance.m_MapData.Get(i + 1);
            }

            if (tData != null)
            {
                m_Ways[m_WayCount].SetMapData(tData);
                yield return ResourceManager.Instance.StartCoroutine(ResourceManager.Instance.CoInstantiate(tData.res, m_Ways[i].m_Trans, OnEndLoad));
                m_WayCount++;
            }
        }
    }

    void OnEndLoad(GameObject obj)
    {
        MapObject map = obj.GetComponent<MapObject>();
        map.Init();
        m_Ways[m_WayCount].MapLoad(map);
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
        m_Score += m_BgValue;
        GameManager.Instance.m_InGameUI.SetScore((int)m_Score);
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
            m_Ways[m_WayIndex[0]].NextMapLoad();
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
}
