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
    public Animator m_Animator; //CharacterChange;
    public Collider2D m_Col;
    public Rigidbody2D m_RigidBody;

    private void Awake()
    {
        m_Animator.Play("Run");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
     
        
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
