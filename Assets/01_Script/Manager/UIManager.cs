using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public InGameUIMain m_InGame;
    public OutGameUIMain m_OutGame;

    public void Init()
    {
        m_InGame.Init();
        m_OutGame.Init();
    }
}
