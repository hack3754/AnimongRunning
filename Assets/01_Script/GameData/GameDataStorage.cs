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

    public void Init()
    {
        m_MaxHP = DataManager.Instance.m_BGData.hp_max;
        m_HP = DataManager.Instance.m_BGData.hp_max;
        m_SprintTime = 0;
        m_IsSprint = false;
    }
}
