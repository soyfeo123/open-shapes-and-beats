using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;

public class OSB_MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onClick;

    RectTransform currentTransform;
    float previousWidth;

    public bool haveDelay = true;
    public bool changeScale = true;
    public bool playSubmitSound = true;
    public bool playPlaySelectSound = false;

    GameObject leftTriangle;
    GameObject rightTriangle;
    GameObject mainBg;
    Color color1;
    Color color2;
    TextMeshProUGUI[] buttonText;

    private Vector3 m_defaultScale;

    // Start is called before the first frame update
    void Awake()
    {
        currentTransform = GetComponent<RectTransform>();
        previousWidth = currentTransform.sizeDelta.x;

        leftTriangle = transform.Find("LeftTriangleTHing").gameObject;
        rightTriangle = transform.Find("RightTriangleThing").gameObject;
        mainBg = transform.Find("MainBg").gameObject;
        buttonText = mainBg.transform.GetComponentsInChildren<TextMeshProUGUI>();

        color1 = mainBg.GetComponent<Image>().color;
        color2 = buttonText[0].color;

        m_defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnEnable()
    //{
    //    transform.DOKill();
    //    transform.localScale = m_defaultScale;
    //}

    void jsForEach<T>(T[] array, Action<T> each)
    {
        foreach(var thing in array)
        {
            each?.Invoke(thing);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        currentTransform.DOKill();
        currentTransform.DOSizeDelta(new Vector2(currentTransform.sizeDelta.x * 1.5f, currentTransform.sizeDelta.y), 0.15f).SetEase(Ease.OutBack);
        Vector3 pos = currentTransform.position;
        pos.z = 10;
        currentTransform.position = pos;

        //leftTriangle.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);
        //mainBg.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);
        //rightTriangle.GetComponent<Image>().DOColor(color2, 0.5f).SetEase(Ease.OutExpo);

        //jsForEach(buttonText, (button) => button.DOColor(color1, 0.5f).SetEase(Ease.OutExpo));

        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SELECT);
        //currentTransform.DOScale(1.1f, 0.5f).SetEase(Ease.OutExpo);
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (changeScale)
        {
            transform.localScale = m_defaultScale;
        }

        currentTransform.DOKill();
        currentTransform.DOSizeDelta(new Vector2(previousWidth, currentTransform.sizeDelta.y), 0.15f).SetEase(Ease.OutBack);

        //Vector3 pos = currentTransform.position;
        //pos.z = 0;
        //currentTransform.position = pos;

        //leftTriangle.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        //mainBg.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);
        //rightTriangle.GetComponent<Image>().DOColor(color1, 0.5f).SetEase(Ease.OutExpo);

        //jsForEach(buttonText, (button) => button.DOColor(color2, 0.5f).SetEase(Ease.OutExpo));



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
            Debug.Log(m_defaultScale);

            transform.localScale = m_defaultScale * 1.3f;
            transform.DOScale(m_defaultScale, 0.5f).SetEase(Ease.OutExpo);
        }
        if(playSubmitSound)
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SUBMIT);
        if (playPlaySelectSound)
            SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_BIGSUBMIT);

        leftTriangle.GetComponent<Image>().color = Color.white;
        mainBg.GetComponent<Image>().color = Color.white;
        rightTriangle.GetComponent<Image>().color = Color.white;

        leftTriangle.GetComponent<Image>().DOColor(color1, 0.2f).SetEase(Ease.InExpo);
        mainBg.GetComponent<Image>().DOColor(color1, 0.2f).SetEase(Ease.InExpo);
        rightTriangle.GetComponent<Image>().DOColor(color1, 0.2f).SetEase(Ease.InExpo);

        StartCoroutine(ClickDelay());
    }

    IEnumerator ClickDelay()
    {
        if(haveDelay)
        yield return new WaitForSeconds(0.5f);
        onClick.Invoke();
    }
}
