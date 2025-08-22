using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    public static string CurrentEffectName { get; set; } = "JSABTrail";

    public GameObject CurrentEffect { get
        {
            return transform.Find(CurrentEffectName).gameObject;
        } }

    private ParticleSystem[] m_effectParticleSystems;

    public void Play()
    {
        for(int i = 0; i < m_effectParticleSystems.Length; i++)
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

    private void Start()
    {
        foreach(Transform childEffect in transform)
        {
            childEffect.gameObject.SetActive(false);
        }

        CurrentEffect.SetActive(true);

        m_effectParticleSystems = CurrentEffect.GetComponentsInChildren<ParticleSystem>();
    }
}
