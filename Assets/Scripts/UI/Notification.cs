using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Notification : MonoBehaviour
{
    public Dictionary<KeyCode, UnityAction> keybinds;
    public string text;
    public TextMeshProUGUI mainBodyRef;
    public string keybindText;
    public TextMeshProUGUI keybindRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainBodyRef.text = text;
        keybindRef.text = keybindText;
        foreach(KeyValuePair<KeyCode, UnityAction> key in keybinds)
        {
            if (Input.GetKeyDown(key.Key))
            {
                key.Value.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public static void CreateNotification(string text, string keybindText, Dictionary<KeyCode, UnityAction> keybinds)
    {
        GameObject thing = Instantiate((GameObject)Resources.Load("Prefabs/Notification"));
        Notification notification = thing.GetComponent<Notification>();
        string newText = text.Replace("<_", "<color=#00FFF4>").Replace("_>", "</color>").Replace("[_", "<b>").Replace("_]", "</b>");
        notification.text = newText;
        notification.keybindText = keybindText;
        notification.keybinds = keybinds;
        DontDestroyOnLoad(thing);
    }
}
