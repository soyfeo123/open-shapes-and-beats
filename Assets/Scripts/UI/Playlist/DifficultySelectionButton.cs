using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultySelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI text;
    public Image buttonBackground;
    public GameObject outline;
    public UnityEvent onClick;

    Color foreColor;
    Color backColor;

    Vector2 position;

    void Start()
    {
        position = GetComponent<RectTransform>().anchoredPosition;

        foreColor = text.color;
        backColor = buttonBackground.color;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<RectTransform>().DOKill();

        buttonBackground.color = foreColor;
        text.color = backColor;
        outline.SetActive(true);
        outline.GetComponent<Image>().color = Color.white;
        GetComponent<RectTransform>().anchoredPosition = position + new Vector2(0f, 10f);
        GetComponent<RectTransform>().DOAnchorPos(position, 0.5f).SetEase(Ease.OutSine);
    }

    public void OnPointerExit(PointerEventData data)
    {
        buttonBackground.DOKill();

        buttonBackground.color = backColor;
        text.color = foreColor;
        outline.GetComponent<Image>().DOFade(0f, 0.2f);
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;
        GetComponent<RectTransform>().DOKill();
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 20);
        GetComponent<RectTransform>().DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        GetComponent<RectTransform>().localScale = Vector3.one * 1.2f;
        GetComponent<RectTransform>().DOScale(1f, 0.5f).SetEase(Ease.OutExpo);

        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SUBMIT);

        buttonBackground.DOKill();
        buttonBackground.color = Color.white;
        buttonBackground.DOColor(foreColor, 0.5f);

        float filler = 5f;

        DOTween.To(() => filler, x => filler = x, 0f, 0.65f).OnComplete(() =>
        {
            onClick?.Invoke();
        });
    }
}
