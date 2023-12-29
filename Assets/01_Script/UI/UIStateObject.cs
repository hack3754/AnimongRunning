using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStateObject : UIObject
{
    public Image m_ImgGauge; 
    StateInfo m_StateInfo;
    bool m_IsSet;
    public bool SetData(StateInfo stateInfo, bool isItem)
    {
        if (m_IsSet) return false;
        m_IsSet = true;
        m_Obj.SetActive(isItem);
        m_StateInfo = stateInfo;
        GameManager.Instance.StartCoroutine(SetState());

        return true;
    }

    public IEnumerator SetState()
    {
        float ticktime = 0;

        while(true)
        {
            yield return AMUtility.m_WaitForEndOfFrame;

            if(GameManager.Instance.IsGameStart == false)
            {
                m_StateInfo = null;
                m_Obj.SetActive(false);
                m_IsSet = false;
                yield break;
            }

            m_StateInfo.m_StateTime += Time.deltaTime;

            ticktime += Time.deltaTime;
            if (ticktime >= 0.1f)
            {
                //Debug.Log("Tick");
                GameManager.Instance.m_Running.SetDotTrigger(m_StateInfo);
                ticktime = 0;
            }

            m_ImgGauge.fillAmount = m_StateInfo.m_StateTime / m_StateInfo.m_Time;

            if (m_StateInfo.m_StateTime >= m_StateInfo.m_Time)
            {
                Debug.Log("End");
                GameManager.Instance.m_Running.DeleteDotTrap(m_StateInfo);
                m_StateInfo = null;
                m_Obj.SetActive(false);
                m_IsSet = false;
                yield break;
            }
        }


    }
}
