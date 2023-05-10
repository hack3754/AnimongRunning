using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgUpdate : MonoBehaviour
{
    public Player m_Player;
    public Transform m_TransBg;
    public WayReuse m_WayReuse;
    public float m_Speed;
    Vector2 positionChange;
    private void Update()
    {
        m_TransBg.Translate(-(Time.deltaTime * m_Speed), 0, 0);
        m_WayReuse.UpdateWay(m_TransBg);

        if(m_Player.m_IsUp && Input.GetKey(KeyCode.UpArrow))
        {
            m_Player.m_Trans.Translate(0, (Time.deltaTime * m_Speed), 0);

            positionChange = new Vector2(0, (Time.deltaTime * m_Speed));
        }
        else if (m_Player.m_IsDown && Input.GetKey(KeyCode.DownArrow))
        { 
            m_Player.m_Trans.Translate(0, -(Time.deltaTime * m_Speed), 0);
            positionChange = new Vector2(0, -(Time.deltaTime * m_Speed));
        }

        //m_Player.Move(positionChange);
    }
}
