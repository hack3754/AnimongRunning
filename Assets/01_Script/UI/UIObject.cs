using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    public GameObject m_Obj;
    public RectTransform m_Rect;
    
    public void SetActive(bool isActive)
    {
        m_Obj.SetActive(isActive);
    }
}
