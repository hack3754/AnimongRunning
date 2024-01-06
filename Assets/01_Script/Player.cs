using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class StateInfo
{
    public float m_Time;
    public TrapType m_Type;
    public float m_Value;
    public float m_StateTime;
}

public class Player : MonoBehaviour
{
    public GameObject m_Obj;
    public Transform m_Trans;
    public Transform m_TransBody;
    public CharObject m_Char;
    public Collider2D m_Col;
    public Rigidbody2D m_RigidBody;
    public MapObject m_Map;

    //Effect
    public GameObject[] m_FxAni;

    int m_LineIndex;
    TrapCollider m_Trap;
    List<TrapCollider> m_DmgStops = new List<TrapCollider>();
    bool m_IsRainboots;
    public void Init()
    {
        DIsableEffect();
    }

    private void UpdateState()
    {
        
    }

    public void GameReset()
    {
        m_IsRainboots = false;
        m_DmgStops.Clear();
        m_Trap = null;
        m_Map = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagName.Lane.ToString())
        {
            LaneObject lane = collision.gameObject.GetComponent<LaneObject>();
            if (lane != null)
            {
                m_LineIndex = lane.m_Index;
                SetSort(lane.m_Index);
            }
        }

        if (collision.tag == TagName.Obstacle.ToString())
        {
            TrapCollider trap = collision.gameObject.GetComponent<TrapCollider>();
            if (trap != null)
            {
                if (m_IsRainboots && (trap.m_tData.res.Equals("Poop_001") || trap.m_tData.res.Equals("Mud_001"))) return;
                m_Trap = trap;
                GameManager.Instance.m_Running.SetTrap(m_Trap);
            }
        }

        if (collision.tag == TagName.Block.ToString())
        {
            BlockObject block = collision.gameObject.GetComponent<BlockObject>();
            if (block != null)
            {
                //게임 오브에 대한 코드
                //GameManager.Instance.m_Running.GameOverBlock();
                //멈춤에 대한 코드
                GameManager.Instance.m_IsStop = true;
                GameData.m_BGSpeed = DataManager.Instance.m_BGData.min_speed;
            }
        }

        if (collision.tag == TagName.Map.ToString())
        {
            MapCollider mapCol = collision.gameObject.GetComponent<MapCollider>();
            if (mapCol != null)
            {
                m_Map = mapCol.m_MapObj;
                m_DmgStops.Clear();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagName.Obstacle.ToString())
        {
            TrapCollider trap = collision.gameObject.GetComponent<TrapCollider>();
            if (m_Trap != null && m_Trap.Equals(trap))
            {
                m_Trap = null;
                GameManager.Instance.m_Running.ResetRunning();
            }
        }

        if (collision.tag == TagName.Block.ToString())
        {
            BlockObject block = collision.gameObject.GetComponent<BlockObject>();
            if (block != null)
            {
                GameManager.Instance.m_IsStop = false;
            }
        }
    }

    public void SetPlayer()
    {
        if(m_Char != null)
        {
            Destroy(m_Char.gameObject);
            m_Char = null;
        }
        var table = DataManager.Instance.m_CharData.Get(GameData.m_LocalData.m_Data.SelectCharId);
        if(table == null)
        {
            table = DataManager.Instance.m_CharData.Get(1);
        }

        if (table != null)
        {
            string key = ResourceKey.GetKey(ResourceKey.m_KeyCharPrefab, table.res);
            GameObject obj = ResourceLoadData.Instance.GetPrefab(key);
            if (obj != null)
            {
                GameObject charObj = Instantiate<GameObject>(obj, m_Trans);
                m_Char = charObj.GetComponent<CharObject>();
                m_Char.m_Animator.speed = 0.5f;
            }

            obj = null;
        }
    }

    void OnEndLoad(Animator ani, bool isSuccess)
    {
        if (isSuccess == false || m_Char == null) return;
        m_Char.m_Animator = ani;
        Idle();
    }

