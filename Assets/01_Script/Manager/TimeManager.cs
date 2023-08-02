using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime
{
    public float m_Scale = 1;
    public float m_SetScale = 1;
    public float m_StartScaleTime = 0;
    public float m_ScaleTimeGap = 0;
    public bool m_TimeScale = false;
    public bool m_Active = false;

    public CustomTime m_CustomTime;

    public void init()
    {
        m_Scale = 1;
        m_SetScale = 1;
        m_StartScaleTime = 0;
        m_ScaleTimeGap = 0;
        m_TimeScale = false;
        m_Active = false;
        m_CustomTime = null;
    }

	public void StopTimeScale(float scale)
	{
		m_TimeScale = true;
		m_SetScale = scale;
		m_Scale = scale;
		if (m_CustomTime == null)
			m_StartScaleTime = Time.time;
		else
			m_StartScaleTime = m_CustomTime.GetTime();
	}

	public void EndTimeScale()
	{
		m_TimeScale = false;
		if (m_CustomTime == null)
			m_ScaleTimeGap += ((Time.time - m_StartScaleTime) - ((Time.time - m_StartScaleTime) * m_Scale));
		else
			m_ScaleTimeGap += ((m_CustomTime.GetTime() - m_StartScaleTime) - ((m_CustomTime.GetTime() - m_StartScaleTime) * m_Scale));
		m_Scale = 1;
		m_SetScale = 1;
	}

	public void StartSkillTimeScale(float scale)
    {
        m_TimeScale = true;
        m_SetScale = scale;
        m_Scale = 1;
        if (m_CustomTime == null)
            m_StartScaleTime = Time.time;
        else
            m_StartScaleTime = m_CustomTime.GetTime();
    }
    public void EndSkillTimeScale()
    {
		if (!m_TimeScale) return;
        m_TimeScale = false;
        if(m_CustomTime == null)
            m_ScaleTimeGap += ((Time.time - m_StartScaleTime) - ((Time.time - m_StartScaleTime) * m_Scale));
        else
            m_ScaleTimeGap += ((m_CustomTime.GetTime() - m_StartScaleTime) - ((m_CustomTime.GetTime() - m_StartScaleTime) * m_Scale));
        //m_Scale = 1;
        m_SetScale = 1;
    }

    public void UpdateRealTimeSclae(float scale)
    {
        m_TimeScale = true;

        if (m_CustomTime == null)
            m_ScaleTimeGap += ((Time.time - m_StartScaleTime) - ((Time.time - m_StartScaleTime) * m_Scale));
        else
            m_ScaleTimeGap += ((m_CustomTime.GetTime() - m_StartScaleTime) - ((m_CustomTime.GetTime() - m_StartScaleTime) * m_Scale));

        m_SetScale = m_Scale = scale;
        if (m_CustomTime == null)
            m_StartScaleTime = Time.time;
        else
            m_StartScaleTime = m_CustomTime.GetTime();
    }

    public float GetTime()
    {
        if (m_TimeScale)
        {
            if (m_CustomTime == null)
                return (m_StartScaleTime + ((Time.time - m_StartScaleTime) * m_Scale)) - m_ScaleTimeGap;
            else
                return (m_StartScaleTime + ((m_CustomTime.GetTime() - m_StartScaleTime) * m_Scale)) - m_ScaleTimeGap;
        }
        else
        {
            if (m_CustomTime == null)
                return Time.time - m_ScaleTimeGap;
            else
                return m_CustomTime.GetTime() - m_ScaleTimeGap;
        }
    }

    public float GetDeltaTime()
    {
        if (m_CustomTime == null)
        {
            if (m_TimeScale)
                return Time.deltaTime * m_Scale;
            else
                return Time.deltaTime;
        }
        else
        {
            if (m_TimeScale)
                return m_CustomTime.GetDeltaTime() * m_Scale;
            else
                return m_CustomTime.GetDeltaTime();
        }
    }

    public float GetSmoothDeltaTime()
    {
        if (m_CustomTime == null)
        {
            if (m_TimeScale)
                return Time.smoothDeltaTime * m_Scale;
            else
                return Time.smoothDeltaTime;
        }
        else
        {
            if (m_TimeScale)
                return m_CustomTime.GetSmoothDeltaTime() * m_Scale;
            else
                return m_CustomTime.GetSmoothDeltaTime();
        }

    }

    public bool Update()
    {
        if (m_Scale != m_SetScale)
        {
            if (m_CustomTime == null)
            {
                m_Scale = Mathf.Lerp(m_Scale, m_SetScale, Time.deltaTime * 4);
                m_ScaleTimeGap += ((Time.time - m_StartScaleTime) - ((Time.time - m_StartScaleTime) * m_Scale));
                m_StartScaleTime = Time.time;
            }
            else
            {
                m_Scale = Mathf.Lerp(m_Scale, m_SetScale, m_CustomTime.GetDeltaTime() * 4);
                m_ScaleTimeGap += ((m_CustomTime.GetTime() - m_StartScaleTime) - ((m_CustomTime.GetTime() - m_StartScaleTime) * m_Scale));
                m_StartScaleTime = m_CustomTime.GetTime();
            }
            return true;
        }
        return false;
    }
}

public class TimeManager : MonoBehaviour {
    List<CustomTime> m_List = new List<CustomTime>();
    CustomTime m_MainCustomTime;
    public CustomTime m_CustomDest;
	public CustomTime m_BaseTime;
	int num = 0;
    public void Init()
    {
		m_BaseTime = new CustomTime();
		m_MainCustomTime = new CustomTime();
        m_MainCustomTime.init();
		m_MainCustomTime.m_CustomTime = m_BaseTime;
		m_CustomDest = new CustomTime();
        m_CustomDest.init();
        m_CustomDest.m_CustomTime = m_MainCustomTime;
        StartCoroutine(TimeUpdate());
    }

	public void StopTimeScale()
	{
		m_MainCustomTime.StopTimeScale(0);
	}

	public void EndTimeScale()
	{
		m_MainCustomTime.EndTimeScale();
	}

	public void StartSkillTimeScale(float scale)
    {
        m_MainCustomTime.StartSkillTimeScale(scale);
    }
    public void EndSkillTimeScale()
    {
        m_MainCustomTime.EndSkillTimeScale();
    }

    public float GetTime()
    {
        return m_MainCustomTime.GetTime();
    }

    public float GetDeltaTime()
    {
        return m_MainCustomTime.GetDeltaTime();
    }

    public float GetSmoothDeltaTime()
    {
        return m_MainCustomTime.GetSmoothDeltaTime();
    }

    public float GetScale()
    {
        return m_MainCustomTime.m_Scale;
    }

    public CustomTime GetCustomTime()
    {
        for (num = 0; num < m_List.Count; num++)
        {
            if (!m_List[num].m_Active)
            {
                m_List[num].init();
                m_List[num].m_Active = true;
                return m_List[num];
            }
        }

        m_List.Add(new CustomTime());
        m_List[m_List.Count - 1].init();
		m_List[m_List.Count - 1].m_CustomTime = m_BaseTime;
		m_List[m_List.Count - 1].m_Active = true;
        return m_List[m_List.Count - 1];
    }

    protected IEnumerator TimeUpdate()
    {
        while (true)
        {
            m_MainCustomTime.Update();
            m_CustomDest.Update();
			m_BaseTime.Update();
			for (num = 0; num < m_List.Count; num++)
            {
                if (m_List[num].m_Active)
                {
                    m_List[num].Update();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
