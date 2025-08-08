using DG.Tweening;
using UnityEngine;

public class AnimatedMenuOpener : MonoBehaviour
{
    [SerializeField] private Vector2 m_initialState;
    [SerializeField] private float m_duration;
    [SerializeField] private float m_delay = 0;
    [SerializeField] private Ease m_easingFunction;

    private Vector2 m_defaultState;

    public void Animate()
    {
        var rect = GetComponent<RectTransform>();
        rect.DOKill();
        m_defaultState = rect.anchoredPosition;

        rect.anchoredPosition = m_initialState;

        rect.DOAnchorPos(m_defaultState, m_duration).SetEase(m_easingFunction).SetDelay(m_delay);

        Debug.Log(gameObject.name + " plus " + m_defaultState);
    }

    public void AnimateExit()
    {
        var rect = GetComponent<RectTransform>();
        rect.DOKill();
        m_defaultState = rect.anchoredPosition;

        rect.anchoredPosition = m_defaultState;

        rect.DOAnchorPos(m_initialState, 0.5f).SetEase(m_easingFunction).OnComplete(() => { rect.anchoredPosition = m_defaultState; });

        Debug.Log(gameObject.name + " plus " + m_defaultState);
    }
}
