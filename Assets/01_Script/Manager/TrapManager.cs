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

        if (GameData.m_RadomSeed > 9999) GameData.m_RadomSeed = 1;
        float temp = Time.time * GameData.m_RadomSeed;
        UnityEngine.Random.InitState((int)temp);

        Tilemap tileMap = m_Tilemaps[UnityEngine.Random.Range(0, m_Tilemaps.Length)];

        if (tileMap == null) return;

        ObstacleType obstacleType = ObstacleType.Max;

        foreach (Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            if (!tileMap.HasTile(pos)) continue;

            TileBase tileBase = tileMap.GetTile(pos);
            Vector3 position = tileMap.GetCellCenterWorld(pos);
            TrapCollider col = null;

            Enum.TryParse<ObstacleType>(tileBase.name, out obstacleType);
            /*
            if (tileBase.name.Equals(AMUtility.TRAP))
                col = GameManager.Instance.m_TrapColliderMgr.GetTrapCollider(m_Parent, obstacleType);
            else if (tileBase.name.Equals(AMUtility.ITEM))
                col = GameManager.Instance.m_TrapColliderMgr.GetTrapCollider(m_Parent, ObstacleType.Item);
            */

            //col = GameManager.Instance.m_TrapColliderMgr.GetTrapCollider(m_Parent, obstacleType);

            if (tileBase.name.Equals(AMUtility.TRAP))
                col = GameManager.Instance.m_TrapColliderMgr.GetRandomTrapCollider(m_Parent);

            if (col != null)
            {
                col.m_Trans.position = position;
                col.Init();
            }
        }
    }
}
