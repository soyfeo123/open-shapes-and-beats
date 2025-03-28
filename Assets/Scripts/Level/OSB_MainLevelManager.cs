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

    private void Update()
    {
        if (levelActive)
        {
            msTime = (float)levelMusic.audioSrc.timeSamples / levelMusic.Clip.frequency * 1000f;

            OSBLevelEditorStaticValues.deltaTime = msTime * 0.001f - lastSongPos;
            lastSongPos = msTime * 0.001f;
        }
        
        onFrame.Invoke();
    }
}

public static class MainGameLevels
{
    public const string I_SAID_MEOW = "level_isaidmeow";
}