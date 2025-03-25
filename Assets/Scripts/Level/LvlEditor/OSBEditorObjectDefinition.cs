using UnityEngine;

[CreateAssetMenu(fileName = "FxGeneric", menuName = "Open Shapes & Beats Editor/Create Object Defintion")]
public class OSBEditorObjectDefinition : ScriptableObject
{
    public GameObject prefab;
    public Sprite editorIcon;
    public string cSharpActorName;
}