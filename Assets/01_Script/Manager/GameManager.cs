using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MSingleton<GameManager>
{
    public RunningManager m_Running;
    public TrapColliderManager m_TrapColliderManager;
    public UIManager m_UIManager;

    bool m_IsGameStart;
    private void Awake()
    {
        
    }

    public void GameStart()
    {
        m_TrapColliderManager.Init();
        m_IsGameStart = true;
        m_Running.m_BgUpdate.Init();
    }

    private void Update()
    {
        if (m_IsGameStart == false) return;
        m_Running.Running();
    }
}
