using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInGame : UIObject
{
    public Camera m_Cam;
    public Text m_TxtTime;
    public Text m_TxtScore;
    public UIGauge m_UIHP;
    public EventTrigger m_MoveTigger;
    public ButtonObject m_BtnJump;
    Vector3 m_BeginPos;
    Vector3 m_DragPos;
    Vector3 m_PreDragPos;

    public void Init()
    {
        m_TxtScore.text = "0";
        m_UIHP.Init();
        m_UIHP.SetValue(1);
        m_BtnJump.m_FncOnClick = OnClickJump;
        /*
        EventTrigger.Entry entry_BeginDrag = new EventTrigger.Entry();
        entry_BeginDrag.eventID = EventTriggerType.BeginDrag;
        entry_BeginDrag.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        m_MoveTigger.triggers.Add(entry_BeginDrag);

        EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        m_MoveTigger.triggers.Add(entry_Drag);

        EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
        entry_EndDrag.eventID = EventTriggerType.EndDrag;
        entry_EndDrag.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        m_MoveTigger.triggers.Add(entry_EndDrag);
        */

    }

    public void Reset()
    {
        m_TxtTime.text = "00:00.00";
        m_TxtScore.text = "0";
        m_UIHP.SetValue(1);
    }

    public void SetTime()
    {
        m_TxtTime.text = string.Format("{0}:{1}.{2}", GameTimeSystem.GetTime().Minutes.ToString("00"), GameTimeSystem.GetTime().Seconds.ToString("00"), GameTimeSystem.GetTime().Milliseconds.ToString("00"));
    }

    public void SetScore(int score)
    {
        m_TxtScore.text = score.ToString();
    }

    public void SetHP(float hpValue)
    {
        m_UIHP.SetValue(hpValue / DataManager.Instance.m_BGData.hp_max);
    }

    void OnClickJump()
    {
        GameManager.Instance.m_Running.Jump("Gomong_Jump_Run");
    }

    public void OnBeginDrag(PointerEventData data)
    {
        m_BeginPos = m_Cam.ScreenToWorldPoint(data.position);
        //GameManager.Instance.m_Running.m_IsDrag = true;
    }

    public void OnDrag(PointerEventData data)
    {
        m_DragPos = m_Cam.ScreenToWorldPoint(data.position);

        if (data.delta.y > 0)
        {
            if (GameManager.Instance.m_Running.m_Dir == -1)
            {
                //GameManager.Instance.m_Running.m_Vertical = 0;
            }

            GameManager.Instance.m_Running.m_Dir = 1;
        }
        else
        {
            if (GameManager.Instance.m_Running.m_Dir == 1)
            {
                //GameManager.Instance.m_Running.m_Vertical = 0;
            }

            GameManager.Instance.m_Running.m_Dir = -1;
        }

        GameManager.Instance.m_Running.m_Vertical += (m_DragPos - m_BeginPos).y;

        m_BeginPos = m_DragPos;
    }
    public void OnEndDrag(PointerEventData data)
    {
        GameManager.Instance.m_Running.m_Vertical = 0;
        GameManager.Instance.m_Running.m_Dir = 0;
    }
}
