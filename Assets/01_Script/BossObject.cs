using UnityEngine;

public class BossObject : ObjectEntries
{
    public Transform m_TransFirst;
    public Transform m_TransLast;

    public float m_PosLast;
    System.Action m_FncSetBossPosition;
    Vector3 m_HpPos;
    float m_PosFirst;
    float m_HpPer;
    float m_FirstHp;
    bool m_IsShow;
    Vector3 m_Vec3;
    float m_PosY;
    bool m_IsMove;


    float m_Speed;
    float m_Distance;

    public void Init(System.Action fncSetBossPosition)
    {
        m_PosFirst = m_Trans.localPosition.x;
        m_FncSetBossPosition = fncSetBossPosition;
    }

    public void MoveUpdate(float hpValue)
    {   
        m_HpPer = hpValue / GameData.m_Player.m_MaxHP;
        
        if (m_HpPer <= 0.4f)
        {
            m_Obj.SetActive(true);

            if (m_IsShow == false)
            {
                m_HpPos = m_Trans.position;
                m_FncSetBossPosition?.Invoke();
                SetSpeed();
            }

            m_IsShow = true;

            m_PosY = m_Trans.position.y;
            //m_Trans.position = Vector3.Lerp(m_HpPos, m_TransLast.position, 1 - (m_HpPer / m_FirstHp));
            //Vector3 speed = Vector3.zero;
            //m_Trans.position = Vector3.SmoothDamp(m_Trans.position, m_TransLast.position, ref speed, 0.1f);
            //m_Trans.position = Vector3.MoveTowards(m_Trans.position, m_TransLast.position, (m_HpPer / m_FirstHp));

            m_Vec3 = m_Trans.position;
            m_Vec3.x += m_Speed * Time.deltaTime;
            m_Vec3.y = m_PosY;
            m_Trans.position = m_Vec3;
        }
        else
        {
            /*
            if(m_IsShow)
            {
                m_IsShow = false;

                m_PosY = m_Trans.position.y;
                m_Trans.position = Vector3.MoveTowards(m_Trans.position, m_TransFirst.position, 20 * Time.deltaTime);

                m_Vec3 = m_Trans.position;
                m_Vec3.y = m_PosY;
                m_Trans.position = m_Vec3;

                if (m_Trans.position.x <= m_TransFirst.position.x)
                {
                    m_IsShow = false;
                    m_Obj.SetActive(false);
                }
            }
            */
  
        }        
    }

    public void SetSpeed()
    {
        Debug.Log(GameData.m_Player.m_HP / GameData.m_Player.m_MaxHP);
        if ((GameData.m_Player.m_HP / GameData.m_Player.m_MaxHP) > 0.4f)
        {
            m_IsShow = false;
            return;
        }
        m_Distance = Vector3.Distance(m_Trans.position, m_TransLast.position);
        m_Speed = m_Distance / GameData.m_Player.m_HP;
    }

    public void SetPosition(Vector3 pos)
    {
        m_Vec3 = m_Trans.position;
        m_Vec3.y = pos.y;
        m_Trans.position = m_Vec3;
    }
    public void GameReset()
    {
        m_Obj.SetActive(false);
        m_IsShow = false;
        m_Speed = 0;
        m_Trans.position = m_TransFirst.position;
    }
}
