using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MoveObjManager : MonoBehaviour
{
    public MoveObject m_Obj;
    List<MoveObject> m_Objs;

    public void Init()
    {
        m_Obj.Init();
        m_Objs = new List<MoveObject>();
        m_Objs.Add(m_Obj);
    }

    public void SetMoveObject(Vector3 pos, Vector3 endPos, int index)
    {
        for(int i = 0; i < m_Objs.Count;i++)
        {
            if (m_Objs[i].SetMove(pos, endPos, index)) return;
        }

        MoveObject obj = Instantiate<MoveObject>(m_Obj, transform);
        obj.SetMove(pos, endPos, index);
        obj.Init();
        m_Objs.Add(obj);
    }
}
