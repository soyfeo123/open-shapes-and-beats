using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;

public class OSBLayer : MonoBehaviour
{
    public List<OSBEditorObject> objectsOnLayer = new List<OSBEditorObject>();
    public GameObject objContainer;

    [Header("Debug")]
    public GameObject prefab;

    public void AddObjectToLayer(GameObject obj, float time)
    {
        GameObject instance = Instantiate(obj);
        instance.transform.SetParent(objContainer.transform);
        instance.GetComponent<OSBEditorObject>().SetPositionWithTime(time);
        //instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(instance.GetComponent<RectTransform>().anchoredPosition.x, 0);
        Debug.Log(instance.GetComponent<OSBEditorObject>());
        objectsOnLayer.Add(instance.GetComponent<OSBEditorObject>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K))
        {
            AddObjectToLayer(prefab, EditorPlayhead.Singleton.SongPosMS);
        }
    }

    public void ClearLayer()
    {
        Notification.CreateNotification("[_<_YOU SURE?_>_]\nEverything on this layer [_WILL_] be cleared!\nFor a very long time, I may add.", "[enter] yes please   [esc] wait no", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Return, ()=>
        {
            Debug.Log("DELETING WOMP WOMP");
            foreach(OSBEditorObject obj in objectsOnLayer)
            {
                Debug.Log(obj);
                Destroy(obj.gameObject);
                
            }
        } }, { KeyCode.Escape, ()=>{ } } } );
    }
}
