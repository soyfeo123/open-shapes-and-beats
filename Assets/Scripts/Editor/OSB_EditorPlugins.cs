using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class OSB_EditorPlugins
{
    static string bundlePath = "Assets/StreamingAssets/levels";
    static string rawLevelsPath = "Assets/Levels";

    [MenuItem("Open Shapes & Beats Editor/Update ST Level Folder")]
    public static void UpdateLevelFolder()
    {
        Debug.Log("[OSB] Getting levels...");
        string[] allSubFolders = AssetDatabase.GetSubFolders(rawLevelsPath);
        foreach (string level in allSubFolders)
        {
            Debug.Log("[OSB] Found " + level + ", creating asset bundle");
        }

        BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
    }
}

