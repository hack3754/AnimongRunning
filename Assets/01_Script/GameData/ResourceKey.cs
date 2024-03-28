using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceKey
{
    public readonly static string m_KeyCharUIPrefab = "Assets/04_Prefabs/UI/Character/UI_{0}.prefab";
    public readonly static string m_KeyCharPrefab = "Assets/04_Prefabs/Objects/Characters/{0}.prefab";
    public readonly static string m_KeySound = "Assets/07_Sound/Effect/{0}.wav";
    public readonly static string m_KeySoundmp3 = "Assets/07_Sound/Effect/{0}.mp3";
    public static string GetKey(string key, string name)
    {
        if (string.IsNullOrEmpty(name)) return string.Empty;
        return string.Format(key, name);
    }
}
