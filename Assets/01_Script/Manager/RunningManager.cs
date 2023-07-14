using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum JumpState
{
    Up,
    Down,
}

public class RunningManager : MonoBehaviour
{
    public Player m_Player;
    public BgUpdate m_BgUpdate;
    public Transform m_TransCamera;

    public Transform[] m_Points;

    public float m_BGSpeed;
    public float m_PlayerSpeed;
    public float m_PlayerJumpSpeed;
    public float m_HeightPeak;


    Vector3 m_Vec3;
    Vector2 m_Vec2;

    float m_JumpValue;
    Vector2 m_PerJumpValue;

    bool m_IsJump;
    JumpState m_JumpState;

    bool m_isMove;
    float m_Time = 0;

    //data
    float m_MaxSpeed;
    public void Running()
    {
        m_Time += Time.deltaTime;
        m_BgUpdate.BgMove(m_BGSpeed);

        m_Vec3 = Vector3.zero;
        m_Vec2 = Vector2.zero;

        if (Input.GetKey(KeyCode.F1))
        {
            m_isMove = true;
            m_Time = 0;

            m_BGSpeed = 10;
        }

        if (m_isMove)
        {
            m_TransCamera.Translate(-(Time.deltaTime), 0, 0);
        }

        if(m_TransCamera.localPosition.x <= -5.16f)
        {
            m_Vec3 = m_TransCamera.localPosition;
            m_Vec3.x = -5.16f;
            m_TransCamera.localPosition = m_Vec3;
            m_isMove = true;
        }


        if (m_IsJump)
        {
            m_Vec2 = m_PerJumpValue;

            if (m_JumpState == JumpState.Up)
            {
                m_JumpValue += m_PlayerJumpSpeed * Time.deltaTime;
            }
            else if (m_JumpState == JumpState.Down)
            {
                m_JumpValue -= m_PlayerJumpSpeed * Time.deltaTime;
            }

            if (m_JumpValue >= m_HeightPeak)
            {
                m_JumpState = JumpState.Down;
                m_JumpValue = m_HeightPeak;
            }

            if (m_JumpValue <= 0)
            {
                m_IsJump = false;
                m_Player.m_RigidBody.position = m_PerJumpValue;
                m_Player.m_Col.enabled = true;
            }
            else
            {
                m_Vec2.y = m_PerJumpValue.y + m_JumpValue;
                m_Player.m_RigidBody.position = m_Vec2;
            }

        }
        else
        {
#if UNITY_EDITOR
            float Vertical = Input.GetAxis("Vertical") * m_PlayerSpeed * Time.deltaTime;
            //Debug.Log(Vertical);
            m_Vec2.y += Vertical;
            m_Player.m_RigidBody.velocity = m_Vec2;

            if(m_IsJump == false && Input.GetKey(KeyCode.Z))
            {
                Jump("Jump_01");
            }

            if (m_IsJump == false && Input.GetKey(KeyCode.X))
            {
                Jump("Jump_02");
            }

            if (m_IsJump == false && Input.GetKey(KeyCode.C))
            {
                Jump("Gomong_Jump_Run");
            }
#endif
        }
    }

    public void Jump(string aniName)
    {
        m_Player.Jump(aniName);
        m_Player.m_Col.enabled = false;
        m_IsJump = true;
        m_JumpState = JumpState.Up;
        m_JumpValue = 0;
        m_PerJumpValue = m_Player.m_RigidBody.position;
    }
}
