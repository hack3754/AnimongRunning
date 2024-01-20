using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class UIInGame : UIObject
{
    public Camera m_Cam;
    public Canvas m_Canvas;
    public Text m_TxtTime;
    public TMP_Text m_TxtHp;
    public Text m_TxtScore;
    public TMP_Text m_TxtGold;
    public UIGauge[] m_UIHP;
    public UIGauge m_UIEmergencyHP;
    public EventTrigger m_MoveTigger;
    //public ButtonObject m_BtnJump;

    public ButtonObject m_BtnUp;
    public ButtonObject m_BtnDown;

    public RectTransform m_RectGold;
    public Transform m_TransStateParent;
    public UIStateObject m_UIStateObject;
    public UIStateObject[] m_UIStateObjects;
    //Coin

    List<UIStateObject> m_ListTrapState;

    Vector3 m_BeginPos;
    Vector3 m_DragPos;
    Vector3 m_PreDragPos;
    float m_HpPer;
    int m_HpIndex;
    public void Init()
    {
        m_TxtScore.text = "0";
        m_TxtGold.text = "0";
        for (int i = 0; i < m_UIHP.Length; i++)
        {
            m_UIHP[i].Init();
            m_UIHP[i].SetValue(1);
        }
        m_UIEmergencyHP.Init();
        m_UIEmergencyHP.SetValue(1);
        //m_BtnJump.m_FncOnClick = OnClickJump;
        m_BtnUp.m_FncOnClick = OnClickUp;
        m_BtnDown.m_FncOnClick = OnClickDown;

        m_ListTrapState = new List<UIStateObject>();
        m_ListTrapState.Add(m_UIStateObject);

        SetGoldPos();
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

    public void GameReset()
    {
        m_TxtTime.text = "00:00.00";
        m_TxtScore.text = "0";
        m_TxtGold.text = "0";
        SetHP(DataManager.Instance.m_BGData.hp_max);
    }

    public void SetTime()
    {
        m_TxtTime.text = string.Format("{0}:{1}.{2}", GameTimeSystem.GetTime().Minutes.ToString("00"), GameTimeSystem.GetTime().Seconds.ToString("00"), ((int)(GameTimeSystem.GetTime().Milliseconds * 0.1f)).ToString("00"));
    }

    public void SetScore(int score)
    {
        m_TxtScore.text = score.ToString();
    }

    public void SetGold(int gold)
    {
        m_TxtGold.text = gold.ToString();
    }


    public void SetHP(float hpValue)
    {
        m_TxtHp.text = ((int)hpValue).ToString();
        m_HpPer = hpValue / DataManager.Instance.m_BGData.hp_max;
        m_HpIndex = (int)m_HpPer;
        if (m_HpIndex >= 0)
        {
            if (m_HpPer <= 0.4f)
            {
                for (int i = 0; i < m_UIHP.Length; i++)
                {
                    m_UIHP[i].SetActive(false);
                }

                m_UIEmergencyHP.SetActive(true);
                m_UIEmergencyHP.SetValue(m_HpPer);
            }
            else
            {
                for (int i = 0; i < m_UIHP.Length; i++)
                {
                    if (i <= m_HpIndex)
                    {
                        m_UIHP[i].SetActive(true);                        
                        if (m_HpPer - i < 1)
                        {
                            m_UIHP[i].SetValue(m_HpPer - i);
                        }
                        else
                            m_UIHP[i].SetValue(1);
                    }
                    else m_UIHP[i].SetActive(false);
                }

                m_UIEmergencyHP.SetActive(false);
            }
        }
    }

    void OnClickJump()
    {
        GameManager.Instance.m_Running.Jump("Gomong_Jump_Run");
    }

    void OnClickUp()
    {
        GameManager.Instance.m_Running.MoveUp();
    }

    void OnClickDown()
    {
        GameManager.Instance.m_Running.MoveDown();
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

    public void SetState(StateInfo stateInfo, bool isItem)
    {
        if (isItem)
        {
            for(int i = 0;i < m_UIStateObjects.Length;i++)
            {
                if (m_UIStateObjects[i].isSet == false)
                {
                    m_UIStateObjects[i].SetData(stateInfo, isItem);
                    break;
                }
            }
            return;
        }
        for(int i = 0;i < m_ListTrapState.Count;i++)
        {
            if (m_ListTrapState[i].SetData(stateInfo, isItem)) return;
        }

        UIStateObject obj = Instantiate<UIStateObject>(m_UIStateObject, m_TransStateParent);
        obj.SetData(stateInfo, isItem);
        m_ListTrapState.Add(obj);
    }

    public void AllStopState()
    {
        if (m_ListTrapState == null) return;
        for(int i = 0;i < m_ListTrapState.Count;i++)
        {
            m_ListTrapState[i].StopAllCoroutines();
        }
    }

    public void SetGoldPos()
    {
        Vector3 pos = m_Cam.WorldToScreenPoint(m_RectGold.transform.position);
        pos.z = (m_Canvas.transform.position - m_Cam.transform.position).magnitude;
        GameData.m_PosGold = Camera.main.ScreenToWorldPoint(pos);
    }
}
