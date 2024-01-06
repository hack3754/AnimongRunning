using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static PlayerData m_Player;
    public static float m_BGSpeed;
    public static float m_CAMSpeed;

    public static float m_SpeedSlow;
    public static float m_SpeedUp;

    public static int m_RadomSeed;

    public static LocalDataSave m_LocalData;

    public static float m_Score;
    public static int m_Gold;
    public static void Init()
    {
        m_Player = new PlayerData();
        m_Player.Init();
        m_BGSpeed = 0;
        m_LocalData = new LocalDataSave();
        m_LocalData.Init();

        m_RadomSeed = 1;
    }

    public static void GameReset()
    {
        m_BGSpeed = 0;
        m_RadomSeed = 1;
        m_SpeedSlow = 0;
        m_SpeedUp = 0;
        m_Score = 0;
        m_Gold = 0;
        m_Player.Reset();
    }
}
