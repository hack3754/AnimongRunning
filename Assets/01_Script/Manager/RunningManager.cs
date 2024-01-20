using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
public enum JumpState
{
    Up,
    Down,
}

public class RunningManager : MonoBehaviour
{
    public Camera m_Cam;
    public GameObject m_Obj;
    public Player m_Player;
    public BgUpdate m_BgUpdate;
    public Transform m_TransCamera;
    public LaneObject[] m_LaneObjects;
    public Transform[] m_Points;

    public float m_PlayerSpeed;
    public float m_VerticalSpeed;
    public float m_PlayerJumpSpeed;
    public float m_HeightPeak;
    public Vector3 m_StartPos;

    Vector3 m_PosOri;
    bool m_IsJump;
    bool m_IsJumpBlock;

    Vector2 m_Vec2;
    Vector3 m_Vec3;

    public float m_Vertical;
    public int m_Dir;
    public Vector3 m_BeginPos;
    public Vector3 m_DragPos;
    public float m_LanePos;

    int m_LaneIndex;


    float m_Time = 0;

    //data
    float m_MaxSpeed;
    bool m_IsDrag;

    List<StateInfo> m_States = new List<StateInfo>();

    public void Init()
    {
        m_Player.Init();
        m_IsJumpBlock = false;
        m_PosOri = m_BgUpdate.m_TransBg.localPosition;

        m_LaneIndex = m_LaneObjects.Length / 2;
    }

    public void GameReset()
    {
        m_Time = 0;
        GameData.GameReset();
        m_Player.GameReset();
        m_BgUpdate.m_TransBg.localPosition = m_PosOri;
        m_BgUpdate.GameReset();
        m_States.Clear();

        GameManager.Instance.m_IsStop = false;
        GameManager.Instance.m_IsStun = false;

        m_LaneIndex = m_LaneObjects.Length / 2;
        m_Player.m_RigidBody.position = m_Points[m_LaneIndex].position;
    }

    public void SetOutGame()
    {
        m_Obj.SetActive(true);
        m_Player.SetPlayer();
        GameManager.Instance.m_InGameUI.SetHP(GameData.m_Player.m_MaxHP);
    }

    public void GameReadyStart()
    {
        /*
        m_Time += Time.deltaTime;
        m_BgUpdate.m_TransBg.localPosition = Vector3.Lerp(m_PosOri, m_StartPos, m_Time);
        if (m_Time < 1) GameManager.Instance.m_BGControl.BgMove();
        if (m_Time >= 1)
        {
            StartCoroutine(GameStart());
        }
        */
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        m_Player.Idle();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.GameStart();
        m_Time = 0;
    }

