using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SettingMenuManager : MonoBehaviour
{
    [Header("Dynamics")]
    [SerializeField] private GameObject m_settingCategoryPrefab;
    [SerializeField] private GameObject m_sliderSettingPrefab;
    [SerializeField] private GameObject m_dropdownSettingPrefab;
    [SerializeField] private GameObject m_toggleSettingPrefab;

    [Header("References")]
    [SerializeField] private Transform m_settingContainer;
    [SerializeField] private Button m_settingCloseHitbox;
    [SerializeField] private Transform m_settingCatSideBar;
    [SerializeField] private Transform m_settingMainSideBar;

    public void Close()
    {
        m_settingCloseHitbox.gameObject.SetActive(false);
        m_settingMainSideBar.DOMoveX(-200f, 0.35f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            Destroy(gameObject);
        });
        m_settingCatSideBar.DOMoveX(-200f, 0.35f).SetEase(Ease.InExpo);
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_MENU_CLOSE);
    }

    private void Awake()
    {
        foreach (var category in SettingManager.Singleton.Categories)
        {
            var categoryUI = Instantiate(m_settingCategoryPrefab, m_settingContainer, false);

            foreach (var setting in category.settings)
            {
                switch (setting.type)
                {
                    case SettingType.Toggle:
                        var toggle = Instantiate(m_toggleSettingPrefab, categoryUI.transform, false);
                        var toggleMan = toggle.GetComponent<OSBToggleSetting>();
                        toggleMan.AssignedSetting = setting;
                        break;
                    case SettingType.Slider:
                        var slider = Instantiate(m_sliderSettingPrefab, categoryUI.transform, false);
                        var sliderMan = slider.GetComponent<OSBSliderSetting>();
                        sliderMan.AssignedSetting = setting;
                        break;
                    case SettingType.Dropdown:
                        var drop = Instantiate(m_dropdownSettingPrefab, categoryUI.transform, false);
                        var dropMan = drop.GetComponent<OSBDropdownSetting>();
                        dropMan.AssignedSetting = setting;
                        break;
                }
            }

            var categoryUIRefs = categoryUI.GetComponent<SettingHeaderPointers>();
            categoryUIRefs.bottomMargin.transform.SetAsLastSibling();
            categoryUIRefs.divider.transform.SetAsLastSibling();
            categoryUIRefs.title.text = category.categoryName;
        }

        m_settingCloseHitbox.onClick.AddListener(Close);
    }

    public void Instant()
    {
        StartCoroutine(WaitUntilDestroy());
    }

    public static GameObject Open()
    {
        var menu = Instantiate(Resources.Load<GameObject>("Prefabs/Settings/OptionsMenu"));

        OpenSection(menu.transform.Find("SettingsMenu"));
        OpenSection(menu.transform.Find("SettingsSidebar"));

        return menu;
    }

    public static void ApplyAllSettingsAtOnce()
    {
        var menu = Open();
        menu.GetComponent<SettingMenuManager>().Instant();
    }

    IEnumerator WaitUntilDestroy()
    {
        yield return null;
        Destroy(gameObject);
    }

    private static void OpenSection(Transform section)
    {
        var originalPosition = section.position;

        section.position = new Vector2(-200, section.position.y);

        section.DOMove(originalPosition, 0.35f).SetEase(Ease.OutExpo);
    }
}