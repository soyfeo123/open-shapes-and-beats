using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public static class Utils
{
    public static string GenerateUniqueName(string prefix)
    {
        return prefix + "-" + Random.Range(472, 1754);
    }

    public static void Timer(float duration, UnityAction onComplete)
    {
        float filler = 0f;
        DOTween.To(() => filler, x => filler = x, 1f, duration).OnComplete(() => onComplete?.Invoke());
    }
}