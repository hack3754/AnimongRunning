using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObject : ObjectEntries
{
    public Transform m_TransFirst;
    public Transform m_TransLast;

    public float m_PosLast;

    float m_PosFirst;
    float m_HpPer;
    
    public void Init()
    {
        m_PosFirst = m_Trans.localPosition.x;
    }

    public void MoveUpdate(float hpValue)
    {
        m_HpPer = hpValue / GameData.m_Player.m_MaxHP;
        if (m_HpPer <= 0.4f)
        {
            m_Obj.SetActive(true);
            m_Trans.position = Vector3.Lerp(m_TransFirst.position, m_TransLast.position, 1 - (m_HpPer / 0.4f));
        }
        else
        {
            m_Obj.SetActive(false);
        }
    }

    public void GameReset()
    {
        m_Obj.SetActive(false);
    }
}
