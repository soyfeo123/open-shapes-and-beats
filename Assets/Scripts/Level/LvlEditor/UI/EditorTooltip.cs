using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText;

    public void OnPointerEnter(PointerEventData data)
    {
        OSB_LevelEditorManager.Singleton.ShowTooltip();
        OSB_LevelEditorManager.Singleton.tooltip.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = tooltipText;

        Canvas.ForceUpdateCanvases();
    }

    public void ForceUpdate()
    {
        OSB_LevelEditorManager.Singleton.tooltip.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = tooltipText;
    }

    public void OnPointerExit(PointerEventData data)
    {
        OSB_LevelEditorManager.Singleton.HideTooltip();
    }
}
