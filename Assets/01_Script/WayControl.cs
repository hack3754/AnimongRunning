using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayControl : MonoBehaviour
{
    public Transform m_Trans;

    MapObject m_Map;
    MapDataItem m_tData;
    public int ID { get; set; }

    Vector3 m_PosOri;

    public void Init()
    {
        m_PosOri = m_Trans.localPosition;
    }

    public void NextMapLoad(int nextId)
    {
        if (m_tData == null) return;

        m_Map.TrapsRelase();

        Destroy(m_Map.gameObject);
        MapLoad(nextId);
    }

    public void SetMapData(MapDataItem tData, int id)
    {
        m_tData = tData;
        ID = id;
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

    public void MapLoad(int nextId)
    {
        //m_tData = DataManager.Instance.m_MapData.Get(id);
        int id = 0;
        DataManager.Instance.m_MapData.TryGetRandom(nextId, out id, out m_tData);
        ID = id;

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
