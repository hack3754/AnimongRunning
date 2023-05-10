using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WayReuse : MonoBehaviour
{
    public Transform[] m_TransWay;
    public int m_TitleCount;
    int[] m_WayIndex;
    Vector3 m_Vec3;
    private void Awake()
    {
        m_WayIndex = new int[m_TransWay.Length];
        for(int i = 0; i < m_WayIndex.Length;i++)
        {
            m_WayIndex[i] = i;
        }
    }
    public void UpdateWay(Transform moveBg)
    {
        if (-1 * m_TransWay[m_WayIndex[1]].localPosition.x >= moveBg.localPosition.x)
        {
            m_Vec3 = m_TransWay[m_WayIndex[0]].position;
            m_Vec3.x = m_TransWay[m_WayIndex[m_WayIndex.Length - 1]].position.x + m_TitleCount;
            m_TransWay[m_WayIndex[0]].position = m_Vec3;


            Sort();
        }
    }

    void Sort()
    {
        for (int i = 0; i < m_WayIndex.Length - 1;i++)
        {
            int temp = m_WayIndex[i + 1];
            m_WayIndex[i + 1] = m_WayIndex[i];
            m_WayIndex[i] = temp;
        }
    }
}
