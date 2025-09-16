using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OSBDropdownSetting : OSBSettingInstance, IPointerClickHandler
{
    [SerializeField] private TMP_Dropdown m_dropdown;

    private void Start()
    {
        m_dropdown.onValueChanged.AddListener((int val) =>
        {
            AssignedSetting.intValue = val;
            AssignedSetting.OnIntChanged?.Invoke(val);
        });

        m_dropdown.options.Clear();
        foreach (var option in AssignedSetting.options)
        {
            m_dropdown.options.Add(new(option));
        }

        m_dropdown.value = AssignedSetting.intValue;
    }

    protected override void OnAssignedSetting()
    {
        m_dropdown.value = AssignedSetting.intValue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("playing sond");
        SoundManager.Singleton.PlaySound(m_dropdown.IsExpanded ? LoadedSFXEnum.UI_MENU_OPEN : LoadedSFXEnum.UI_MENU_CLOSE);
    }
}