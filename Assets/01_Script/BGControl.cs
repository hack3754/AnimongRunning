using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
    public BgUpdate[] m_BgUpdate;
    public float[] m_BgSpeed;
    float m_Time;
    float[] m_BgSpeedValue;
    private void Awake()
    {
        m_BgSpeedValue = new float[m_BgSpeed.Length];
        for (int i = 0; i < m_BgSpeed.Length; i++)
        {
            m_BgSpeedValue[i] = m_BgSpeed[i];
        }
    }

    public void BgMove()
    {
        for (int i = 0; i < m_BgUpdate.Length; i++)
        {
            if (m_BgSpeed.Length > i)
            {
                if (GameData.m_SpeedSlow > 0)
                {
                    if (GameData.m_BGSpeed > DataManager.Instance.m_BGData.min_speed)
                    {
                        m_BgSpeedValue[i] -= GameData.m_SpeedSlow * Time.deltaTime;
                    }
                }
                else
                {
                    if (m_BgSpeedValue[i] <= m_BgSpeed[i])
                        m_BgSpeedValue[i] += GameData.m_SpeedSlow * Time.deltaTime;
                    else
                        m_BgSpeedValue[i] = m_BgSpeed[i];
                }
                
                m_BgUpdate[i].BgMove(m_BgSpeed[i]);
            }
            else
            {
                m_BgUpdate[i].BgMove(GameData.m_BGSpeed);
            }
        }
    }
}
