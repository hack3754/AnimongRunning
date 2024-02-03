using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public float m_HP;
    public float m_MaxHP;
    public bool m_IsJump;
    public bool m_IsMove;
    public bool m_IsJumpBlock;

    public float m_SprintTime;
    public bool m_IsSprint;

    CharDataItem m_SelectChar;
    public float m_MaxSpeed;
    public int m_PlayCharId;

    public void Init()
    {
        m_MaxHP = DataManager.Instance.m_BGData.hp_max;
        m_HP = DataManager.Instance.m_BGData.hp_max;
        m_MaxSpeed = DataManager.Instance.m_BGData.max_speed;
        m_SprintTime = 0;
        m_IsSprint = false;
    }

    public void Reset()
    {
        m_HP = DataManager.Instance.m_BGData.hp_max;
        m_IsJumpBlock = false;
    }

    public void SetSelectChar()
    {
        m_SelectChar = DataManager.Instance.m_CharData.Get(m_PlayCharId);
        if (m_SelectChar == null)
        {
            m_MaxHP = DataManager.Instance.m_BGData.hp_max;
            m_HP = DataManager.Instance.m_BGData.hp_max;
            m_MaxSpeed = DataManager.Instance.m_BGData.max_speed;
        }
        else
        {
            m_MaxHP = DataManager.Instance.m_BGData.hp_value * m_SelectChar.health;
            m_HP = m_MaxHP;
            m_MaxSpeed = DataManager.Instance.m_BGData.speed_value * m_SelectChar.speed;
        }
    }
}
