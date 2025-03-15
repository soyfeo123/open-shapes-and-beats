using UnityEngine;

public static class Utils
{
    public static string GenerateUniqueName(string prefix)
    {
        return prefix + "-" + Random.Range(472, 1754);
    }
}