using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;

public class MainLevelManager : MBSingleton<MainLevelManager>
{
    public float msTime = 0;

    public bool levelActive = false;
    public AssetBundle currentLevelSceneBundle;
    public AssetBundle currentLevelBundle;
    public SongMetadata metadata;
    public GameObject player1;

    public UnityEvent onFrame = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("level manager is loaded!! congrats man you didn't screw up the code somehow (not surprising if you did later on)");
        onFrame.AddListener(() =>
        {
            // bandade fix to make unity stop annoying me with errors
        });
    }

    public void LoadLevel(string levelName)
    {
        msTime = 0;
        currentLevelSceneBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, @"StreamingAssets\levels\", levelName + "_scene"));
        currentLevelBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, @"StreamingAssets\levels\", levelName));
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);

        metadata = currentLevelBundle.LoadAsset("SongMetadata") as SongMetadata;
        Debug.Log(metadata.SongTitle + " " + metadata.MiddleLine + " " + metadata.SongArtist);

        ThePlayersParents.Singleton.SpawnPlayer();

        StartCoroutine(LevelDelay());
    }

    IEnumerator LevelDelay()
    {
        msTime = -500;
        yield return new WaitForSeconds(2f);
        msTime = 0;
        levelActive = true;
    }

    private void Update()
    {
        if (levelActive)
        {
            msTime += Time.deltaTime * 1000;
        }
        
        onFrame.Invoke();
    }
}

public static class MainGameLevels
{
    public const string I_SAID_MEOW = "level_isaidmeow";
}