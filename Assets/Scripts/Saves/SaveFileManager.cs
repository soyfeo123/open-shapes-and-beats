using System.IO;
using UnityEngine;

public static class SaveFileManager
{
    public static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, "save", fileName);
    }
}