using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;

public class CSgtBase
{

}

public class CSingleton<T> : CSgtBase where T : class, new ()
{
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = new T();
            }

            return m_Instance;
        }
    }
}
