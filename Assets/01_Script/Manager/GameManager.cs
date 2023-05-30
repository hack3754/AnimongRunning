using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MSingleton<GameManager>
{
    public RunningManager m_Running;
    public TrapColliderManager TrapColliderManager;
    public UIManager m_UIManager;

    private void Awake()
    {
        
    }
}
