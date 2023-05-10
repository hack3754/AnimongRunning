using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform m_Trans;
    public Collider2D m_Col;
    public Rigidbody2D m_RigidBody;
    public bool m_IsUp;
    public bool m_IsDown;
    public List<RaycastHit2D> m_Hits;
    GameObject m_BGCollision;
    Vector3 m_Vec3;
    private void Awake()
    {
        m_IsUp = true;
        m_IsDown = true;
        m_Hits = new List<RaycastHit2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TopCollider"))
        {
            m_IsUp = false;            
            Debug.Log("Top Enter");
        }

        if (collision.gameObject.CompareTag("BottomCollider"))
        {
            m_IsDown = false;
            Debug.Log("Bottom Enter");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("TopCollider"))
        {
            m_IsUp = true;
            Debug.Log("Top Exit");
        }
        if (collision.gameObject.CompareTag("BottomCollider"))
        {
            m_IsDown = true;
            Debug.Log("Bottom Exit");
        }
        
    }

    public void Move(Vector2 positionChange)
    {
        //Vector2 positionChange = new Vector2(m_Trans.localPosition.x, m_Trans.localPosition.y) * Time.deltaTime;
        ContactFilter2D filter2d = new ContactFilter2D();
        m_Col.Cast(positionChange, filter2d, m_Hits, positionChange.magnitude);

        if(m_Hits.Count > 0)
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
}
