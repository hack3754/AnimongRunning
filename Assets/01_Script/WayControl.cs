using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayControl : MonoBehaviour
{
    public Transform m_Trans;

    MapObject m_Map;
    MapDataItem m_tData;

    Vector3 m_PosOri;

    public void Init()
    {
        m_PosOri = m_Trans.localPosition;
    }

    public void NextMapLoad()
    {
        if (m_tData == null) return;

        m_Map.TrapsRelase();

        Destroy(m_Map.gameObject);

        int nextId = 0;

        nextId = m_tData.nextMaps[Random.Range(0, m_tData.nextMaps.Count)];
        MapLoad(nextId);
    }

    public void SetMapData(MapDataItem tData)
    {
        m_tData = tData;
    }

    public void MapLoad(MapObject map)
    {
        m_Map = map;
    }

    public void InitMap()
    {
        m_Map.Init();
    }

    public void GameReset()
    {
        m_Trans.localPosition = m_PosOri;
        m_Map.TrapsRelase();
        Destroy(m_Map.gameObject);
        m_tData = null;
    }

    public void MapLoad(int id)
    {
        m_tData = DataManager.Instance.m_MapData.Get(id);
        if (m_tData != null)
        {
            //ResourceManager.Instance.Instantiate(m_tData.name, m_Trans, OnEndLoad);
            StartCoroutine(ResourceManager.Instance.CoInstantiate(m_tData.res, m_Trans, OnEndLoad));
        }
    }

    void OnEndLoad(GameObject obj)
    {
        m_Map = obj.GetComponent<MapObject>();
        m_Map.Init();
    }
}
