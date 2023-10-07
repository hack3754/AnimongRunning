using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MSingleton<GameManager>
{
    public SoundManager m_Sound;
    public RunningManager m_Running;
    public TrapColliderManager m_TrapColliderManager;
    public UIManager m_UIManager;

    bool m_IsGameStart;
    private void Awake()
    {
       
    }

    public void Init()
    {
        GameData.Init();
        m_TrapColliderManager.Init();
        m_Running.Init();
        m_UIManager.Init();
    }

    public void GameReady()
    {
        StartCoroutine(m_Running.m_BgUpdate.FirstMapLoad(GameStart));
    }

    void GameStart()
    {
        m_IsGameStart = true;
        m_Running.m_Player.Run();

        GameTimeSystem.SetTime();
    }

    private void Update()
    {
        if (m_IsGameStart == false) return;
        m_Running.Running();
    }
}
