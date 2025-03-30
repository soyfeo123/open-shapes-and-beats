using TMPro;
using UnityEngine;

public class ExportWindowController : MonoBehaviour
{
    public TMP_InputField songName;
    public TMP_InputField middleLine;
    public TMP_InputField songArtist;
    public TMP_InputField levelAuthor;

    public void Export()
    {
        OSB_LevelEditorManager.Singleton.ExportLevel(songName.text, middleLine.text, songArtist.text, levelAuthor.text);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
