using MRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TrapColliderManager : MonoBehaviour
{
    public Transform m_Parent;
    public TrapCollider[] m_PrefabsTrap;
    List<string> m_TrapNames;
    Dictionary<string, List<TrapCollider>> m_Traps;

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
    }
    /*
    public TrapCollider GetTrapCollider(string name, Transform parent)
    {
        if (m_Traps.ContainsKey(name))
        {
            TrapCollider col = null;

            for (int i = 1; i < m_Traps[name].Count; i++)
            {
                if (m_Traps[name][i].IsEnable == false)
                {
                    col = m_Traps[name][i];
                    break;
                }
            }

            if (col == null)
            {
                col = Instantiate(m_Traps[name][0]);
                col.m_Trans.SetParent(parent);
                m_Traps[name].Add(col);
            }
            else
                col.m_Trans.SetParent(parent);

            col.Set(true);
            col.transform.localScale = m_Traps[name][0].m_Trans.localScale;

            return col;
        }

        return null;
    }
    */
    public TrapCollider GetTrapCollider(Transform parent)
    {
        if (m_TrapNames == null || m_TrapNames.Count <= 0) return null;

        string trap = m_TrapNames[UnityEngine.Random.Range(0, m_TrapNames.Count)];

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
                col.m_Trans.SetParent(parent);
                m_Traps[trap].Add(col);
            }
            else
                col.m_Trans.SetParent(parent);

            col.Set(true);
            col.transform.localScale = m_Traps[trap][0].m_Trans.localScale;

            return col;
        }

        return null;
    }

    public void Free(List<TrapCollider> objs)
    {
        Vector3 localScale = Vector3.one;
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
