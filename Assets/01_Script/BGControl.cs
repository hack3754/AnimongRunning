using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
    public BgUpdate[] m_BgUpdate;
    public float[] m_BgSpeed;
    float m_Time;
    public void BgMove()
    {
        for (int i = 0; i < m_BgUpdate.Length; i++)
        {
            if (m_BgSpeed.Length > i)
            {
                m_BgUpdate[i].BgMove(m_BgSpeed[i]);
            }
            else
            {
                m_BgUpdate[i].BgMove(GameData.m_BGSpeed);
            }
        }
    }
}
