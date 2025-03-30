using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;
using OSB.Editor;
using System;

public class MainLevelManager : MBSingleton<MainLevelManager>
{
    public float msTime = 0;

    public bool levelActive = false;
    public string fullLevelPath;
    public string fullMetadataPath;
    public GameObject player1;
    public LvlMetadataV1 levelMetadata;

    public UnityEvent onFrame = new UnityEvent();

    public List<LevelActor> levelActors = new List<LevelActor>();

    public Music levelMusic;

    public LevelMode currentLevelMode = LevelMode.Normal;

    protected override void Awake()
    {
        base.Awake();
        levelMusic = new Music();
        //Debug.Log("level manager is loaded!! congrats man you didn't screw up the code somehow (not surprising if you did later on)");
        onFrame.AddListener(() =>
        {
            // bandade fix to make unity stop annoying me with errors
        });
    }

    public void LoadLevel(string levelName)
    {
        
        levelActors.Clear();
        levelActors = new List<LevelActor>();
        LevelSpawnSprites.LoadSprites();
        levelActive = false;
        msTime = 0;

        fullLevelPath = Path.Combine(Application.streamingAssetsPath, "levelsserialized", levelName + "_lvl.osb");
        fullMetadataPath = Path.Combine(Application.streamingAssetsPath, "levelsserialized", levelName + ".txt");
        levelMetadata = new LvlMetadataV1();
        levelMetadata.LoadFromString(File.ReadAllText(fullMetadataPath));
        levelMetadata.StartLoad();

        Debug.Log(levelMetadata.TrackName + " " + levelMetadata.MiddleLine + " " + levelMetadata.TrackArtist);
        levelMusic.LoadMusic(Path.Combine(Application.streamingAssetsPath, "songs", levelMetadata.SongFileName), ()=> { });

        if(currentLevelMode != LevelMode.ZenMode)
        ThePlayersParents.Singleton.SpawnPlayer();

        StartCoroutine(LevelDelay());
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Vector2.zero, new Vector2(500, 500)), msTime.ToString());
    }

    IEnumerator LevelDelay()
    {
        yield return new WaitForSeconds(2f);

        SongIndicator.Show(levelMetadata.TrackName.ToUpper(), levelMetadata.MiddleLine, levelMetadata.TrackArtist, levelMetadata.LevelAuthor);

        yield return new WaitForSeconds(2.5f);
        string[] levelLines = File.ReadAllLines(fullLevelPath);

        foreach(string line in levelLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string[] lineId = line.Split('>');
            if(lineId[0] == "OBJ")
            {
                string[] splitParam = lineId[1].Split(',');
                if (string.IsNullOrEmpty(splitParam[0]))
                {
                    continue;
                }

                string actorType = splitParam[0].Split(':')[1];
                LevelActor actor = Activator.CreateInstance(Type.GetType(actorType)) as LevelActor;

                foreach(string param in splitParam)
                {
                    if (string.IsNullOrEmpty(param))
                    {
                        continue;
                    }

                    string[] paramSections = param.Split(':');
                    string paramName = paramSections[0];
                    ActorParam value = new ActorParam(paramSections[1], paramSections[1]);

                    actor.objParams[paramName] = value;
                }
                onFrame.AddListener(actor.Frame);
                levelActors.Add(actor);
            }
        }
        

        levelMusic.Play();
        levelActive = true;
    }

    float lastSongPos = 0;
    bool inPauseMenu = false;

    private void Update()
    {
        if (levelActive)
        {
            msTime = (float)levelMusic.audioSrc.timeSamples / levelMusic.Clip.frequency * 1000f;

            OSBLevelEditorStaticValues.deltaTime = msTime * 0.001f - lastSongPos;
            lastSongPos = msTime * 0.001f;
        }
        
        onFrame?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape) && !inPauseMenu)
        {
            inPauseMenu = true;
            Notification.CreateNotification(@"[_<_EXIT LEVEL_>_]
You'll lose any rewards you were going to get.
<_You sure?_>", "[enter] yes   [esc] no", new Dictionary<KeyCode, UnityAction>() { { KeyCode.Return, () =>
                                            {
                                                inPauseMenu = false;
                                                StopLevel();
                                            } }, { KeyCode.Escape, () => inPauseMenu = false } }) ;
        }
    }

    public void StopLevel()
    {
        if (!OSBLevelEditorStaticValues.IsInEditor)
        {
            levelActive = false;

            onFrame.RemoveAllListeners();

            for(int i = 0; i < levelActors.Count; i++)
            {
                LevelActor actor = levelActors[i];
                
                try
                {
                    actor.Dispose();
                    levelActors[i] = null;
                }
                catch(Exception ex) {
                    Debug.LogError(ex.ToString());
                }
            }

            levelActors.Clear();
            levelActors = new List<LevelActor>();

            System.GC.Collect();

            levelMusic.FadeOut(null, 1f);

            FadeManager.FadeOut(1f, () =>
            {
                levelMusic.Stop();
                levelMusic.Dispose();
                
                Utils.Timer(0.25f, () =>
                {
                    ThePlayersParents.Singleton.DestroyPlayer();
                    FadeManager.FadeIn(0.5f);
                    UIController.OpenMenu(UIMenus.MAIN_MENU);
                });
            });
        }
    }
}

public static class MainGameLevels
{
    public const string I_SAID_MEOW = "level_isaidmeow";
}

public enum LevelMode
{
    Normal, ZenMode, HardcoreMode
}