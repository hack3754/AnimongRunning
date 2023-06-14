using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrapCollider : ObjectEntries
{
    public string m_PrefabName;

    public bool IsEnable { get; set; }

    public void Init()
    {
        IsEnable = false;
    }
    
    public void Set(bool isSet)
    {
        SetActive(isSet);
        IsEnable = isSet;
    }
}
