using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OSBSettingInstance : MonoBehaviour, IPointerEnterHandler
{
    public Setting AssignedSetting
    {
        get => m_assignedSetting; set
        {
            m_assignedSetting = value;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().text = value.name;
            OnAssignedSetting();
        }
    }

    private Setting m_assignedSetting;
    
    protected virtual void OnAssignedSetting(){}

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SELECT);
    }
}