using UnityEngine;
using UnityEngine.Events;

public class LogicEventToKey : MonoBehaviour
{
    public KeyCode key;
    public UnityAction action;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            action?.Invoke();
        }
    }

    public static void Add(GameObject obj, KeyCode keybind, UnityAction action)
    {
        LogicEventToKey letk = obj.AddComponent<LogicEventToKey>();

        letk.key = keybind;
        letk.action = action;
       
    }
}
