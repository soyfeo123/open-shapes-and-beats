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
using System.Text;

public class OSB_LevelEditorManager : MBSingletonDestroy<OSB_LevelEditorManager>
{
    [Header("Shown Text Stuff")]
    public TextMeshProUGUI playheadTimePosition;
    public GameObject tooltip;

    [Header("Topbar Related References")]
    public Image playIcon;
    public Sprite playIconSprite;
    public Sprite pauseIconSprite;

    [Header("Window Prefabs")]
    public GameObject spriteManager;

    [Header("Actual Editor Variables")]
    public bool isPlaying;
    public bool isRecording;
    public Music levelMusic;
    public List<OSBLayer> layers =new List<OSBLayer>();
    public Transform layerViewContent;
    public Transform newLayerButton;
    public string musicFilePath;

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

        levelMusic = new Music();

        musicFilePath = Path.Combine(Application.streamingAssetsPath, "songs", "defaultEditorSong.mp3");
        levelMusic.LoadMusic(musicFilePath, () => { });
    }

    string ConvertToMinutesAndSeconds(float seconds)
    {
        int _minutes = Mathf.FloorToInt(seconds / 60);
        float _seconds = seconds - (_minutes * 60);
        _seconds = (float)((Mathf.Round(_seconds * 100)) / 100.0);
        return _minutes.ToString() + ":" + (_seconds < 10 ? "0" : "") + _seconds.ToString();
    }

    float lastSongPos;

    public static bool IsPointerFocusedInputField()
    {

        var selectables = Selectable.allSelectablesArray;
        foreach (var selectable in selectables)
        {
            
            TMP_InputField inputField = selectable as TMP_InputField;
            if (inputField != null && inputField.isFocused)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        playheadTimePosition.text = ConvertToMinutesAndSeconds(EditorPlayhead.Singleton.SongPosS);

        if (isPlaying)
        {
            EditorPlayhead.Singleton.SongPosMS = (float)levelMusic.audioSrc.timeSamples / levelMusic.Clip.frequency * 1000f;
            MainLevelManager.Singleton.msTime = EditorPlayhead.Singleton.SongPosMS;
        }

        OSBLevelEditorStaticValues.deltaTime = EditorPlayhead.Singleton.SongPosS - lastSongPos;
        lastSongPos = EditorPlayhead.Singleton.SongPosS;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out pos);
        tooltip.GetComponent<RectTransform>().anchoredPosition = pos + new Vector2(10, -10);

        // TESTING RELATED STUFF
#if UNITY_2017_1_OR_NEWER
        bool pointer = IsPointerFocusedInputField();


        if(!pointer && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(layers[0].Save());
        }


#endif
    }

    public void ShowTooltip()
    {
        tooltip.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void HideTooltip()
    {
        tooltip.GetComponent<CanvasGroup>().alpha = 0f;
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
            isRecording = false;
            levelMusic.Pause();
            playIcon.sprite = playIconSprite;
        }
        
    }

    public void Event_Record()
    {
        isRecording = true;
        Event_TogglePlayback();
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


    // Layer Related Stuff

    public void Event_AddLayer()
    {
        GameObject layer = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/UI/TimelineLayer"));
        layer.transform.SetParent(layerViewContent);
        layer.name = "Layer" + layers.Count.ToString();
        newLayerButton.SetAsLastSibling();
        layers.Add(layer.GetComponent<OSBLayer>());
    }


    // Music Related Stuff

    public void Event_ChangeSong()
    {
        musicFilePath = StandaloneFileBrowser.OpenFilePanel("Select a song", "", new ExtensionFilter[] { new("Music Files", "mp3", "wav", "ogg"), new("All Files", "*") }, false)[0];
        levelMusic.LoadMusic(musicFilePath, () => { });
    }

    

    // Save Related Stuff

    public void Event_NewLevel()
    {
        Notification.CreateNotification(@"[_<_NEW LEVEL_>_]
All unsaved progress you've done in this level will be lost!
<_You sure?_>", "[esc] no   [enter] yes", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Escape, ()=> { } }, { KeyCode.Return, () => { UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor"); } }  } );
    } 

    public void Event_SaveLevel()
    {
        string filePath = StandaloneFileBrowser.SaveFilePanel("Save the level", "", "level.osb", "osb");

        

        File.WriteAllText(filePath, SaveLevel());
    }

    public string SaveLevel()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(musicFilePath);

        builder.AppendLine("SPRITE");
        foreach(KeyValuePair<string, ImageAsset> sprite in MainLevelManager.Singleton.imageResources)
        {
            builder.AppendLine($"SPR>{sprite.Key};{sprite.Value.path}");
        }
        builder.AppendLine("ENDSPRITE");
        foreach (OSBLayer layer in layers)
        {
            builder.Append(layer.Save());
        }
        return builder.ToString();
    }

    public string[] SaveLevel(bool array)
    {
        return SaveLevel().Split(System.Environment.NewLine.ToCharArray());
    }

    public void Event_LoadLevel()
    {
        foreach(OSBLayer layer in layers)
        {
            Destroy(layer.gameObject);
        }
        layers.Clear();

        string filePath = StandaloneFileBrowser.OpenFilePanel("Open a level", "", new ExtensionFilter[] { new("OSB Level", "osb") }, false)[0];
        string[] file = File.ReadAllLines(filePath);

        Debug.Log("Music: " + file[0]);
        levelMusic.LoadMusic(file[0], ()=> { });

        StringBuilder currentLayer = new StringBuilder();
        StringBuilder currentSpriteGroup = new StringBuilder();
        foreach(string line in file)
        {
            string[] splitFromLineID = line.Split('>');

            if (line == "SPRITE")
            {
                currentSpriteGroup = new StringBuilder();
                Debug.Log("New sprite group!");
            }

            if (line == "LAYER>a")
            {
                currentLayer = new StringBuilder();
                Debug.Log("New layer!");
            }
            else if(line == "END")
            {
                GameObject layer = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/UI/TimelineLayer"));
                layer.transform.SetParent(layerViewContent);
                layer.name = "Layer" + layers.Count.ToString();
                layer.GetComponent<OSBLayer>().Load(currentLayer.ToString().Split(System.Environment.NewLine));
                newLayerButton.SetAsLastSibling();
                layers.Add(layer.GetComponent<OSBLayer>());
            }
            else if(splitFromLineID[0] == "OBJ")
            {
                
                
                currentLayer.AppendLine(splitFromLineID[1]);
            }
            else if(splitFromLineID[0] == "SPR")
            {
                currentSpriteGroup.AppendLine(splitFromLineID[1]);
            }
            else if(line == "ENDSPRITE")
            {
                Debug.Log("Finished sprite group!");

                foreach(string sprite in currentSpriteGroup.ToString().Split(System.Environment.NewLine.ToCharArray()))
                {
                    if (string.IsNullOrEmpty(sprite))
                    {
                        continue;
                    }

                    Debug.Log(sprite);
                    string[] split = sprite.Split(';');
                    MainLevelManager.Singleton.LoadSprite(split[1], split[0]);
                }
            }
            
            
        }
    }

    public void Event_OpenSpriteManager()
    {
        Instantiate(spriteManager, transform);
    }

    public void Event_OpenExportWindow()
    {
        GameObject win = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/ExportWindow"));
        win.transform.SetParent(transform, false);
    }

    public void ExportLevel(string title, string middleLine, string artist, string author)
    {
        string filePath = StandaloneFileBrowser.SaveFilePanel("Export...", "", "Level.obz", "obz");

        
        string[] lines = SaveLevel(true);

        ObzFormat file = new ObzFormat(lines, title, artist, middleLine, author, levelMusic.musicPath);

        file.Export(filePath);
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
            OSBScenes.LoadGameplayScene();
        });
    }

    private void OnDestroy()
    {
        ThePlayersParents.Singleton.DestroyPlayer();
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
    public static float deltaTime;

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

public class LevelSave
{
    public class SaveObject
    {

    }
    
    public string trackPath;
    public List<List<SaveObject>> layers = new List<List<SaveObject>>();
}