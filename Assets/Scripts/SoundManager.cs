using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void PlaySound(AudioClip clip)
    {
        if(sfxSource == null)
        {
            GameObject go = new GameObject("SFX-" + Random.Range(271, 953));
            sfxSource = go.AddComponent<AudioSource>();

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
    AudioSource audioSrc;

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

    public Music()
    {
        GameObject go = new GameObject("MUSIC-" + Random.Range(574,1372));
        audioSrc = go.AddComponent<AudioSource>();
    }

    public void Dispose()
    {
        Object.Destroy(audioSrc.gameObject);
    }

    public void Play(float time = 0)
    {
        float pos = TimePosition;
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
}
