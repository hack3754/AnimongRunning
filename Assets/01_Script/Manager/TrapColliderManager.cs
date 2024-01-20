using MRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ObstacleType
{
    Trap,
    Item,
    Score,
    Gold,
    Max,
}

public class TrapColliderManager : MonoBehaviour
{
    public Transform m_Parent;
    public TrapCollider[] m_PrefabsTrap;
    public TrapCollider[] m_PrefabsItem;
    public TrapCollider[] m_PrefabsScore;
    public TrapCollider[] m_PrefabsGold;
    public TrapCollider[] m_PrefabsBlock;

    List<string> m_TrapNames;
    List<string> m_ItemNames;
    List<string> m_ScoreNames;
    List<string> m_GoldNames;
    List<string> m_BlockNames;
    Dictionary<string, List<TrapCollider>> m_Traps;

    Vector3 m_Scale;

    public void Init()
    {
        m_TrapNames = new List<string>();
        m_Traps = new Dictionary<string, List<TrapCollider>>();
        for (int i = 0; i < m_PrefabsTrap.Length; i++)
        {
            if (m_TrapNames.Contains(m_PrefabsTrap[i].name) == false) m_TrapNames.Add(m_PrefabsTrap[i].name);
            
            m_PrefabsTrap[i].Init();
            if (m_PrefabsTrap[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsTrap[i].m_tData.res) == false)
                {
                    m_Traps.Add(m_PrefabsTrap[i].m_tData.res, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsTrap[i].name].Add(m_PrefabsTrap[i]);
            }
        }

        m_ItemNames = new List<string>();
        for (int i = 0; i < m_PrefabsItem.Length; i++)
        {
            if (m_ItemNames.Contains(m_PrefabsItem[i].name) == false) m_ItemNames.Add(m_PrefabsItem[i].name);

            m_PrefabsItem[i].Init();
            if (m_PrefabsItem[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsItem[i].m_tData.res) == false)
                {
                    m_Traps.Add(m_PrefabsItem[i].m_tData.res, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsItem[i].name].Add(m_PrefabsItem[i]);
            }
        }

        m_ScoreNames = new List<string>();
        for (int i = 0; i < m_PrefabsScore.Length; i++)
        {
            if (m_ScoreNames.Contains(m_PrefabsScore[i].name) == false) m_ScoreNames.Add(m_PrefabsScore[i].name);

            m_PrefabsScore[i].Init();
            if (m_PrefabsScore[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsScore[i].m_tData.res) == false)
                {
                    m_Traps.Add(m_PrefabsScore[i].m_tData.res, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsScore[i].name].Add(m_PrefabsScore[i]);
            }
        }

        m_GoldNames = new List<string>();
        for (int i = 0; i < m_PrefabsGold.Length; i++)
        {
            if (m_GoldNames.Contains(m_PrefabsGold[i].name) == false) m_GoldNames.Add(m_PrefabsGold[i].name);

            m_PrefabsGold[i].Init();
            if (m_PrefabsGold[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsGold[i].m_tData.res) == false)
                {
                    m_Traps.Add(m_PrefabsGold[i].m_tData.res, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsGold[i].name].Add(m_PrefabsGold[i]);
            }
        }

        m_BlockNames = new List<string>();
        for (int i = 0; i < m_PrefabsBlock.Length; i++)
        {
            if (m_BlockNames.Contains(m_PrefabsBlock[i].name) == false) m_BlockNames.Add(m_PrefabsBlock[i].name);

            m_PrefabsBlock[i].Init();
            if (m_PrefabsBlock[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsBlock[i].m_tData.res) == false)
                {
                    m_Traps.Add(m_PrefabsBlock[i].m_tData.res, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsBlock[i].name].Add(m_PrefabsBlock[i]);
            }
        }
    }

