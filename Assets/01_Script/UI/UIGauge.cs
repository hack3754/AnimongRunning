using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGauge : UIObject
{
    public RectTransform m_RectGauge;
    public bool m_IsHorizon;
    float m_Max;

    public void Init()
    {
        if (m_IsHorizon) m_Max = m_RectGauge.sizeDelta.x;
        else m_Max = m_RectGauge.sizeDelta.y;
    }

    public void SetValue(float value)
    {
        Vector2 vec2 = m_RectGauge.sizeDelta;
        if (m_IsHorizon)
        {
            vec2.x = m_Max * value;
        }
        else
        {
            vec2.y = m_Max * value;
        }
        m_RectGauge.sizeDelta = vec2;
    }
}
