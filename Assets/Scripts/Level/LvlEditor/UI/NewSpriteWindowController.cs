using TMPro;
using UnityEngine;

public class NewSpriteWindowController : MonoBehaviour
{
    public string spritePath;
    public TMP_InputField nameField;
    public SpriteManagerWindowController mainWindow;

    public void Event_AddSpriteButton()
    {
        MainLevelManager.Singleton.LoadSprite(spritePath, nameField.text);

        mainWindow.UpdateList();
        Destroy(gameObject);
    }
}
