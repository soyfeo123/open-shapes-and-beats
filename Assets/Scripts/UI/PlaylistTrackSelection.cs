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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultpos - 20f, GetComponent<RectTransform>().anchoredPosition.y);
        if(!MainMenuManager.Singleton.noMoreSongsPlease)
        MainMenuManager.Singleton.menuMusic.LoadMusic(Path.Combine(Application.streamingAssetsPath, "songs", metadata.SongFileName), ()=> { MainMenuManager.Singleton.menuMusic.Play(0, 65);
            
            GetComponent<RectTransform>().DOAnchorPosX(defaultpos, 1f).SetEase(Ease.OutExpo);

            MainMenuManager.Singleton.menuMusic.FadeIn(null, 1f);
        });

        
    }

    public void OnPointerClick(PointerEventData data)
    {
        Camera.main.backgroundColor = Color.black;

        MainMenuManager.Singleton.RemoveBackground();
        MainMenuManager.Singleton.menuMusic.FadeOut(()=> { }, 0.25f);
        UIController.FadeOut(0.25f, ()=>
        {
            MainLevelManager.Singleton.LoadLevel(levelFileName);
        });
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_BIGSUBMIT);
    }
}
