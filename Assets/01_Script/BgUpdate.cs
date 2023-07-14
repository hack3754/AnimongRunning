using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BgUpdate : MonoBehaviour
{
    public Player m_Player;
    public Transform m_TransBg;
    public MapObject[] m_MapObjects;
    public Transform[] m_TransWay;
    public int m_TitleCount;
    int[] m_WayIndex;
    Vector3 m_Vec3;

    private void Awake()
    {
        m_WayIndex = new int[m_TransWay.Length];
        for (int i = 0; i < m_WayIndex.Length; i++)
        {
            m_WayIndex[i] = i;
        }
    }

    public void Init()
    {
        for(int i = 0;i < m_MapObjects.Length;i++)
        {
            m_MapObjects[i].Init();
        }
    }

    public void BgMove(float bgSpeed)
    {
        m_TransBg.Translate(-(Time.deltaTime * bgSpeed), 0, 0);
        UpdateWay(m_TransBg);     
    }

    public void UpdateWay(Transform moveBg)
    {
        if (-m_TransWay[m_WayIndex[1]].localPosition.x >= moveBg.localPosition.x)
        {
            m_Vec3 = m_TransWay[m_WayIndex[0]].localPosition;
            m_Vec3.x = m_TransWay[m_WayIndex[m_WayIndex.Length - 1]].localPosition.x + (m_TitleCount * 0.8f);
            m_TransWay[m_WayIndex[0]].localPosition = m_Vec3;

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
