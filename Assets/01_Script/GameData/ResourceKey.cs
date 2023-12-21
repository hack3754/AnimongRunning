using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceKey
{
    public readonly static string m_KeyCharPrefab = "Assets/04_Prefabs/UI/Character/{0}.prefab";
    public readonly static string m_KeyCharAni = "Assets/02_Designs/03_Characters/Ca_02_Tod/Ani/{0}.controller";
    
    public static string GetKey(string key, string name)
    {
        if (string.IsNullOrEmpty(name)) return string.Empty;
        return string.Format(key, name);
    }
}
