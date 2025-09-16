using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    public static string CurrentEffectName { get; set; } = "JSABTrail";

    public GameObject CurrentEffect
    {
        get
        {
            return transform.Find(CurrentEffectName).gameObject;
        }
    }

    private ParticleSystem[] m_effectParticleSystems;

    public void Play()
    {
        for (int i = 0; i < m_effectParticleSystems.Length; i++)
        {
            m_effectParticleSystems[i].Play();
        }
    }

    public void Stop()
    {
        for (int i = 0; i < m_effectParticleSystems.Length; i++)
        {
            m_effectParticleSystems[i].Stop();
        }
    }

    public void LoadTrailFromSettings()
    {
        var setting = SettingManager.Singleton.GetSetting("Gameplay", "PlrTrail");
        Debug.Log("loading setting " + setting.name + " with val: " + setting.intValue);
        CurrentEffectName = transform.GetChild(setting.intValue).name;
        Debug.Log("loaded " + CurrentEffectName);

        m_effectParticleSystems = CurrentEffect.GetComponentsInChildren<ParticleSystem>();
        CurrentEffect.SetActive(true);
    }

    private void Awake()
    {
        foreach (Transform childEffect in transform)
        {
            childEffect.gameObject.SetActive(false);
        }
        LoadTrailFromSettings();
    }
}
