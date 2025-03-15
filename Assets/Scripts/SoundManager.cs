using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

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

    [RuntimeInitializeOnLoadMethod]
    static void Load()
    {
        UI_SUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SUBMIT_2");
        UI_SELECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_SELECT");
        UI_REJECT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_REJECT");
        UI_BIGSUBMIT = (AudioClip)Resources.Load("Sound/SFX/UI/SFX_UI_BIGSUBMIT");
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
