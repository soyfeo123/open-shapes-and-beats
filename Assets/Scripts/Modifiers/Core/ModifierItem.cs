using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModifierItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field:SerializeField] public ModifierDefinition AssignedDefinition { get; set; }

    [Space]

    [SerializeField] private Image m_leftTriangle;
    [SerializeField] private Image m_rightTriangle;
    private Image m_mainImage;

    private Vector2 m_defaultSize;
    private RectTransform m_rectTransform;
    private TextMeshProUGUI m_text;

    private Color m_backColor;
    private Color m_foreColor;

    private bool m_enabled = false;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_defaultSize = m_rectTransform.sizeDelta;
        m_mainImage = GetComponent<Image>();

        m_mainImage.color = AssignedDefinition.color;
        m_leftTriangle.color = AssignedDefinition.color;
        m_rightTriangle.color = AssignedDefinition.color;

        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_text.text = AssignedDefinition.modifierTitle;

        m_backColor = m_mainImage.color;
        m_foreColor = m_text.color;

        m_enabled = ModifierManager.Singleton.IsEnabled(AssignedDefinition);
        UpdateVisual(m_enabled, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_enabled = !m_enabled;

        ModifierManager.Singleton.EnableModifier(AssignedDefinition, m_enabled);

        UpdateVisual(m_enabled);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_rectTransform.DOKill();
        m_rectTransform.DOSizeDelta(new Vector2(m_defaultSize.x * 2f, m_defaultSize.y), 0.15f).SetEase(Ease.OutBack);
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SELECT);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_rectTransform.DOKill();
        m_rectTransform.DOSizeDelta(m_defaultSize, 0.15f).SetEase(Ease.OutBack);
    }

    public void UpdateVisual(bool selected, bool withSound = true)
    {
        m_text.color = selected ? m_backColor : m_foreColor;

        m_mainImage.DOColor(selected ? m_foreColor : m_backColor, 0.1f);
        m_leftTriangle.DOColor(selected ? m_foreColor : m_backColor, 0.1f);
        m_rightTriangle.DOColor(selected ? m_foreColor : m_backColor, 0.1f);

        m_rectTransform.DOKill();
        if (selected)
            m_rectTransform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack, 2.8f);
        else
            m_rectTransform.DOScale(1f, 0.1f).SetEase(Ease.OutBack);

        if(withSound)
            SoundManager.Singleton.PlaySound(selected ? LoadedSFXEnum.UI_PANEL_OPEN : LoadedSFXEnum.UI_PANEL_CLOSE);
    }
}
