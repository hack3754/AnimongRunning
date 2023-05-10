using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCustomActivity : MonoBehaviour
{
    public Button m_Btn;
    public TMP_Text m_TxtLog;

    public void Awake()
    {
        m_Btn.onClick.AddListener(OnClickBtn);
    }

    public void OnClickBtn()
    {
        var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var jo = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        var jc = new AndroidJavaClass("com.unity.animong.CustomActivity");
        jc.CallStatic("SetActivity", jo);
    }
}
