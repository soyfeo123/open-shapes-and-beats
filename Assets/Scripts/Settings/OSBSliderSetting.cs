using UnityEngine;
using UnityEngine.UI;

public class OSBSliderSetting : OSBSettingInstance
{
    [SerializeField] private Slider m_slider;

    private void Start()
    {
        m_slider.onValueChanged.AddListener((float val) =>
        {
            AssignedSetting.floatValue = val;
            AssignedSetting.OnFloatChanged?.Invoke(val);
        });
    }

    protected override void OnAssignedSetting()
    {
        m_slider.minValue = AssignedSetting.minValue;
        m_slider.maxValue = AssignedSetting.maxValue;
        m_slider.value = AssignedSetting.floatValue;
    }
}