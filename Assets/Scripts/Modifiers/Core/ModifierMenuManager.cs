using UnityEngine;

public class ModifierMenuManager : MonoBehaviour
{
    [SerializeField] private Transform m_modifierContainer;
    [SerializeField] private GameObject m_modifierItemPrefab;

    private void Start()
    {
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
            Destroy(gameObject);
        }
    }

    public static void OpenMenu()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/ModifierMenu"));
    }
}
