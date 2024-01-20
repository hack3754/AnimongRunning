using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrapCollider : ObjectEntries
{
    public SpriteRenderer[] m_Sprites;
    public int m_Id;
    int m_LineIndex;
    public bool IsEnable { get; set; }

    public TrapDataItem m_tData;

    public void Init()
    {
        m_tData = DataManager.Instance.m_TrapData.GetData(m_Id);
        if(m_tData != null && m_tData.IsUnder)
        {
            for (int i = 0; i < m_Sprites.Length; i++)
            {
                m_Sprites[i].sortingOrder = 0;
            }
        }
    }

    public void InitTrap()
    {
        IsEnable = false;
        m_LineIndex = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_tData == null || m_tData.IsUnder == true) return;

        if(m_LineIndex == 0 && collision.tag == TagName.Lane.ToString())
        {
            LaneObject lane = collision.gameObject.GetComponent<LaneObject>();
            
            if (lane != null)
            {
                m_LineIndex = lane.m_Index;
                SetSort(lane.m_Index);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    public void Set(bool isSet)
    {
        SetActive(isSet);
        IsEnable = isSet;

        if (IsEnable == false)
        {
            m_LineIndex = 0;
        }
    }

    void SetSort(int lane)
    {
        for(int i = 0;i < m_Sprites.Length;i++)
        {
            m_Sprites[i].sortingOrder = lane * 3;
        }
    }

    public void SetDisable()
    {
        m_Obj.SetActive(false);
        m_LineIndex = 0;
        //disable Ani
    }
}
