using UnityEngine;
using UnityEngine.UI;

public class DebugExitPlayback : MonoBehaviour
{
    Button btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
