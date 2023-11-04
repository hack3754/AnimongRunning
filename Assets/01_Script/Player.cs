using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject m_Obj;
    public Transform m_Trans;
    public Transform m_TransBody;
    public SpriteRenderer[] m_Sprites;
    public Animator m_Animator; //CharacterChange;
    public Collider2D m_Col;
    public Rigidbody2D m_RigidBody;
    
    int m_LineIndex;
    TrapCollider m_Trap;

    public void Init()
    {
        m_Animator.speed = 0.5f;
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

        if (collision.tag == TagName.Trap.ToString())
        {
            TrapCollider trap = collision.gameObject.GetComponent<TrapCollider>();
            if (trap != null)
            {
                m_Trap = trap;
                GameManager.Instance.m_Running.SetTrap(m_Trap);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagName.Trap.ToString())
        {
            TrapCollider trap = collision.gameObject.GetComponent<TrapCollider>();
            if (m_Trap != null && m_Trap.Equals(trap))
            {
                m_Trap = null;
                GameManager.Instance.m_Running.ResetRunning();
            }
        }
    }

    void SetSort(int lane)
    {
        for (int i = 0; i < m_Sprites.Length; i++)
        {
            m_Sprites[i].sortingOrder = (lane * 3) - (i + 1);
        }
    }

    public void Idle()
    {
        m_Animator.Play("Idle", -1, 0);
        //m_Animator.speed = 0.2f;
    }

    public void Run()
    {
        m_Animator.Play("Run", -1, 0);
        //m_Animator.speed = 0.5f;
    }

    public void Jump(string aniName)
    {
        m_Animator.Play(aniName, -1, 0);
        //m_Animator.speed = 0.5f;
    }

    public void EndJump()
    {
        GameManager.Instance.m_Running.EndJump();
        m_Col.enabled = true;
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

