using System.Collections.Generic;
using System.Linq;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManagerWindowController : MonoBehaviour
{
    public GameObject spriteEntry;
    public Transform mainContainer;
    public GameObject newSpriteWindow;

    private void Start()
    {
        UpdateList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateList();
        }
    }

    public void Event_NewSprite()
    {
        string file = StandaloneFileBrowser.OpenFilePanel("Select an image", "", new ExtensionFilter[] { new("Image File", "png", "jpg", "jpeg") }, false)[0];

        NewSpriteWindowController window =  Instantiate(newSpriteWindow, transform.parent).GetComponent<NewSpriteWindowController>();

        window.spritePath = file;
        window.mainWindow = this;
    }

    public void Event_Close()
    {
        Destroy(gameObject);
    }

    public void UpdateList()
    {
        foreach(Transform child in mainContainer)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < MainLevelManager.Singleton.imageResources.Count; i++)
        {
            KeyValuePair<string, ImageAsset> asset = MainLevelManager.Singleton.imageResources.ElementAt(i);

            GameObject entry = Instantiate(spriteEntry);
            entry.transform.SetParent(mainContainer);
            entry.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = asset.Key;

            entry.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() =>
            {
                MainLevelManager.Singleton.imageResources.Remove(asset.Key);
                UpdateList();
            });
        }
    }
}
