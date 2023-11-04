using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginUIMain : UIObject
{
    public GameObject m_ObjLoading;
    public TMP_Text m_TxtLoading;
    public void Init()
    {

    }
    public void Show()
    {
        SetActive(true);
    }
    public void ShowLoading()
    {
        m_ObjLoading.SetActive(true);
        StartLoading();
    }

    public void Hide()
    {
        StopAllCoroutines();
        SetActive(false);
    }

    public void StartLoading()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        WaitForSeconds wait = new WaitForSeconds(0.4f);
        int count = 0;
        string str = "LOADING";
        string logining = string.Empty;
        while (true)
        {
            logining = str;

            for (int i = 0; i < count; i++)
            {
                logining += ".";
            }
            m_TxtLoading.text = logining;
            count++;
            if (count == 5)
            {
                count = 0;
            }

            yield return wait;

        }
    }
}
