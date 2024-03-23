using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public enum SoundType
	{
		None,
		Click,
		Cancel,
		Confirm,
		Close,
		StageSelect,
	}

	public bool m_IsClickAni;
	public int m_Index;
	public GameObject m_Obj;
	public GameObject m_ObjAni;
	public RectTransform m_Rect;
	public Button m_Btn;
	public TMP_Text m_TxtName;
	public SoundType m_SoundType = SoundType.Click;
	public UnityEngine.Events.UnityAction m_FncOnClick;
	public UnityEngine.Events.UnityAction<bool> m_FncPress;

	Vector3 m_PressVec = new Vector3(0.95f, 0.95f, 1f);

	void Awake()
	{
		MenuSet();
        m_Btn.onClick.AddListener(OnClickButton);
	}

	public void SetActive(bool isActive)
	{
		m_Obj.SetActive(isActive);
	}

	private void OnDisable()
	{
		RestScale();
	}

	public void RestScale()
	{
		if (m_IsClickAni && m_ObjAni != null && !m_ObjAni.transform.localScale.Equals(Vector3.one))
		{
			LeanTween.cancel(m_ObjAni);
			m_ObjAni.transform.localScale = Vector3.one;
		}
	}

	public void OnPointerDown(PointerEventData evt)
	{
		// 타임스케일에 영향 받지 않게 수정
		if(m_IsClickAni) LeanTween.scale(m_ObjAni, m_PressVec, 0.3f).setEaseOutQuart().setIgnoreTimeScale(true);
		if (m_FncPress!= null) m_FncPress(true);
	}

	public void OnPointerUp(PointerEventData evt)
	{
		if (m_IsClickAni)
		{
            LeanTween.cancel(m_ObjAni);
            m_ObjAni.transform.localScale = m_PressVec;
            LeanTween.scale(m_ObjAni, Vector3.one, 0.3f).setEaseOutQuart().setIgnoreTimeScale(true);
		}
        if (m_FncPress != null) m_FncPress(false);
	}

	protected virtual void OnClickButton()
	{
		PlaySound();
		m_FncOnClick?.Invoke();
	}

	void PlaySound()
	{
        switch (m_SoundType)
        {
            case SoundType.Click:
				//GameManager.Instance.m_Sound.PlayEffectSound(1);
				break;

		}
    }

	[ContextMenu("셋팅")]
	void MenuSet()
	{
		if (m_Obj == null) m_Obj = gameObject;
		if (m_Rect == null) m_Rect = GetComponent<RectTransform>();
		if (m_Btn == null) m_Btn = GetComponent<Button>();
		if (m_ObjAni == null) m_ObjAni = m_Obj;
	}
}
