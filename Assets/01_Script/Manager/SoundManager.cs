using MRPG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    readonly string SOUND_MUTE = "SOUND_MUTE";
    readonly int TOTAL_SOUND_MAX = 10;

    public AudioSource  m_audioSoundBGM;
	public AudioSource  m_audioSoundVoice;
	public GameObject   m_soundSourceObject;

    public System.Action <AudioClip>          onEnd;

    public Dictionary<string, AudioClip> m_dicSound = new Dictionary<string, AudioClip>();

    public bool     m_EditorMute = false;
    public bool     m_Editor = false;
    float           m_BGMFadeTime   = 0f;

    public float bgmVolume
    {
        get
        {
#if UNITY_EDITOR
            if (m_EditorMute)
                return 0f;
            return m_Editor ? 1f : GameSetting.m_BGMVolume;
#endif
            return GameSetting.m_BGMVolume;
        }
    }
    public float effectVolume
    {
        get
        {
#if UNITY_EDITOR
            if (m_EditorMute)
                return 0f;
            return m_Editor ? 1f : GameSetting.m_EffectVolume;
#endif
            return GameSetting.m_EffectVolume;
        }
    }

    AudioSource m_audioSoundEffectLoop;
	string m_soundName;

    public void ReleaseSound()
    {
        Debug.Log($"--SoundManager : ReleaseSound()");
        m_dicSound.Clear();
    }

    public void PlayBGM( int bgm, bool isFadeIn = false, float delay = 0f, bool addHandle = true )
    {
        var soundData = DataManager.Instance.m_SoundData.Get( bgm );
        if (soundData == null)
        {
            return;
        }

        string fineName = soundData.res;
        AudioClip clip = null;
        if (m_dicSound.TryGetValue(fineName, out clip))
        {
            Play( clip , isFadeIn, delay );
        }
        else
        {
            ResourceManager.Instance.LoadAsync<AudioClip>(fineName, (audioClip, sucess) =>
            {
                if (null != audioClip)
                {
                    if (m_dicSound.ContainsKey(fineName) == false)
                    {
                        m_dicSound.Add(fineName, audioClip);

                        Play(audioClip, isFadeIn, delay);
                    }
                }
            });

            m_BGMFadeTime = delay;
        }

        void Play( AudioClip audioClip, bool isFadeIn, float delay = 0f )
        {
            m_audioSoundBGM.clip = audioClip;
            m_audioSoundBGM.loop = true;

            if( isFadeIn == false )
            {
                m_audioSoundBGM.volume = bgmVolume;
                m_audioSoundBGM.Play();
            }
            else
                StartCoroutine( FadeInBGM( delay ) );
        }

    }
    public void StopBGM( bool fadeOut = false, float delay = 0f, System.Action onNext = null )
    {
        Debug.Log($"--SoundManager : StopBGM()");
        if( m_audioSoundBGM.isPlaying == false )
        {
            onNext?.Invoke();
            return;
        }

        if( fadeOut )
        {
            StartCoroutine( FadeOutBGM( onNext, delay ) );
        }
        else
        {
            m_audioSoundBGM.Stop();
        }
    }

    public void SetMuteSound(bool isMute_p)
    {
        if (isMute_p)
        {
            PlayerPrefs.SetInt(SOUND_MUTE, 1);
            StopAllSound();
        }
        else
        {
            PlayerPrefs.SetInt(SOUND_MUTE, 0);
        }
    }
    public static void StopAllSound()
    {
        //for (int i = 0; i < instance.soundSourceList.Count; i++)
        //{
        //    instance.soundSourceList[i].Stop();
        //}
    }
    public IEnumerator FadeOutBGM( System.Action onNext = null, float delay = 0f )
    {
        float time = 0f;
        float timeMax = delay / 1000f;
        float start = bgmVolume;
        float end = 0;

        while( time < timeMax )
        {
            time += Time.deltaTime;
            m_audioSoundBGM.volume = Mathf.Lerp(start, end, time / timeMax);
            yield return null;
        }
        m_audioSoundBGM.volume = 0;
        m_audioSoundBGM.Stop();

        onNext?.Invoke();
    }
    public IEnumerator FadeInBGM( float m_FadeTime )
    {
        float time = 0f;
        float timeMax = m_FadeTime / 1000f;
        float start = 0;
        float end = bgmVolume;

        while (time < timeMax)
        {
            time += Time.deltaTime;
            m_audioSoundBGM.volume = Mathf.Lerp(start, end, time / timeMax);
            yield return null;
        }
        m_audioSoundBGM.volume = bgmVolume;

        if( m_audioSoundBGM.isPlaying == false )
            m_audioSoundBGM.Play();
    }
    public void SetBGMVolume()
    {
        m_audioSoundBGM.volume = bgmVolume;
    }

	public void PlayEffectSound(int sound, float delayTime = 0, bool loop = false)
    {
        PlaySound(sound, delayTime, loop);
    }

	public void StopLoopSoundEffect()
	{
		if (m_audioSoundEffectLoop != null)
		{
			Free(m_audioSoundEffectLoop);
			m_audioSoundEffectLoop.clip = null;
			m_audioSoundEffectLoop = null;
		}
	}

    /// <summary>
    /// 어빌리티에서 플레이하는 사운드. 
    /// Loop를 따로 관리 하기 위해 onFinish 리턴으로 넘긴다.
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="delayTime"></param>
    /// <param name="isLoop"></param>
    /// <param name="onFinish"></param>
    public void PlayAbilitySound(int tSfxID, float delayTime = 0, bool isLoop = false, System.Action<AudioSource> onFinish = null)
    {
        if (effectVolume == 0 || tSfxID == 0)
        {
            return;
        }

        var soundData = DataManager.Instance.m_SoundData.Get(tSfxID);
        if (soundData == null)
        {
            return;
        }

        string fileName = soundData.res;

        AudioClip clip;
        if (m_dicSound.ContainsKey(fileName))
        {
            clip = m_dicSound[fileName];
            Play(clip);
        }
        else
        {
            ResourceManager.Instance.LoadAsync<AudioClip>( fileName, ( audioClip, sucess ) =>
            {
                if( null != audioClip )
                {
                    if (m_dicSound.ContainsKey(fileName) == false)
                    {
                        m_dicSound.Add(fileName, audioClip);
                    }
                    Play(audioClip);
                }
            });
        }

        void Play(AudioClip audioClip)
        {
            var audioSource = GetSource();
            audioSource.gameObject.SetActive(true);
            audioSource.clip = audioClip;
            audioSource.mute = false;
            audioSource.loop = isLoop;
            audioSource.volume = effectVolume;
            audioSource.Play();

            if (isLoop == false)
            {
                StartCoroutine(OnUpdate(audioSource));
            }

            onFinish?.Invoke(audioSource);            
        }
    }

    IEnumerator OnUpdate(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        Free(audioSource);
        //!< End
        onEnd?.Invoke( audioSource.clip );

    }

	/// <summary>
	/// 사운드 출력. Loop가 되는 사운드는 1개만 존재.
	/// </summary>
	/// <param name="sound"></param>
	/// <param name="delayTime"></param>
	/// <param name="loop"></param>
	/// <param name="isVS"></param>
	public void PlaySound( int sound, float delayTime = 0, bool loop = false, bool isVS = false)
    {
        if (effectVolume == 0 || sound == 0 || DataManager.Instance.m_SoundData == null )
        {
            return;
        }

        var soundData = DataManager.Instance.m_SoundData.Get(sound);
        if (soundData == null)
        {
            return;
        }

        string fileName = soundData.res;


        AudioClip clip;
        if (m_dicSound.ContainsKey(fileName))
        {
            clip = m_dicSound[fileName];
            Play(clip, loop);
        }
        else
        {
            ResourceManager.Instance.LoadAsync<AudioClip>(fileName, (audioClip, sucess) =>
            {
                if (null != audioClip)
                {
                    if (m_dicSound.ContainsKey(fileName) == false)
                    {
                        m_dicSound.Add(fileName, audioClip);
                    }
                    Play(audioClip, loop);
                }
            });
        }

        void Play(AudioClip audioClip, bool isloop = false)
        {
            var audioSource = GetSource();
            audioSource.gameObject.SetActive(true);
            audioSource.clip = audioClip;
            audioSource.loop = isloop;
            audioSource.mute = false;
            audioSource.volume = effectVolume;
            audioSource.Play();
            if (isloop)
            {
                if (m_audioSoundEffectLoop != null)
                {
                    Free(m_audioSoundEffectLoop);
                    m_audioSoundEffectLoop = null;
                }

                m_audioSoundEffectLoop = audioSource;
            }
            else StartCoroutine(OnUpdate(audioSource));
        }
    }

    #region SOUND_POOL

    public GameObject m_recSource;

    public MPool<AudioSource> m_pool;

    public void InitPool(Transform parent)
    {
        m_pool = new MPool<AudioSource>(m_recSource.gameObject, parent, 10, 30);
    }

    public AudioSource GetSource()
    {
        return m_pool.Obtain();
    }

    public void Free(AudioSource audio)
    {
        audio.Stop();
        // card.ResetData();
        m_pool.Free(audio);
        audio.gameObject.SetActive(false);
    }

    #endregion
}
