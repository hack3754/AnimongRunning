using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharObject : MonoBehaviour
{
    public Animator m_Animator;
    public SpriteRenderer[] m_Sprites;

    public void Clear()
    {
        StopAllCoroutines();
        if (m_Sprites.Length > 0) m_Sprites[0].color = Color.white;
    }

    public void SetBlink()
    {
        StartCoroutine("Blink");

    }

    private IEnumerator Blink()
    {
        Color tempColor = Color.white;
        while (GameData.m_ContinueTime > 0)
        {

            while (m_Sprites[1].color.a > 0.6f)
            {
                tempColor.a -= 0.1f;
                m_Sprites[1].color = tempColor;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
            while (m_Sprites[1].color.a < 1f)
            {
                tempColor.a += 0.1f;
                m_Sprites[1].color = tempColor;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        m_Sprites[1].color = Color.white;
    }
}
