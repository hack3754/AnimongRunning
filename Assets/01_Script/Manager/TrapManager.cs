using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapManager : MonoBehaviour
{
    public Transform m_Parent;
    public Tilemap[] m_Tilemaps;

    List<TrapCollider> m_Traps;
    public void Init()
    {
        if (m_Tilemaps == null || m_Tilemaps.Length <= 0) return;
        m_Traps = new List<TrapCollider>();
        AMUtility.RandomSeed();

        Tilemap tileMap = m_Tilemaps[UnityEngine.Random.Range(0, m_Tilemaps.Length)];

        if (tileMap == null) return;

        foreach (Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            if (!tileMap.HasTile(pos)) continue;

            TileBase tileBase = tileMap.GetTile(pos);
            Vector3 position = tileMap.GetCellCenterWorld(pos);
            TrapCollider col = null;

            /*
            Enum.TryParse<ObstacleType>(tileBase.name, out obstacleType);
            col = GameManager.Instance.m_TrapColliderMgr.GetTrapCollider(m_Parent, obstacleType);
            */

            col = GameManager.Instance.m_TrapColliderMgr.GetRandomTrapCollider(m_Parent, tileBase.name);

            if (col != null)
            {
                col.m_Trans.position = position;
                col.Init();
                m_Traps.Add(col);
            }
        }

        Color tileColor = new Color(1f, 1f, 1f, 0);
        foreach (var tile in m_Tilemaps)
        {
            tile.color = tileColor;
            tile.gameObject.SetActive(false);
        }

    }

    public void TrapsRelase()
    {
        if (m_Traps == null) return;
        for(int i = 0; i < m_Traps.Count;i++)
        {
            m_Traps[i].SetActive(false);
            m_Traps[i].IsEnable = false;
            m_Traps[i].transform.parent = GameManager.Instance.m_TrapColliderMgr.m_Parent;
        }

        m_Traps.Clear();
        m_Traps = null;
    }
}
