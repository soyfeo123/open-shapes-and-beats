using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LvlEditorInventory : MonoBehaviour
{
    GameObject item;
    public Transform scrollViewContent;
    public UnityEvent<GameObject, string> onChosen;

    private void Awake()
    {
        item = Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/UI/InventoryItem");
    }

    void Start()
    {
        OSBEditorObjectDefinition[] objects = Resources.LoadAll<OSBEditorObjectDefinition>("Prefabs/LevelEditorObjects/");

        foreach(OSBEditorObjectDefinition obj in objects)
        {
            GameObject inst = Instantiate(item);
            inst.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = obj.name;
            inst.transform.SetParent(scrollViewContent, true);
            inst.GetComponent<Button>().onClick.AddListener(() =>
            {
                onChosen.Invoke(obj.prefab, obj.cSharpActorName);
                Destroy(gameObject);
            });
        }
    }

    public static void OpenInventory(UnityAction<GameObject, string> onChosen_)
    {
        GameObject instantiate = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/UI/Inventory"));
        instantiate.transform.SetParent(GameObject.Find("OSB_LevelEditorUI").transform, false);

        instantiate.GetComponent<LvlEditorInventory>().onChosen.AddListener(onChosen_);
    }
}
