using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MSingleton<GameManager>
{
    public SoundManager m_Sound;
    public RunningManager m_Running;
    public TrapColliderManager m_TrapColliderMgr;
    public LoginUIMain m_LoignUI;
    public InGameUIMain m_InGameUI;
    public OutGameUIMain m_OutGameUI;
    public BGControl m_BGControl;
    public GameObject m_BG;
    public bool m_IsStop;
    public bool m_IsStun;
    bool m_IsGameStart;
    bool m_IsReadyStart;
    private void Awake()
    {
        m_LoignUI.Init();
        m_IsStop = false;
        m_IsStun = false;
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
            key = ResourceKey.GetKey(ResourceKey.m_KeyCharPrefab, dicData.Value.res);

            if (string.IsNullOrEmpty(key)) continue;

            yield return StartCoroutine(ResourceManager.Instance.LoadPrefab(key, AddPrefab));
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

    void ShowOutGame()
    {
        m_LoignUI.Hide();
        m_OutGameUI.Show();
        m_Running.SetOutGame();
    }

    public void GameReadyStart()
    {
        m_IsReadyStart = true;
        m_InGameUI.Reset();
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
