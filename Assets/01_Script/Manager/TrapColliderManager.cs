using MRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapColliderManager : MonoBehaviour
{
    public GameObject m_PrefabsTrap;
    MPool<TrapCollider> m_Traps;

    private void Awake()
    {
        m_Traps = new MPool<TrapCollider>(m_PrefabsTrap, transform);
    }
}
