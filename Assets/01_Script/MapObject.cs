using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public Transform m_TransWay;
    public TrapManager m_Trap;
    public List<int> m_BlockIndex;
    public void Init()
    {
        m_Trap.Init();
    }

    public void TrapsRelase()
    {
        m_Trap.TrapsRelase();
    }
}
