using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

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

    [RuntimeInitializeOnLoadMethod]
    static void Load()
    {
        UI_SUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SUBMIT_2");
        UI_SELECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SELECT");
        UI_REJECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_REJECT");
        UI_BIGSUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_BIGSUBMIT");

        UI_PANEL_OPEN = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_PANEL_OPEN");
        UI_PANEL_CLOSE = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_PANEL_CLOSE");
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
        Stop();
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
        request.SendWebRequest().completed += delegate
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                Clip = clip;
                Debug.Log("Loaded " + new FileInfo(filePath).Name + "! Wow! Look!");

                onComplete.Invoke();
            }
            else
            {
                Debug.LogError("ERROR whlie loading song! Result: " + request.result + ", message: " + request.error);
            }
        };
    }

    public void Dispose()
    {
        Object.Destroy(audioSrc.gameObject);
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
        audioSrc.Stop();
        TimePosition = 0;
    }

    public void FadeOut(UnityAction onComplete, float time = 1f)
    {
        audioSrc.DOFade(0, time).OnComplete(()=> { onComplete.Invoke(); });
    }
}
