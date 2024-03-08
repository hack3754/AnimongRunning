using UnityEngine;
using TMPro;
using System;
using GooglePlayGames;
using System.Collections;
using System.IO;

public class UIResult : UIObject
{
    public ButtonObject m_BtnAds;
    public ButtonObject m_BtnResult;
    public ButtonObject m_BtnHome;
    public ButtonObject m_BtnRestart;
    public ButtonObject m_BtnShare;
    public ButtonObject m_BtnRanking;

    public GameObject m_ObjBtnContinue;
    public GameObject m_ObjBtnResult;

    public GameObject m_ObjNewRanking;
    public GameObject m_ObjNormalRanking;
    public TMP_Text m_TxtTime;
    public TMP_Text m_TxtScore;
    public TMP_Text m_TxtLastScore;
    public TMP_Text m_TxtGold;
    public TMP_Text m_TxtResultTime;

    public void Init()
    {
        m_BtnAds.m_FncOnClick = OnClickAds;
        m_BtnResult.m_FncOnClick = OnClickResult;

        m_BtnHome.m_FncOnClick = OnClickHome;
        m_BtnRestart.m_FncOnClick = OnClickRestart;

        m_BtnRanking.m_FncOnClick = OnClickRanking;
        m_BtnShare.m_FncOnClick = OnClickShare;
    }

    public void Show(TimeSpan time)
    {
        SetActive(true);
        m_TxtScore.text = ((int)GameData.m_Score).ToString();
        m_TxtTime.text = string.Format("{0}:{1}.{2}", time.Minutes.ToString("00"), time.Seconds.ToString("00"), ((int)(GameTimeSystem.GetTime().Milliseconds * 0.1f)).ToString("00"));
        m_TxtGold.text = GameData.m_Gold.ToString();
        m_ObjNewRanking.SetActive(GameData.m_Score > GameData.m_LocalData.m_Data.Score);
        m_ObjNormalRanking.SetActive(GameData.m_Score <= GameData.m_LocalData.m_Data.Score);

        GameData.m_LocalData.m_Data.SetScore();
        m_TxtLastScore.text = ((int)GameData.m_LocalData.m_Data.Score).ToString();

        GameData.m_LocalData.m_Data.Gold += GameData.m_Gold;
        GameData.m_LocalData.Save();

        GameManager.Instance.m_OutGameUI.SetCoin();

        if(GameData.m_IsContinue == false)
        {
            m_ObjBtnContinue.SetActive(true);
            m_ObjBtnResult.SetActive(false);
            StartCoroutine(StartResult());
        }
        else
        {
            SetResult();
        }
    }

    public void Hide()
    {
        SetActive(false);
    }

    void SetResult()
    {
        m_ObjBtnContinue.SetActive(false);
        m_ObjBtnResult.SetActive(true);

        Social.ReportScore((int)GameData.m_Score, "CgkIxYGzvLsTEAIQAQ", (bool success) => {

        });
    }

    IEnumerator StartResult()
    {
        int time = 10;
        m_TxtResultTime.text = time.ToString();
        while (true)
        {
            if(time <= 0)
            {
                SetResult();
                yield break;
            }

            yield return new WaitForSeconds(1);
            time--;
            m_TxtResultTime.text = time.ToString();
        }
    }

    void OnClickHome()
    {
        GameManager.Instance.GameRestart();
    }

    void OnClickRestart()
    {
        GameManager.Instance.GameRestart(false);
        //GameManager.Instance.GameContinue();
    }

    void OnClickAds()
    {
#if !UNITY_EDITOR
        AdsManager.Instance.ShowRewardedAd(GetReward);
#else
        GetReward();
#endif
    }

    void OnClickResult()
    {
        SetResult();
    }


    void GetReward()
    {
        GameData.m_ContinueTime = 2;
        GameData.m_IsContinue = true;
        GameManager.Instance.GameContinue();
    }

    void OnClickRanking()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIxYGzvLsTEAIQAQ");
#endif
    }

    void OnClickShare()
    {
        //ShareActivity("fadsf", "asfdgadf", "afeef");
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            //.SetSubject("Subject goes here").SetText("Hello world!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
            //.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }

    private AndroidJavaObject activity = null;

    private void CreateActivity()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    if(activity == null)
        activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").
            GetStatic<AndroidJavaObject>("currentActivity");
#endif

    }

    public void ShareActivity(string title, string subject, string body)
    {
        CreateActivity();
        AndroidJavaClass intent = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObj = new AndroidJavaObject("android.content.Intent");
        intentObj.Call<AndroidJavaObject>("setAction", intent.GetStatic<string>("ACTION_SEND"));
        intentObj.Call<AndroidJavaObject>("setType", "text/plan");
        intentObj.Call<AndroidJavaObject>("putExtra", intent.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObj.Call<AndroidJavaObject>("putExtra", intent.GetStatic<string>("EXTRA_TEXT"), body);

        AndroidJavaObject chooser = intent.CallStatic<AndroidJavaObject>("createChooser", intentObj, title);
        activity.Call("startActivity", chooser);
    }

    void takeScreenShotAndShare()
    {
        StartCoroutine(takeScreenshotAndSave());
    }

    private IEnumerator takeScreenshotAndSave()
    {
        string path = "";
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();


        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/GameOverScreenShot");
        path = Application.persistentDataPath + "/GameOverScreenShot" + "/DiedScreenShot.png";
        System.IO.File.WriteAllBytes(path, imageBytes);

        StartCoroutine(shareScreenshot(path));
    }

    private IEnumerator shareScreenshot(string destination)
    {
        string ShareSubject = "Picture Share";
        string shareLink = "Test Link";// + "\nhttp://stackoverflow.com/questions/36512784/share-image-on-android-application-from-unity-game";
        string textToShare = "Text To share";

        Debug.Log(destination);


        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), textToShare + shareLink);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ShareSubject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }
        yield return null;
    }
}