    void SetSort(int lane)
    {
        if (m_Char == null) return;

        for (int i = 0; i < m_Char.m_Sprites.Length; i++)
        {
            m_Char.m_Sprites[i].sortingOrder = (lane * 3) - (i + 1);
        }
    }

    void DIsableEffect()
    {
        for (int i = 0; i < m_FxAni.Length; i++)
        {
            m_FxAni[i].SetActive(false);
        }
    }

    public void Idle()
    {
        if (m_Char == null) return;
        //m_Animator.Play("Idle", -1, 0);
        DIsableEffect();
        m_Char.m_Animator.Play("Idle");
        //m_Animator.speed = 0.2f;
    }

    public void Run()
    {
        if (m_Char == null) return;
        DIsableEffect();
        m_Char.m_Animator.Play("Run");
        m_FxAni[0].SetActive(true);
        //m_Animator.speed = 0.5f;
    }

    public void TimeOut()
    {
        if (m_Char == null) return;
        DIsableEffect();
        m_Char.m_Animator.Play("Groggy_03");
        m_FxAni[3].SetActive(true);
    }

    public void TrapDeath()
    {
        if (m_Char == null) return;
        DIsableEffect();
        m_Char.m_Animator.Play("Groggy_02");
        m_FxAni[1].SetActive(true);
    }

    public void Slip()
    {
        if (m_Char == null) return;
        DIsableEffect();
        m_Char.m_Animator.Play("Slip_01");
        m_FxAni[2].SetActive(true);
    }


    public void Jump(string aniName)
    {
        if (m_Char == null) return;
        m_Char.m_Animator.Play(aniName, -1, 0);
        //m_Animator.speed = 0.5f;
    }

    public void EndJump()
    {
        GameManager.Instance.m_Running.EndJump();
        m_Col.enabled = true;
    }

    public void SetDmgStop(TrapCollider trap)
    {
        if (m_DmgStops.Contains(trap) == false)
        {
            m_DmgStops.Add(trap);
        }
    }

    public void SetRainboots(bool isEnable)
    {
        m_IsRainboots = isEnable;
    }

    #region hitTest
    /*
    RaycastHit2D[] m_Hits = new RaycastHit2D[5];
    public bool Hit()
    {
        ContactFilter2D filter2d = new ContactFilter2D();
 
        filter2d.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("BGCollider")));
        int count = m_Col.Cast(Vector2.up, filter2d, m_Hits, 0.5f);

        if (count > 0)
        {
            m_Vec3 = m_Trans.localPosition;
            m_Vec3.y = m_Hits[0].transform.localPosition.y + m_Hits[0].collider.bounds.extents.y;
            m_Trans.localPosition = m_Vec3;
        }

        return false;
    }

    public void Move(Vector2 positionChange)
    {
        //Vector2 positionChange = new Vector2(m_Trans.localPosition.x, m_Trans.localPosition.y) * Time.deltaTime;
        ContactFilter2D filter2d = new ContactFilter2D();
        filter2d.SetLayerMask(LayerMask.NameToLayer("BGColider"));
        m_Col.Cast(Vector2.up, filter2d, m_Hits, positionChange.magnitude);

        if(m_Hits.Length > 0)
        {
            Debug.Log(m_Hits[0].transform.name);
            float colliderDistance = m_Hits[0].distance;
            Vector2 closesColliderPoint = m_Col.ClosestPoint(m_Hits[0].point);

            float distanceBetween = Vector2.Distance(closesColliderPoint, m_Hits[0].point);

            if(positionChange.normalized != Vector2.zero && !Mathf.Approximately(distanceBetween, 0))
            {
                positionChange = positionChange.normalized * distanceBetween;
            }
            else
            {
                positionChange = CornerPush(closesColliderPoint);
            }
        }

        m_RigidBody.MovePosition(m_RigidBody.position + (positionChange));
    }

    Vector2 CornerPush(Vector2 closesColliderPoint)
    {
        return Vector2.zero;
    }
    */
    #endregion
}

