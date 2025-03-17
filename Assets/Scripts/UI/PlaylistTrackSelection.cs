using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlaylistTrackSelection : MonoBehaviour, IPointerEnterHandler
{
    float defaultpos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultpos = GetComponent<RectTransform>().anchoredPosition.x;
        Debug.Log(defaultpos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultpos - 20f, GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<RectTransform>().DOAnchorPosX(defaultpos, 1f).SetEase(Ease.OutExpo);
    }
}
