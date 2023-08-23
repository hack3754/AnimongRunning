using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static PlayerData m_Player;
    public static float m_BGSpeed;
    public static float m_CAMSpeed;

    public static float m_SlowSpeed;
    public static void Init()
    {
        m_Player = new PlayerData();
        m_Player.Init();
        m_BGSpeed = 0;
    }
}
