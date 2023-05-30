using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BgUpdate : MonoBehaviour
{
    public Player m_Player;
    public Transform m_TransBg;
    public WayReuse m_WayReuse;

    public void BgMove(float bgSpeed)
    {
        m_TransBg.Translate(-(Time.deltaTime * bgSpeed), 0, 0);
        m_WayReuse.UpdateWay(m_TransBg);     
    }
}
