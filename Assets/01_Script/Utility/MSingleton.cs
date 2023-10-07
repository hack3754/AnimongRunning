using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static T m_Instance;
    private static object m_Lock = new object();
    public static T Instance
    {
        get
        {
            // 게임 종료 시 Object 보다 싱글톤의 OnDestroy 가 먼저 실행 될 수도 있다. 
            // 해당 싱글톤을 gameObject.Ondestory() 에서는 사용하지 않거나 사용한다면 null 체크를 해주자
            if (m_ShuttingDown)
            {
                Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)    //Thread Safe
            {
                if (m_Instance == null)
                {
                    m_Instance = (T)FindObjectOfType(typeof(T));
                    if (m_Instance == null)
                    {
                        GameObject obj = new GameObject();
                        m_Instance = obj.AddComponent<T>();
                        obj.name = typeof(T).ToSafeString();

                        DontDestroyOnLoad(obj);
                    }
                }
            }

            return m_Instance;
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}
