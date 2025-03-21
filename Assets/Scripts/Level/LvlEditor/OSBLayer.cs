using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;

public class OSBLayer : MonoBehaviour
{
    public List<OSBEditorObject> objectsOnLayer;
    public GameObject objContainer;

    [Header("Debug")]
    public GameObject prefab;

    public void AddObjectToLayer(GameObject obj, float time)
    {
        GameObject instance = Instantiate(obj);
        instance.transform.parent = objContainer.transform;
        instance.GetComponent<OSBEditorObject>().SetPositionWithTime(time);
        //instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(instance.GetComponent<RectTransform>().anchoredPosition.x, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K))
        {
            AddObjectToLayer(prefab, EditorPlayhead.Singleton.SongPosMS);
        }
    }
}
