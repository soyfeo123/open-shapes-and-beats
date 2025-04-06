using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.IO;
using TMPro;

public class PlaylistTrackSelection : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    float defaultpos;
    public LvlMetadataV1 metadata;
    public string levelFileName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultpos = 0;
        Debug.Log(defaultpos);

        transform.Find("TrackName").GetComponent<TextMeshProUGUI>().text = metadata.TrackName;
        transform.Find("Artist").GetComponent<TextMeshProUGUI>().text = metadata.TrackArtist;
        transform.Find("LevelAuthor").GetComponent<TextMeshProUGUI>().text = metadata.LevelAuthor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultpos - 20f, GetComponent<RectTransform>().anchoredPosition.y);
        MainMenuManager.Singleton.songName.text = metadata.TrackName;
        MainMenuManager.Singleton.songArtist.text = metadata.MiddleLine + "\n" + metadata.TrackArtist;
        if(!MainMenuManager.Singleton.noMoreSongsPlease)
        MainMenuManager.Singleton.menuMusic.LoadMusic(Path.Combine(Application.streamingAssetsPath, "songs", metadata.SongFileName), ()=> { MainMenuManager.Singleton.menuMusic.Play(0, 65);
            
            GetComponent<RectTransform>().DOAnchorPosX(defaultpos, 1f).SetEase(Ease.OutExpo);

            MainMenuManager.Singleton.menuMusic.FadeIn(null, 1f);
        });

        
    }

    public void OnPointerClick(PointerEventData data)
    {
        DifficultySelection.Open(levelFileName);
    }
}
