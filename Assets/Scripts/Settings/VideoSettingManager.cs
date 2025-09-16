using UnityEngine;
using UnityEngine.Audio;

public class VideoSettingManager : MonoBehaviour
{
    private void Start()
    {
        // resolution + fs
        var resolution = SettingManager.Singleton.GetSetting("Video", "Res");
        var fullscreen = SettingManager.Singleton.GetSetting("Video", "FS");

        resolution.OnIntChanged = (int val) =>
        {
            SettingManager.Singleton.ApplyResolutionAndFullScreen();
        };
        fullscreen.OnBoolChanged = (bool val) =>
        {
            SettingManager.Singleton.ApplyResolutionAndFullScreen();
        };
    }
}