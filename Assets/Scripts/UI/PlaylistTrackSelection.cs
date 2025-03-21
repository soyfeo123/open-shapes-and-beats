using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.IO;
using TMPro;

public class PlaylistTrackSelection : MonoBehaviour, IPointerEnterHandler
{
    float defaultpos;
    public LvlMetadataV1 metadata;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultpos = 0;
        Debug.Log(defaultpos);

        transform.Find("TrackName").GetComponent<TextMeshProUGUI>().text = metadata.TrackName;
        transform.Find("Artist").GetComponent<TextMeshProUGUI>().text = metadata.TrackArtist;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultpos - 20f, GetComponent<RectTransform>().anchoredPosition.y);
        MainMenuManager.Singleton.menuMusic.LoadMusic(Path.Combine(Application.streamingAssetsPath, "songs", metadata.SongFileName), ()=> { MainMenuManager.Singleton.menuMusic.Play(0, 65);
            
            GetComponent<RectTransform>().DOAnchorPosX(defaultpos, 1f).SetEase(Ease.OutExpo);

            MainMenuManager.Singleton.menuMusic.FadeIn(null, 1f);
        });

        
    }
}
