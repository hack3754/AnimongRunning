using MRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject[] m_Coin;
    public GameObject m_Obj;
    public Transform m_Trans;
    public TweenMove m_Tween;
    public TweenScale m_TweenScale;

    Vector3 m_Scale;
    bool m_IsEnable;
    float m_Time;

    public void Init()
    {
        m_Scale = m_Trans.localScale;
        m_Tween.m_OnCompleat = EndMove;
        m_IsEnable = false;
    }
    public bool SetMove(Vector3 pos, Vector3 endPos, int index)
    {
        if (m_IsEnable) return false; 
        m_IsEnable = true;
        m_Time = 0;
        for (int i = 0;i < m_Coin.Length;i++)
        {
            if (i == index) m_Coin[i].SetActive(true);
            else m_Coin[i].SetActive(false);
        }
        m_Obj.SetActive(true);
        m_Trans.localScale = m_Scale;
        m_Trans.position = pos;
        m_Tween.mStartPos = pos;
        m_Tween.mEndPos = endPos;
        m_Tween.Play();
        m_TweenScale.Play();

        return true;
    }

    void EndMove()
    {
        m_Obj.SetActive(false);
        m_IsEnable = false;
    }
}
