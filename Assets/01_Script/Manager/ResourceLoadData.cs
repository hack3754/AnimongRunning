using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceLoadData : MSingleton<ResourceLoadData>
{
    //Atlas
    public SpriteAtlas m_AtlasItem;

    Dictionary<string, Sprite> m_Items;
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

    public Sprite GetItemSprite(string name)
    {
        if (m_Items == null) m_Items = new Dictionary<string, Sprite>();
        Sprite temp = null;
        if (m_Items.ContainsKey(name) == false)
        {
            temp = m_AtlasItem.GetSprite(name);
            if (temp != null) m_Items.Add(name, temp);
        }
        else
        {
            temp = m_Items[name];
        }

        return temp;
    }
}
