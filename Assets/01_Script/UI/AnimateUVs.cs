using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.U2D;

public class AnimateUVs : MonoBehaviour
{
    public SpriteRenderer m_Renderer;
    public float m_ScrollSpeed;
    float m_OffsetX;
    Vector2 m_OffsetVec = Vector2.zero;

    private void Update()
    {
        m_OffsetX = Mathf.Repeat(m_ScrollSpeed * Time.time, 1);
        m_OffsetVec.x = m_OffsetX;
        //m_Renderer.material.SetTextureOffset("_MainTex", m_OffsetVec);
        m_Renderer.material.mainTextureOffset = m_OffsetVec;
    }
}
