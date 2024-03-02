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
    public static bool m_IsMove;

    public static int m_RadomSeed;

    public static LocalDataSave m_LocalData;

    public static float m_Score;
    public static int m_Gold;

    public static bool m_IsContinue;

    public static Vector3 m_PosGold;

    public static bool m_IsStop;
    public static bool m_IsSpeedUp;
    public static bool m_IsStun;

    public static List<int> m_PlayCharIDs;

    public static void Init()
    {
        m_Player = new PlayerData();
        m_Player.Init();
        m_BGSpeed = 0;
        m_LocalData = new LocalDataSave();
        m_LocalData.Init();
        m_LocalData.SelectCharID();
        m_LocalData.m_Data.SetScore();
        m_Score = 0;

        m_IsMove = true;
        m_RadomSeed = 1;

        m_IsStop = false;
        m_IsSpeedUp = false;
        m_IsStun = false;

        m_PlayCharIDs = new List<int>();
        m_PlayCharIDs.Add(m_LocalData.m_Data.SelectCharId);

        m_Player.m_PlayCharId = m_LocalData.m_Data.SelectCharId;
        m_Player.SetSelectChar();

        m_IsContinue = false;
    }

    public static void GameReset()
    {
        m_BGSpeed = 0;
        m_RadomSeed = 1;
        m_SpeedSlow = 0;
        m_SpeedUp = 0;
        m_Score = 0;
        m_Gold = 0;
        m_IsStop = false;
        m_IsSpeedUp = false;
        m_IsStun = false;
        m_IsMove = true;
        m_IsContinue = false;
        m_Player.Reset();

        m_Player.m_PlayCharId = m_LocalData.m_Data.SelectCharId;
        m_Player.SetSelectChar();
    }

    public static void GameContinue()
    {
        m_BGSpeed = 0;
        m_RadomSeed = 1;
        m_SpeedSlow = 0;
        m_SpeedUp = 0;
        m_IsStop = false;
        m_IsSpeedUp = false;
        m_IsStun = false;
        m_IsMove = true;
        m_IsContinue = false;

        m_Player.SetSelectChar();
    }

    public static bool IsCharLock(int id)
    {
        return m_PlayCharIDs.Contains(id) == false;
    }
}
