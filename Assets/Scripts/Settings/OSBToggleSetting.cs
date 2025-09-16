using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class OSBToggleSetting : OSBSettingInstance
{
    [SerializeField] private Transform m_filledGraphic;

    private Button m_myButton;
    private bool Value;

    void Start()
    {
        Value = AssignedSetting.boolValue;

        m_myButton = GetComponent<Button>();

        m_myButton.onClick.AddListener(() =>
        {
            Value = !Value;
            UpdateVisual(true);

            AssignedSetting.boolValue = Value;
            AssignedSetting.OnBoolChanged?.Invoke(Value);
        });

        UpdateVisual(false);
        AssignedSetting.OnBoolChanged?.Invoke(Value);
    }

    public void UpdateVisual(bool sound)
    {
        m_filledGraphic.DOScale(Value ? 1 : 0, 0.25f).SetEase(Ease.OutExpo);
        
        if(sound)
            SoundManager.Singleton.PlaySound(Value ? LoadedSFXEnum.UI_CHECK_ON : LoadedSFXEnum.UI_CHECK_OFF);
    }
}
