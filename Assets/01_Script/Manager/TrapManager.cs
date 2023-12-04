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
    public void Init()
    {
        if (m_Tilemaps == null || m_Tilemaps.Length <= 0) return;

        AMUtility.RandomSeed();

        Tilemap tileMap = m_Tilemaps[UnityEngine.Random.Range(0, m_Tilemaps.Length)];

        if (tileMap == null) return;

        ObstacleType obstacleType = ObstacleType.Max;

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
            }
        }
    }
}
