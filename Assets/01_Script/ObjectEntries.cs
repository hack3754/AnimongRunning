using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEntries : MonoBehaviour
{
    public GameObject m_Obj;
    public Transform m_Trans;

    public void SetActive(bool isActive)
    {
        m_Obj.SetActive(isActive);
    }
}
