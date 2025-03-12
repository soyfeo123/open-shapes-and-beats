using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*TO WHOEVER'S READING THIS
 * THIS IS CANON THAT LITTLE CUBE HAS PARENTS!!!*/

/*I HONESTLY FORGOT THIS WAS OPEN SOURCE SOOO*/

// im profesinoal

public class ThePlayersParents : MBSingleton<ThePlayersParents>
{
    GameObject playerPrefab;
    GameObject boundaryPrefab;
    
    public OSB_Player PlayerOnScreen;

    public void InitPlayerStuffOnBoot()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/LevelStuff/Player1");
        boundaryPrefab = (GameObject)Resources.Load("Prefabs/LevelStuff/CollisionBoundaries");

        
    }

    public OSB_Player SpawnPlayer(bool ignorePrevPlayers = false)
    {
        if(PlayerOnScreen != null && !ignorePrevPlayers)
        {
            Debug.LogError("Player still spawned! How dare you attempt to spawn another one!?");
            return null;
        }
        else if (ignorePrevPlayers)
        {
            Debug.LogWarning("There's gonna be multiple players at once... have the emergency button on standby");
        }

        GameObject obj = Instantiate(playerPrefab);
        DontDestroyOnLoad(obj);
        PlayerOnScreen = obj.GetComponent<OSB_Player>();

        GameObject boundary = Camera.main.transform.Find("CollisionBoundaries") == null ? Instantiate(boundaryPrefab) : Camera.main.transform.Find("CollisionBoundaries(Clone)").gameObject;
        boundary.transform.parent = Camera.main.transform;
        boundary.transform.localPosition = Vector3.zero;

        

        return PlayerOnScreen;
    }

    public void DestroyPlayer()
    {
        if(PlayerOnScreen == null)
        {
            Debug.LogError("Woah woah woah there's no player here!");
            return;
        }
        Debug.Log("Eyes locked on the suspect, fire when ready");
        Destroy(PlayerOnScreen.gameObject);
        Debug.Log("He's dead now");
    }
}
