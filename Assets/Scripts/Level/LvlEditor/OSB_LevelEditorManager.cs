using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OSB.Editor;
using System.IO;
using SFB;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OSB_LevelEditorManager : MBSingletonDestroy<OSB_LevelEditorManager>
{
    [Header("Shown Text Stuff")]
    public TextMeshProUGUI playheadTimePosition;

    [Header("Topbar Related References")]
    public Image playIcon;
    public Sprite playIconSprite;
    public Sprite pauseIconSprite;

    [Header("Actual Editor Variables")]
    public bool isPlaying;
    public Music levelMusic;


#if UNITY_2017_1_OR_NEWER
    public BigExpandingCircle currentCircle;
#endif

    //public List 

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        ThePlayersParents.Singleton.InitPlayerStuffOnBoot();
        LevelSpawnSprites.LoadSprites();

        Notification.CreateNotification(@"[_<_NOTICE!_>_]

The level editor is still in beta. Stuff [_IS_] going to be broken, and many other features are coming too.
<_Remember to report any bugs to Palo/GameSharp!_>", "[enter] continue", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Return, () => { } } });



        StartCoroutine(LoadSong(Path.Combine(Application.streamingAssetsPath, "songs", "defaultEditorSong.mp3")));
    }

    string ConvertToMinutesAndSeconds(float seconds)
    {
        int _minutes = Mathf.FloorToInt(seconds / 60);
        float _seconds = seconds - (_minutes * 60);
        _seconds = (float)((Mathf.Round(_seconds * 100)) / 100.0);
        return _minutes.ToString() + ":" + (_seconds < 10 ? "0" : "") + _seconds.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        playheadTimePosition.text = ConvertToMinutesAndSeconds(EditorPlayhead.Singleton.SongPosS);

        if (isPlaying)
        {
            EditorPlayhead.Singleton.SongPosMS += Time.deltaTime * 1000f;
            MainLevelManager.Singleton.msTime = EditorPlayhead.Singleton.SongPosMS;
        }




        // TESTING RELATED STUFF
#if UNITY_2017_1_OR_NEWER
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.E))
        {
            FlyingProjectile flyingProjectile = new FlyingProjectile();
            flyingProjectile.Prepare();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ConstantlySpawnFunnyDebris());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCircle = new BigExpandingCircle();
            currentCircle.Prepare();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentCircle.ActivateAttack();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentCircle.Dispose();
        }
#endif
    }

    // Playback Related Stuff

    public void Event_TogglePlayback()
    {
        if(levelMusic == null)
        {
            Notification.CreateNotification("[_<_HEY!_>_]\nYou need to select a song first!", "[enter] got it", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Return, () => { } } });
            return;
        }

        isPlaying = !isPlaying;
        
        levelMusic.TimePosition = EditorPlayhead.Singleton.SongPosS;
        Debug.Log(levelMusic.TimePosition);
        if (isPlaying)
        {
            
            levelMusic.Play(EditorPlayhead.Singleton.SongPosS);
            playIcon.sprite = pauseIconSprite;
            OSBLevelEditorStaticValues.onPlay.Invoke((int)EditorPlayhead.Singleton.SongPosMS);
        }
        else
        {
            OSBLevelEditorStaticValues.onStop.Invoke();
            
            levelMusic.Pause();
            playIcon.sprite = playIconSprite;
        }
        
    }

    public void Event_RewindToStart()
    {
        EditorPlayhead.Singleton.SongPosMS = 0;
    }


    // Player Related Stuff

    public void Event_SpawnThePlayer()
    {
        ThePlayersParents.Singleton.SpawnPlayer(false);
    }

    public void Event_DestroyPlayer()
    {
        ThePlayersParents.Singleton.DestroyPlayer();
    }


    // Music Related Stuff

    public void Event_ChangeSong()
    {
        string filePath = StandaloneFileBrowser.OpenFilePanel("Select a song", "", new ExtensionFilter[] { new("Music Files", "mp3", "wav", "ogg"), new("All Files", "*") }, false)[0];
        StartCoroutine(LoadSong(filePath));
    }

    IEnumerator LoadSong(string filepath)
    {
        AudioType type = AudioType.UNKNOWN;

        switch (new FileInfo(filepath).Extension)
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

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filepath, type);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            if(levelMusic != null)
            {
                levelMusic.Dispose();
            }

            levelMusic = SoundManager.Singleton.CreateMusic(DownloadHandlerAudioClip.GetContent(request));
            Debug.Log("Loaded " + new FileInfo(filepath).Name + "! You didn't screw it up!");
        }
        else
        {
            Debug.LogError("Error while loading audioclip! " + request.result + " | Message: " + request.error);
        }
    }

    // Save Related Stuff

    public void Event_NewLevel()
    {
        Notification.CreateNotification(@"[_<_NEW LEVEL_>_]
All unsaved progress you've done in this level will be lost!
<_You sure?_>", "[esc] no   [enter] yes", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Escape, ()=> { } }, { KeyCode.Return, () => { UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor"); } }  } );
    } 


    // Misc

    public void Event_ShowTestNotif()
    {
        Notification.CreateNotification("[_<_COMING SOON_>_]\n\nWe're hard at work!\n<_(I think)_>", "[enter] ok", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Return, () =>
        {
            Debug.Log("hi");
        } } });
    }

    public void Event_Quit()
    {
        FadeManager.FadeOut(0.5f, () =>
        {
            SceneManager.LoadScene("OSB_Debug");
        });
    }

    IEnumerator ConstantlySpawnFunnyDebris()
    {
        while (true)
        {
            new FlyingProjectile().Prepare();
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public static class OSBLevelEditorStaticValues
{
    public static UnityEngine.Events.UnityEvent onStop = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent<int> onPlay = new UnityEngine.Events.UnityEvent<int>();

    public static bool IsInEditor
    {
        get
        {
            return OSB_LevelEditorManager.HasInstance;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    public static void BandadeBugFix()
    {
        onStop.AddListener(()=> { });
        onPlay.AddListener((thing)=> { Debug.Log("Play Fired"); });
    }
}
