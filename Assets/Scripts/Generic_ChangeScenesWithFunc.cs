using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generic_ChangeScenesWithFunc : MonoBehaviour
{
    public void Event_ChangeScene(string name)
    {
        OSBScenes.LoadGameplayScene();
    }
}
