using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapManager : MonoBehaviour
{
    public Transform m_Parent;
    public Tilemap m_Tilemap;
    private void Start()
    {
        foreach (Vector3Int pos in m_Tilemap.cellBounds.allPositionsWithin)
        {
            if (!m_Tilemap.HasTile(pos)) continue;
            TileBase tileBase = m_Tilemap.GetTile(pos);
            Vector3 position = m_Tilemap.GetCellCenterWorld(pos);
            TrapCollider col = GameManager.Instance.m_TrapColliderManager.GetTrapCollider(tileBase.name, m_Parent);
            if(col != null)
            {
                col.m_Trans.position = position;
            }
        }
    }
}
