using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

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

    public static float ConvertPixelToPosition(float percent, UtilsDirection direction)
    {
        return Mathf.LerpUnclamped(direction == UtilsDirection.X ? -9f : 5f, direction == UtilsDirection.X ? 9f : -5f, Mathf.InverseLerp(0, direction == UtilsDirection.X ? 1280 : 720, percent));
    }

    public static float CalculateSize(float percent, float baseValue)
    {
        return Mathf.LerpUnclamped(0, baseValue, percent * 0.01f);
    }

    public static float MapWorldToPixel(float value, UtilsDirection direction)
    {
        float t = Mathf.InverseLerp(direction == UtilsDirection.X ? -9f : 5f, direction == UtilsDirection.X ? 9f : -5f, value); 
        return Mathf.LerpUnclamped(0, direction == UtilsDirection.X ? 1280 : 720, t);
    }

    /// <summary>
    /// uses json so idk bout this
    /// </summary>
    /// <returns></returns>
    public static T CloneObject<T>(T obj)
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
    }
}

public enum UtilsDirection
{
    X, Y
}