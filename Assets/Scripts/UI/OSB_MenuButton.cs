using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class OSB_MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onClick;

    RectTransform currentTransform;
    float previousWidth;

    public bool haveDelay = true;
    public bool changeScale = true;
    public bool playSubmitSound = true;

    GameObject leftTriangle;
    GameObject rightTriangle;
    GameObject mainBg;
    Color color1;
    Color color2;
    TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        currentTransform = GetComponent<RectTransform>();
        previousWidth = currentTransform.sizeDelta.x;

        leftTriangle = transform.Find("LeftTriangleTHing").gameObject;
        rightTriangle = transform.Find("RightTriangleThing").gameObject;
        mainBg = transform.Find("MainBg").gameObject;
        buttonText = mainBg.transform.GetComponentInChildren<TextMeshProUGUI>();

        color1 = mainBg.GetComponent<Image>().color;
        color2 = buttonText.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        currentTransform.DOKill();
        currentTransform.DOSizeDelta(new Vector2(currentTransform.sizeDelta.x * 1.5f, currentTransform.sizeDelta.y), 0.3f).SetEase(Ease.OutExpo);
        Vector3 pos = currentTransform.position;
        pos.z = 10;
        currentTransform.position = pos;

        leftTriangle.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);
        mainBg.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);
        rightTriangle.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);
        buttonText.DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SELECT);
        //currentTransform.DOScale(1.1f, 0.5f).SetEase(Ease.OutExpo);
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (changeScale)
        {
            currentTransform.localScale = Vector3.one;
        }
        currentTransform.DOKill();
        currentTransform.DOSizeDelta(new Vector2(previousWidth, currentTransform.sizeDelta.y), 0.3f).SetEase(Ease.OutExpo);
        Vector3 pos = currentTransform.position;
        pos.z = 0;
        currentTransform.position = pos;

        leftTriangle.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        mainBg.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        rightTriangle.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        buttonText.DOColor(color2, 0.5f).SetEase(Ease.OutExpo);

        

        //currentTransform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
    }

    public void OnPointerClick(PointerEventData data)
    {
        currentTransform.DOKill();
        Vector2 size = currentTransform.sizeDelta;
        size.x = previousWidth * 1.6f;
        currentTransform.sizeDelta = size;
        currentTransform.DOSizeDelta(new Vector2(previousWidth * 1.5f, currentTransform.sizeDelta.y), 0.5f).SetEase(Ease.OutExpo);

        if (changeScale)
        {
            currentTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            currentTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        }
        if(playSubmitSound)
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SUBMIT);

        StartCoroutine(ClickDelay());
    }

    IEnumerator ClickDelay()
    {
        if(haveDelay)
        yield return new WaitForSeconds(0.5f);
        onClick.Invoke();
    }
}
