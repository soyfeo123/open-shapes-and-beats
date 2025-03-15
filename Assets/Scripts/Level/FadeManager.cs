using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public static class FadeManager
{
    static GameObject fadePrefab;

    [RuntimeInitializeOnLoadMethod]
    static void init()
    {
        fadePrefab = (GameObject)Resources.Load("Prefabs/FadeTemplate");
    }

    public static void FadeIn(float time)
    {
        GameObject instance = GameObject.Instantiate(fadePrefab);

        instance.name = Utils.GenerateUniqueName("Fade");

        instance.GetComponent<Image>().color = new(instance.GetComponent<Image>().color.r, instance.GetComponent<Image>().color.g, instance.GetComponent<Image>().color.b, 1f);
        instance.GetComponent<GraphicRaycaster>().enabled = false;
        instance.GetComponent<Image>().DOFade(0f, time).OnComplete(()=>
        {
            GameObject.Destroy(instance);
        });
    }

    public static void FadeOut(float time, UnityAction actionOnComplete)
    {
        GameObject instance = GameObject.Instantiate(fadePrefab);

        instance.name = Utils.GenerateUniqueName("Fade");

        instance.GetComponent<Image>().color = new(instance.GetComponent<Image>().color.r, instance.GetComponent<Image>().color.g, instance.GetComponent<Image>().color.b, 0f);
        instance.GetComponent<GraphicRaycaster>().enabled = true;
        instance.GetComponent<Image>().DOFade(1f, time).OnComplete(() =>
        {
            actionOnComplete.Invoke();
            GameObject.Destroy(instance, 0.8f);
        });
    }
}