    public void Running()
    {
        GameData.m_Player.m_HP -= Time.deltaTime;
        GameManager.Instance.m_InGameUI.SetHP(GameData.m_Player.m_HP);
        if (GameData.m_Player.m_HP <= 0)
        {
            //GameOver;
            GameOverBlock();
            m_Player.TimeOut();
            return;
        }

        //GameManager.Instance.m_InGameUI.SetTime();

        if (GameManager.Instance.m_IsStop == false && GameManager.Instance.m_IsStun == false)
        {
            m_BgUpdate.BgMoveScore(GameData.m_BGSpeed);

            m_Vec2 = Vector2.zero;


            if (GameData.m_SpeedSlow > 0)
            {
                if (GameData.m_BGSpeed > DataManager.Instance.m_BGData.min_speed)
                {
                    GameData.m_BGSpeed -= GameData.m_SpeedSlow * Time.deltaTime;
                    if (GameData.m_BGSpeed <= DataManager.Instance.m_BGData.min_speed) GameData.m_BGSpeed = DataManager.Instance.m_BGData.min_speed;
                }
            }
            else if(GameData.m_SpeedUp > 0)
            {
                if (GameData.m_BGSpeed < DataManager.Instance.m_BGData.max_speed_up)
                {
                    GameData.m_BGSpeed += GameData.m_SpeedUp * Time.deltaTime;
                    if (GameData.m_BGSpeed >= DataManager.Instance.m_BGData.max_speed_up) GameData.m_BGSpeed = DataManager.Instance.m_BGData.max_speed_up;
                }
            }
            else
            {

                if (GameData.m_BGSpeed < DataManager.Instance.m_BGData.max_speed)
                {
                    GameData.m_BGSpeed += DataManager.Instance.m_BGData.bg_speed * Time.deltaTime;
                    if (GameData.m_BGSpeed >= DataManager.Instance.m_BGData.max_speed) GameData.m_BGSpeed = DataManager.Instance.m_BGData.max_speed;
                }
                else if (GameData.m_BGSpeed > DataManager.Instance.m_BGData.max_speed)
                {
                    GameData.m_BGSpeed -= DataManager.Instance.m_BGData.bg_speed * Time.deltaTime;
                    if (GameData.m_BGSpeed <= DataManager.Instance.m_BGData.max_speed) GameData.m_BGSpeed = DataManager.Instance.m_BGData.max_speed;
                }
                /*
                if (GameData.m_CAMSpeed < DataManager.Instance.m_BGData.cam_max_speed)
                {
                    GameData.m_CAMSpeed += DataManager.Instance.m_BGData.cam_speed * Time.deltaTime;
                    Debug.Log("CAM SPEED " + GameData.m_CAMSpeed);
                    m_TransCamera.Translate(-(GameData.m_CAMSpeed * Time.deltaTime), 0, 0);
                }
                */
            }
        }


        if (m_IsJump == false)
        {
#if UNITY_EDITOR
            /* Touch Move
            if (Input.GetMouseButtonUp(0))
            {
                m_IsDrag = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_IsDrag = true;
                m_BeginPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                
            }

            if (m_IsDrag && Input.GetMouseButton(0))
            {
                m_DragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_BeginPos = m_DragPos;
            }

            float Vertical = m_Vertical * m_PlayerSpeed * Time.deltaTime;
            m_Vec2.y += Vertical;
            m_Player.m_RigidBody.velocity = m_Vec2;
            */

            /*
            if (Vertical == 0 && GameData.m_BGSpeed >= DataManager.Instance.m_BGData.max_speed)
            {
                GameData.m_Player.m_SprintTime += Time.deltaTime;
                if (GameData.m_Player.m_IsSprint == false && GameData.m_Player.m_SprintTime > 2)
                {
                    GameData.m_Player.m_IsSprint = true;
                    GameData.m_BGSpeed = DataManager.Instance.m_BGData.sprint_speed;

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
            */

            //float Vertical = Input.GetAxis("Vertical") * m_PlayerSpeed * Time.deltaTime;
            //m_Vec2.y += Vertical;
            //m_Player.m_RigidBody.velocity = m_Vec2;

            if (GameManager.Instance.m_IsStun == false)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MoveUp();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MoveDown();
                }
                /*
                if (Input.GetKey(KeyCode.Z))
                {
                    Jump("Gomong_Jump_Run");
                }
                */
            }
#endif
        }
    }

    public void GameOverBlock()
    {
        StartCoroutine(GameOverPlayer());
    }
    IEnumerator GameOverPlayer()
    {
        m_States.Clear();
        GameManager.Instance.GameOverBlock();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.GameOver();
    }

    public void ResetRunning()
    {
        //GameData.m_SlowSpeed = 0;
        //m_IsJumpBlock = false;
        GameManager.Instance.m_IsStop = false;
    }

    public void MoveUp()
    {
        if(GameData.m_IsMove == false)
        {
            return;
        }
        if (m_LaneIndex <= 0)
        {
            m_LaneIndex = 0;
            return;
        }

        if (m_Player.m_Map != null)
        {
            if (GameManager.Instance.m_IsStop == false && m_Player.m_Map.m_BlockIndex.Contains(m_LaneIndex - 1))
            {
                return;
            }
        }
        m_LaneIndex--;

        m_Player.m_RigidBody.position = m_Points[m_LaneIndex].position;
    }

    public void MoveDown()
    {
        if (GameData.m_IsMove == false)
        {
            return;
        }
        if (m_LaneIndex >= m_LaneObjects.Length - 1)
        {
            m_LaneIndex = m_LaneObjects.Length - 1;
            return;
        }

        if (m_Player.m_Map != null)
        {
            if (GameManager.Instance.m_IsStop == false && m_Player.m_Map.m_BlockIndex.Contains(m_LaneIndex + 1))
            {
                return;
            }
        }

        m_LaneIndex++;

        m_Player.m_RigidBody.position = m_Points[m_LaneIndex].position;
    }

    public void SetTrap(TrapCollider trap)
    {
        if (trap == null || trap.m_tData == null) return;

        ObstacleTrigger(trap.m_tData.type, trap.m_tData.value, trap);
        ObstacleTrigger(trap.m_tData.type2, trap.m_tData.value2, trap);
    }

    void ObstacleTrigger(TrapType trapType, int dataValue, TrapCollider trap)
    {
        float dataTime = 0;
        if (trap.m_tData.time > 0) dataTime = trap.m_tData.time / 100f;


        if(!trap.m_tData.IsTrap)
        {
            trap.SetDisable();
        }

        switch (trapType)
        {
            case TrapType.Slow:
                GameData.m_SpeedSlow = dataValue;
                m_IsJumpBlock = true;
                if(dataTime > 0)
                {
                    AddDotTrap(trapType, dataValue, dataTime, !trap.m_tData.IsTrap, trap.m_tData.res);
                }
                break;
            case TrapType.SpdUp:
                if (dataTime > 0)
                {
                    if (trap.m_tData.IsTrap)
                    {
                        m_Player.Slip();
                        GameData.m_IsMove = false;
                    }
                    AddDotTrap(trapType, dataValue, dataTime, !trap.m_tData.IsTrap, trap.m_tData.res);
                }
                break;
            case TrapType.Score:
                m_BgUpdate.SetScore(dataValue);
                break;
            case TrapType.Gold:
                m_BgUpdate.SetGold(dataValue);
                GameManager.Instance.m_MoveObj.SetMoveObject(trap.transform.position, GameData.m_PosGold, trap.m_tData.value2 - 1);
                break;
            case TrapType.Stop:
                GameManager.Instance.m_IsStun = true;
                GameData.m_BGSpeed = DataManager.Instance.m_BGData.min_speed;
                StartCoroutine(RunAgain(dataTime));
                break;
            case TrapType.Blow:
                trap.SetDisable();
                break;
            case TrapType.DmgStop:
                GameManager.Instance.m_IsStop = true;
                m_Player.SetDmgStop(trap);
                break;
            case TrapType.DotDmg:
                if (dataTime > 0)
                {
                    AddDotTrap(trapType, dataValue / 100f, dataTime, !trap.m_tData.IsTrap, trap.m_tData.res);
                }
                break;
            case TrapType.Death:
                m_Player.TrapDeath();
                GameOverBlock();
                break;
            case TrapType.HpRecovery:
                GameData.m_Player.m_HP += dataValue;
                /*
                if(GameData.m_Player.m_HP >= GameData.m_Player.m_MaxHP)
                {
                    GameData.m_Player.m_HP = GameData.m_Player.m_MaxHP;
                }
                */
                break;
            case TrapType.Dmg:
                GameData.m_Player.m_HP -= dataValue;
                break;
            case TrapType.Rainboots:
                m_Player.SetRainboots(true);
                AddDotTrap(trapType, dataValue, dataTime, !trap.m_tData.IsTrap, trap.m_tData.res);
                break;
        }
    }

    IEnumerator RunAgain(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.m_IsStun = false;
    }

    public bool AddDotTrap(TrapType trapType, float dataValue, float dataTime, bool isItem, string res)
    {
        for (int i = 0; i < m_States.Count; i++)
        {
            if (m_States[i].m_Type == trapType)
            {
                m_States[i].m_StateTime = 0;
                m_States[i].m_Time = dataTime;
                m_States[i].m_Value = dataValue;
                return false;
            }
        }

        StateInfo stateInfo = new StateInfo();
        stateInfo.m_Res = res;
        stateInfo.m_StateTime = 0;
        stateInfo.m_Time = dataTime;
        stateInfo.m_Type = trapType;
        stateInfo.m_Value = dataValue;

        m_States.Add(stateInfo);
        GameManager.Instance.m_InGameUI.m_UIMian.SetState(stateInfo, isItem);
        return true;
    }    

    public void DeleteDotTrap(StateInfo info)
    {
        if (GameManager.Instance.IsGameStart == false) return;
        if (info.m_Type == TrapType.Slow)
        {
            GameData.m_SpeedSlow = 0;
            m_IsJumpBlock = false;
        }
        else if (info.m_Type == TrapType.SpdUp)
        {
            m_Player.Run();
            GameData.m_SpeedUp = 0;
            GameData.m_IsMove = true;
        }
        else if(info.m_Type == TrapType.Rainboots)
        {
            m_Player.SetRainboots(false);
        }

        if(m_States.Contains(info)) m_States.Remove(info);
    }

    public void SetDotTrigger(StateInfo stateInfo)
    {
        switch (stateInfo.m_Type)
        {
            case TrapType.Slow:
                GameData.m_SpeedSlow = stateInfo.m_Value;
                m_IsJumpBlock = true;
                break;
            case TrapType.SpdUp:
                GameData.m_SpeedUp = stateInfo.m_Value;
                GameManager.Instance.m_IsSpeedUp = true;
                break;
            case TrapType.DotDmg:
                GameData.m_Player.m_HP -= stateInfo.m_Value;
                break;
        }
    }

    public void Jump(string aniName)
    {
        if (GameManager.Instance.m_IsStop) return;

        if (m_IsJumpBlock || m_IsJump) return;
        m_Player.Jump(aniName);
        m_Player.m_Col.enabled = false;
        //m_Player.m_RigidBody.velocity = Vector2.zero;
        m_IsJump = true;
    }

    public void EndJump()
    {
        m_IsJump = false;
    }
}
