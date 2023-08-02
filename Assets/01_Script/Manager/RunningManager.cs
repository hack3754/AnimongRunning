using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
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

    public float m_PlayerSpeed;
    public float m_PlayerJumpSpeed;
    public float m_HeightPeak;


    Vector3 m_Vec3;
    Vector2 m_Vec2;

    bool m_IsJump;
    bool m_isMove;
    bool m_IsJumpBlock;

    float m_Time = 0;

    //data
    float m_MaxSpeed;

    public void Init()
    {
        m_Player.Init();
        m_IsJumpBlock = false;
    }

    public void Running()
    {
        m_BgUpdate.BgMove(GameData.m_BGSpeed);

        GameData.m_Player.m_HP -= Time.deltaTime;
        GameManager.Instance.m_UIManager.SetHP(GameData.m_Player.m_HP);
        GameManager.Instance.m_UIManager.SetTime();

        m_Vec3 = Vector3.zero;
        m_Vec2 = Vector2.zero;

        if (GameData.m_BGSpeed < DataManager.Instance.m_GlobalData.max_speed)
        {
            if (m_Time >= 0.1f)
            {
                GameData.m_BGSpeed += DataManager.Instance.m_GlobalData.bg_speed;
                m_Time = 0;
               
            }

            if (GameData.m_BGSpeed >= DataManager.Instance.m_GlobalData.max_speed) GameData.m_BGSpeed = DataManager.Instance.m_GlobalData.max_speed;

            m_Time += Time.deltaTime;
            m_TransCamera.Translate(-(Time.deltaTime * 2f), 0, 0);
        }
       
   
        if (m_IsJump == false)
        {
#if UNITY_EDITOR
            float Vertical = Input.GetAxis("Vertical") * m_PlayerSpeed * Time.deltaTime;
            m_Vec2.y += Vertical;
            m_Player.m_RigidBody.velocity = m_Vec2;
            
            if (Vertical == 0 && GameData.m_BGSpeed >= DataManager.Instance.m_GlobalData.max_speed)
            {
                GameData.m_Player.m_SprintTime += Time.deltaTime;
                if (GameData.m_Player.m_IsSprint == false && GameData.m_Player.m_SprintTime > 2)
                {
                    GameData.m_Player.m_IsSprint = true;
                    GameData.m_BGSpeed = DataManager.Instance.m_GlobalData.sprint_speed;

                    m_Vec3 = m_TransCamera.localPosition;
                    m_Vec3.x = -4.16f;
                    m_TransCamera.localPosition = m_Vec3;
                }
            }
            else
            {
                if (GameData.m_Player.m_IsSprint == true)
                {
                    GameData.m_Player.m_IsSprint = false;
                    ResetRunning();
                }

                GameData.m_Player.m_SprintTime = 0;
            }
            

            if (Input.GetKey(KeyCode.Z))
            {
                Jump("Jump_01");
            }

            if (Input.GetKey(KeyCode.X))
            {
                Jump("Jump_02");
            }

            if (Input.GetKey(KeyCode.C))
            {
                Jump("Gomong_Jump_Run");
            }
#endif
        }
    }

    public void ResetRunning()
    {
        GameData.m_BGSpeed = 0;
        m_IsJumpBlock = false;
    }

    public void SetTrap(TrapCollider trap)
    {
        if (trap == null || trap.m_tData == null) return;
        switch(trap.m_tData.type)
        {
            case TrapType.Slow:
                GameData.m_BGSpeed = trap.m_tData.value;
                m_IsJumpBlock = true;
                break;
        }
    }

    public void Jump(string aniName)
    {
        if (m_IsJumpBlock) return;
        m_Player.Jump(aniName);
        m_Player.m_Col.enabled = false;
        m_Player.m_RigidBody.velocity = Vector2.zero;
        m_IsJump = true;
    }

    public void EndJump()
    {
        m_IsJump = false;
    }
}
