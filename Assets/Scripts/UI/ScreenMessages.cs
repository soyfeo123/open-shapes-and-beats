using TMPro;
using UnityEngine;

public static class ScreenMessages
{
    public static ScreenMessage Create(string body)
    {
        GameObject message = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ScreenMessage"));
        message.transform.Find("Image/Body").GetComponent<TextMeshProUGUI>().text = body;

        return new ScreenMessage(message);
    }
}

public class ScreenMessage
{
    GameObject _screen;
    public ScreenMessage(GameObject screen)
    {
        _screen = screen;
    }

    public void RemoveFromScreen()
    {
        GameObject.Destroy(_screen);
    }
}