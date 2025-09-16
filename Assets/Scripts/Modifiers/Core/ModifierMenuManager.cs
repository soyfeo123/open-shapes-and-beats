using UnityEngine;

public class ModifierMenuManager : MonoBehaviour
{
    [SerializeField] private Transform m_modifierContainer;
    [SerializeField] private GameObject m_modifierItemPrefab;

    private void Start()
    {
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_MENU_OPEN);

        foreach(var definition in ModifierManager.Singleton.AllModifiers)
        {
            GameObject newItem = Instantiate(m_modifierItemPrefab, m_modifierContainer);
            newItem.GetComponent<ModifierItem>().AssignedDefinition = definition;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_MENU_CLOSE);
            Destroy(gameObject);
        }
    }

    public static void OpenMenu()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/ModifierMenu"));
    }
}
