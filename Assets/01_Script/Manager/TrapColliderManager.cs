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
    Dictionary<string, List<TrapCollider>> m_Traps;

    public void Init()
    {
        m_Traps = new Dictionary<string, List<TrapCollider>>();
        for (int i = 0; i < m_PrefabsTrap.Length; i++)
        {
            m_PrefabsTrap[i].Init();
            if (m_PrefabsTrap[i].m_tData != null)
            {
                if (m_Traps.ContainsKey(m_PrefabsTrap[i].m_tData.name) == false)
                {
                    m_Traps.Add(m_PrefabsTrap[i].m_tData.name, new List<TrapCollider>());
                }

                m_Traps[m_PrefabsTrap[i].name].Add(m_PrefabsTrap[i]);
            }
        }
    }

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