    public TrapCollider GetTrapCollider(Transform parent, ObstacleType obstacleType)
    {
        string trap = string.Empty;
        if (obstacleType == ObstacleType.Trap)
        {
            if (m_TrapNames == null || m_TrapNames.Count <= 0) return null;
            trap = m_TrapNames[UnityEngine.Random.Range(0, m_TrapNames.Count)];
        }
        else if (obstacleType == ObstacleType.Item)
        {
            if (m_ItemNames == null || m_ItemNames.Count <= 0) return null;
            trap = m_ItemNames[UnityEngine.Random.Range(0, m_ItemNames.Count)];
        }
        else if (obstacleType == ObstacleType.Score)
        {
            if (m_ScoreNames == null || m_ScoreNames.Count <= 0) return null;
            trap = m_ScoreNames[UnityEngine.Random.Range(0, m_ScoreNames.Count)];
        }
        else if (obstacleType == ObstacleType.Gold)
        {
            if (m_GoldNames == null || m_GoldNames.Count <= 0) return null;
            trap = m_GoldNames[UnityEngine.Random.Range(0, m_GoldNames.Count)];
        }
        else return null;

        if (m_Traps.ContainsKey(trap))
        {
            TrapCollider col = null;

            for (int i = 1; i < m_Traps[trap].Count; i++)
            {
                if (m_Traps[trap][i].IsEnable == false)
                {
                    col = m_Traps[trap][i];
                    break;
                }
            }

            if (col == null)
            {
                col = Instantiate(m_Traps[trap][0]);
                m_Traps[trap].Add(col);
            }

            col.m_Trans.SetParent(parent);
            col.Set(true);
            if (trap.Contains("item_Fruit"))
            {
                Debug.Log(m_Traps[trap][0].m_Trans.localScale);
            }

            col.transform.localScale = m_Traps[trap][0].m_Trans.localScale;

            return col;
        }

        return null;
    }

    public TrapCollider GetRandomTrapCollider(Transform parent, string trapType)
    {
        ObstacleType obstacleType = ObstacleType.Max;
        string trap = string.Empty;

        if (trapType.Equals(AMUtility.BLOCK))
        {
            obstacleType = ObstacleType.Trap;
            if (m_BlockNames == null || m_BlockNames.Count <= 0) return null;
            trap = m_BlockNames[UnityEngine.Random.Range(0, m_BlockNames.Count)];
        }
        else
        {
            if (trapType.Equals(AMUtility.TRAP))
            {
                if (UnityEngine.Random.Range(0, 1000) <= 50) obstacleType = ObstacleType.Item;
                else if (UnityEngine.Random.Range(0, 1000) <= 600) obstacleType = ObstacleType.Gold;
                else if (UnityEngine.Random.Range(0, 1000) <= 100) obstacleType = ObstacleType.Trap;
            }
            else if (trapType.Equals(AMUtility.ITEM))
            {
                if (UnityEngine.Random.Range(0, 10000) <= 1) obstacleType = ObstacleType.Item;
                else if (UnityEngine.Random.Range(0, 1000) <= 600) obstacleType = ObstacleType.Gold;
            }
            else return null;

            if (obstacleType == ObstacleType.Trap)
            {
                if (m_TrapNames == null || m_TrapNames.Count <= 0) return null;
                trap = m_TrapNames[UnityEngine.Random.Range(0, m_TrapNames.Count)];
                //trap = m_TrapNames[5];
            }
            else if (obstacleType == ObstacleType.Item)
            {
                if (m_ItemNames == null || m_ItemNames.Count <= 0) return null;
                trap = m_ItemNames[UnityEngine.Random.Range(0, m_ItemNames.Count)];
            }
            else if (obstacleType == ObstacleType.Score)
            {
                if (m_ScoreNames == null || m_ScoreNames.Count <= 0) return null;
                trap = m_ScoreNames[UnityEngine.Random.Range(0, m_ScoreNames.Count)];
            }
            else if (obstacleType == ObstacleType.Gold)
            {
                if (m_GoldNames == null || m_GoldNames.Count <= 0) return null;
                trap = m_GoldNames[UnityEngine.Random.Range(0, m_GoldNames.Count)];
            }
            else return null;
        }
        

        if (m_Traps.ContainsKey(trap))
        {
            m_Scale = m_Traps[trap][0].m_Trans.localScale;
            TrapCollider col = null;

            for (int i = 1; i < m_Traps[trap].Count; i++)
            {
                if (m_Traps[trap][i].IsEnable == false)
                {
                    col = m_Traps[trap][i];
                    break;
                }
            }

            if (col == null)
            {
                col = Instantiate(m_Traps[trap][0]);
                col.m_Trans.SetParent(parent);
                m_Traps[trap].Add(col);
            }
            else
                col.m_Trans.SetParent(parent);

            col.Set(true);
            col.transform.localScale = m_Scale;
            return col;
        }

        return null;
    }

    public void Free(List<TrapCollider> objs)
    {
        for(int i = 0;i < objs.Count;i++)
        {
            objs[i].Set(false);
            objs[i].m_Trans.SetParent(m_Parent);
            if (m_Traps.ContainsKey(objs[i].name))
            {
                objs[i].transform.localScale = m_Traps[name][0].m_Trans.localScale;
            }
        }
    }
}
