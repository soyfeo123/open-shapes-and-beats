using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class OSBScenes
{
    public static void LoadGameplayScene()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("DebugSelect");
#endif
#if !UNITY_EDITOR
        SceneManager.LoadScene("OSB");
#endif
    }
    public static void QuitGame()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("DebugSelect");
#endif
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
