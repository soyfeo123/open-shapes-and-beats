using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugSceneOnButton : MonoBehaviour
{
    Button btn;
    public string Scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Scene);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
