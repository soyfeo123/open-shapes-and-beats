using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class OSBMouse : MBSingleton<OSBMouse>
{
    public MouseVisibility Visibility { get; set; } = MouseVisibility.OSB;
    [field: SerializeField] public Color DownButtonColor { get; set; }

    private GameObject m_cursor;
    private Image m_cursorFill;
    private RectTransform m_rt;

    private Color m_cursorFillDefaultColor;

    protected override void Awake()
    {
        base.Awake();
        m_cursor = transform.GetChild(0).gameObject;
        m_cursorFill = m_cursor.transform.Find("Fill").GetComponent<Image>();
        m_cursorFillDefaultColor = m_cursorFill.color;
        m_rt = m_cursor.GetComponent<RectTransform>();
    }

    private void Update()
    {
        m_rt.position = Input.mousePosition;

        if (Visibility == MouseVisibility.OSB)
        {
            m_cursor.SetActive(true);
            Cursor.visible = false;
        }
        if (Visibility == MouseVisibility.System)
        {
            m_cursor.SetActive(false);
            Cursor.visible = true;
        }
        if (Visibility == MouseVisibility.Invisible)
        {
            m_cursor.SetActive(false);
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_cursor.transform.DOKill();
            m_cursorFill.DOKill();
            m_cursorFill.DOColor(DownButtonColor, 0.3f).SetEase(Ease.OutSine);
            m_cursor.transform.DOScale(0.875f, 0.3f).SetEase(Ease.OutSine);
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_cursor.transform.DOKill();
            m_cursorFill.DOKill();
            m_cursorFill.DOColor(m_cursorFillDefaultColor, 0.15f).SetEase(Ease.OutSine);
            m_cursor.transform.DOScale(1, 0.15f).SetEase(Ease.OutBack);
        }
    }
}

public enum MouseVisibility
{
    OSB,
    System,
    Invisible
}