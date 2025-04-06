using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using System.Threading;

public class SoundManager : MBSingleton<SoundManager>
{
    /// <summary>
    /// only recommened for areas where SYNC is not necessary as it loads dynamically
    /// </summary>
    /// <param name="resourcePath"></param>
    ///

    AudioSource sfxSource;

    public void PlaySound(string resourcePath)
    {
        PlaySound(Resources.Load(resourcePath) as AudioClip);
    }

    public void PlaySound(AudioClip clip, float volume = 0.75f)
    {
        if(sfxSource == null)
        {
            GameObject go = new GameObject(Utils.GenerateUniqueName("SFX"));
            sfxSource = go.AddComponent<AudioSource>();
            sfxSource.volume = volume;
        }
        sfxSource.PlayOneShot(clip);
    }

    public Music CreateMusic(AudioClip clip)
    {
        Music music = new Music();
        music.Clip = clip;
        return music;
    }

    private void Update()
    {
        
    }
}

/// <summary>
/// Sounds that must be loaded throughout the entire game.
/// </summary>
public static class LoadedSFXEnum
{
    public static AudioClip UI_SUBMIT;
    public static AudioClip UI_SELECT;
    public static AudioClip UI_REJECT;
    public static AudioClip UI_BIGSUBMIT;
    public static AudioClip UI_PANEL_OPEN;
    public static AudioClip UI_PANEL_CLOSE;

    public static AudioClip EXPLOSION;
    public static AudioClip HIT
    {
        get
        {
            return (AudioClip)Resources.Load("Sound/SFX/Gameplay/SFX_HIT_" + Mathf.Clamp(Random.Range(1, 3), 1, 2));
        }
    }
    public static AudioClip CHECKPOINT;

    [RuntimeInitializeOnLoadMethod]
    static void Load()
    {
        UI_SUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SUBMIT_2");
        UI_SELECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SELECT");
        UI_REJECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_REJECT");
        UI_BIGSUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_BIGSUBMIT");

        UI_PANEL_OPEN = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_PANEL_OPEN");
        UI_PANEL_CLOSE = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_PANEL_CLOSE");

        EXPLOSION = (AudioClip)Resources.Load("Sound/SFX/Gameplay/SFX_DIENOW");
        CHECKPOINT = (AudioClip)Resources.Load("Sound/SFX/Gameplay/SFX_CHECKPOINT");
    }
}

public class Music
{
    public AudioClip Clip
    {
        get
        {
            return audioSrc.clip;
        }
        set
        {
            audioSrc.clip = value;
        }
    }
    public AudioSource audioSrc;

    public float TimePosition
    {
        get
        {
            return audioSrc.time;
        }
        set
        {
            audioSrc.time = value;
        }
    }

    public string musicPath;

    /// <summary>
    /// 0-100
    /// </summary>
    public float Volume
    {
        get
        {
            return audioSrc.volume * 100f;
        }
        set
        {
            audioSrc.volume = value * 0.01f;
        }
    }

    public Music()
    {
        GameObject go = new GameObject(Utils.GenerateUniqueName("MUSIC"));
        audioSrc = go.AddComponent<AudioSource>();
    }

    public void LoadMusic(string filePath, System.Action onComplete)
    {
        if(audioSrc == null)
        {
            GameObject go = new GameObject(Utils.GenerateUniqueName("MUSIC"));
            audioSrc = go.AddComponent<AudioSource>();
        }

        Stop();
        Volume = 100;
        AudioType type = AudioType.UNKNOWN;

        switch (new FileInfo(filePath).Extension)
        {
            case ".wav":
                type = AudioType.WAV;
                break;
            case ".mp3":
                type = AudioType.MPEG;
                break;
            case ".ogg":
                type = AudioType.OGGVORBIS;
                break;
            default:
                type = AudioType.MPEG;
                break;
        }
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, type);
        //request.SetRequestHeader("Range", "bytes=0-10000");
        Debug.Log("rq" + request);

        ((DownloadHandlerAudioClip)request.downloadHandler).streamAudio = true;

        var req = request.SendWebRequest();
        req.completed += delegate
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                clip.LoadAudioData();
                Clip = clip;
                onComplete?.Invoke();

                musicPath = filePath;
            }
            else
            {
                Notification.CreateNotification("[_<_ERROR WHILE LOADING AUDIO_>_]\nPlease contact Palo for more info.", "[enter] ok", new Dictionary<KeyCode, UnityAction>() { { KeyCode.Return, ()=> { } } });
            }
        };
    }

    public void Dispose()
    {
        if(audioSrc != null)
        Object.Destroy(audioSrc.gameObject);
        audioSrc = null;
    }

    public void Play(float time = 0, float vol = 100)
    {
        float pos = TimePosition;
        Volume = vol;
        audioSrc.time = pos;
        audioSrc.Play();
        audioSrc.time = time;
    }

    public void Pause()
    {
        float pos = TimePosition;
        audioSrc.Stop();
        audioSrc.time = pos;
    }

    public void Stop()
    {
        if (audioSrc == null)
            return;

        audioSrc.Stop();
        TimePosition = 0;
    }

    public void FadeOut(UnityAction onComplete, float time = 1f)
    {
        audioSrc.DOKill();
        audioSrc.DOFade(0, time).OnComplete(()=> { onComplete?.Invoke(); });
    }

    public void FadeIn(UnityAction onComplete, float time = 1f)
    {
        float vol = audioSrc.volume;
        
        audioSrc.DOKill();
        audioSrc.volume = 0;
        audioSrc.DOFade(vol, time).SetEase(Ease.Linear).OnComplete(() => { onComplete?.Invoke(); });
    }
}
