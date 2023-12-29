using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MSingleton<GameManager>
{
    public Camera m_Cam;
    public Camera m_UICam;
    public CanvasScaler m_OutGameScaler;
    public CanvasScaler m_InGameScaler;
    public SoundManager m_Sound;
    public RunningManager m_Running;
    public TrapColliderManager m_TrapColliderMgr;
    public LoginUIMain m_LoignUI;
    public InGameUIMain m_InGameUI;
    public OutGameUIMain m_OutGameUI;
    public BGControl m_BGControl;
    public GameObject m_BG;
    public bool m_IsStop;
    public bool m_IsSpeedUp;
    public bool m_IsStun;
    bool m_IsGameStart;
    bool m_IsReadyStart;
    List<Coroutine> m_StateCoroutine;

    public bool IsGameStart { get { return m_IsGameStart; } }

    private void Awake()
    {
        m_LoignUI.Init();
        m_IsStop = false;
        m_IsStun = false;
        m_IsSpeedUp = false;
        m_StateCoroutine = new List<Coroutine>();

#if UNITY_EDITOR

#else
        Application.targetFrameRate = 60;
#endif
        int setWidth = 1280; // 사용자 설정 너비
        int setHeight = 720; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        float oriAspect = 0;
        float currentAspect = 0;

        oriAspect = 720f / 1280f;
        currentAspect = (float)Screen.height / (float)Screen.width;
        /*
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            m_Cam.orthographicSize = (5 * currentAspect) / oriAspect;
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            //m_Cam.orthographicSize = (5 * oriAspect) / currentAspect;
            SetResolution();
        }
        */
        SetResolution();
    }

    public void SetResolution()
    {
        int setWidth = 1280; // 사용자 설정 너비
        int setHeight = 720; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        //m_OutGameScaler.referenceResolution *= new Vector2((m_OutGameScaler.referenceResolution.x / Screen.width), (Screen.height / m_OutGameScaler.referenceResolution.y));
        //m_InGameScaler.referenceResolution *= new Vector2((m_InGameScaler.referenceResolution.x / Screen.width), (Screen.height / m_InGameScaler.referenceResolution.y));

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            m_Cam.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
            m_UICam.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용

            //m_OutGameScaler.referenceResolution *= new Vector2((m_OutGameScaler.referenceResolution.x / Screen.width), (Screen.height / m_OutGameScaler.referenceResolution.y));
            //m_InGameScaler.referenceResolution *= new Vector2((m_InGameScaler.referenceResolution.x / Screen.width), (Screen.height / m_InGameScaler.referenceResolution.y));
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            m_Cam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
            m_UICam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용

            //m_OutGameScaler.referenceResolution *= new Vector2((Screen.width / m_OutGameScaler.referenceResolution.x), (m_OutGameScaler.referenceResolution.y / Screen.height));
            //m_InGameScaler.referenceResolution *= new Vector2((Screen.width / m_OutGameScaler.referenceResolution.x), (m_OutGameScaler.referenceResolution.y / Screen.height));
        }
    }

    private void Start()
    {
        m_BG.SetActive(false);
        m_LoignUI.ShowLoading();
        DataManager.Instance.LoadFirstLoad();
    }

    public void Init()
    {
        GameData.Init();
        m_TrapColliderMgr.Init();
        m_Running.Init();
    }

    public void UIInit()
    {
        m_InGameUI.Init();
        m_OutGameUI.Init();
    }

    public void ShowLoading()
    {
        m_LoignUI.ShowLoading();
    }

    public void GameReady()
    {
        //StartCoroutine(m_Running.m_BgUpdate.FirstMapLoad(GameStart));
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        Init();

        Dictionary<int, CharDataItem> dic = DataManager.Instance.m_CharData.Get();
        string key;
        foreach (var dicData in dic)
        {
            key = ResourceKey.GetKey(ResourceKey.m_KeyCharUIPrefab, dicData.Value.res);

            if (!string.IsNullOrEmpty(key))
            {
                yield return StartCoroutine(ResourceManager.Instance.LoadPrefab(key, AddPrefab));
            }

            key = ResourceKey.GetKey(ResourceKey.m_KeyCharPrefab, dicData.Value.res);

            if (!string.IsNullOrEmpty(key))
            {
                yield return StartCoroutine(ResourceManager.Instance.LoadPrefab(key, AddPrefab));
            }
        }


        yield return StartCoroutine(m_Running.m_BgUpdate.FirstMapLoad());

        UIInit();
        m_BG.SetActive(true);
        ShowOutGame();
        m_Running.m_BgUpdate.InitMap();
    }

    void AddPrefab(string key, GameObject prefab)
    {
        ResourceLoadData.Instance.AddPrefab(key, prefab);
    }

    public void GameRestart()
    {
        m_InGameUI.GameReset();
        m_LoignUI.Show();
        m_LoignUI.ShowLoading();
        m_Running.GameReset();
        m_Running.m_Player.Idle();
        StartCoroutine(LoadMap());
    }

    IEnumerator LoadMap()
    {
        yield return StartCoroutine(m_Running.m_BgUpdate.FirstMapLoad());

        m_BG.SetActive(true);
        ShowOutGame();
        m_Running.m_BgUpdate.InitMap();
    }

    public void GameOver()
    {
        m_IsGameStart = false;
        m_InGameUI.ShowResult((int)GameData.m_Score, GameTimeSystem.GetTime());
        //m_Running.m_Player.Idle();
    }

    public void GameOverBlock()
    {
        m_IsGameStart = false;
    }

    void ShowOutGame()
    {
        m_LoignUI.Hide();
        m_OutGameUI.Show();
        m_Running.SetOutGame();
    }

    public void GameReadyStart()
    {
        m_IsReadyStart = true;
        m_InGameUI.GameReset();
        m_InGameUI.Show();
        m_Running.m_Player.Run();
    }

    public void GameStart()
    {
        m_Running.m_Player.Run();
        m_IsGameStart = true;
        m_IsReadyStart = false;
        GameTimeSystem.SetTime();
    }

    private void Update()
    {
        if(m_IsReadyStart)
        {
            m_Running.GameReadyStart();
        }

        if (m_IsGameStart == false) return;
        m_Running.Running();
        if (m_IsStop || m_IsStun) return;
        m_BGControl.BgMove();
    }

}
