using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadData : MSingleton<ResourceLoadData>
{
    //Resource List
    Dictionary<string, GameObject> m_LoadPrefabs;

    public void AddPrefab(string key, GameObject prefab)
    {
        if (m_LoadPrefabs == null) m_LoadPrefabs = new Dictionary<string, GameObject>();

        if (m_LoadPrefabs.ContainsKey(key) == false) m_LoadPrefabs.Add(key, prefab);
    }

    public GameObject GetPrefab(string key)
    {
        if (m_LoadPrefabs == null) m_LoadPrefabs = new Dictionary<string, GameObject>();

        if (m_LoadPrefabs.ContainsKey(key)) return m_LoadPrefabs[key];

        return null;
    }
}
