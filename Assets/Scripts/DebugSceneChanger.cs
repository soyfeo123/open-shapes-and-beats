using UnityEngine;

public class DebugSceneChanger
{
    [RuntimeInitializeOnLoadMethod]
    static void init()
    {
        GameObject obj = new GameObject("DEBUGSCENE");
        GameObject.DontDestroyOnLoad(obj);
        LogicEventToKey.Add(obj, KeyCode.F8, () => { UnityEngine.SceneManagement.SceneManager.LoadScene("DebugSelect"); });
    }
}
