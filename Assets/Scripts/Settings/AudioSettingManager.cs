using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
    [SerializeField] private AudioMixer m_masterMixer;

    private void Start()
    {
        // master volume
        var masterVol = SettingManager.Singleton.GetSetting("Audio", "Master");
        masterVol.OnFloatChanged = (float val) =>
        {
            m_masterMixer.SetFloat("MasterVol", MapVolumeLog(val));
        };
        m_masterMixer.SetFloat("MasterVol", MapVolumeLog(masterVol.floatValue));

        // music volume
        var musicVol = SettingManager.Singleton.GetSetting("Audio", "Music");
        musicVol.OnFloatChanged = (float val) =>
        {
            m_masterMixer.SetFloat("MusicVol", MapVolumeLog(val));
        };
        m_masterMixer.SetFloat("MusicVol", MapVolumeLog(musicVol.floatValue));

        // sfx volume
        var sfxVol = SettingManager.Singleton.GetSetting("Audio", "SFX");
        sfxVol.OnFloatChanged = (float val) =>
        {
            m_masterMixer.SetFloat("SFXVol", MapVolumeLog(val));
        };
        m_masterMixer.SetFloat("SFXVol", MapVolumeLog(sfxVol.floatValue));
    }

    float MapVolumeLog(float sliderValue)
    {
        float minDb = -80f;
        float maxDb = 0f;
        float linear = sliderValue / 100f;
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }
}