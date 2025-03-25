using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Newtonsoft.Json;

public class OSBLayer : MonoBehaviour
{
    public List<OSBEditorObject> objectsOnLayer = new List<OSBEditorObject>();
    public GameObject objContainer;

    public static OSBLayer currentlySelectedLayer;

    public Image layerStuffBg;
    public Color selectedColor;
    Color defaultColor;
    LevelActor thingToClone;
    GameObject objToClone;
    KeyCode keybind;

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

    private void Start()
    {
        defaultColor = layerStuffBg.color;
    }

    internal static Dictionary<string, ActorParam> Clone(Dictionary<string, ActorParam> dictIn)
    {
        Dictionary<string, ActorParam> dictOut = new Dictionary<string, ActorParam>();

        IDictionaryEnumerator enumMyDictionary = dictIn.GetEnumerator();
        while (enumMyDictionary.MoveNext())
        {
            string strKey = (string)enumMyDictionary.Key;
            ActorParam oValue = enumMyDictionary.Value as ActorParam;
            dictOut.Add(strKey, oValue);
        }

        return dictOut;
    }

    private void Update()
    {
        /*if (!OSB_LevelEditorManager.IsPointerFocusedInputField() && ( Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K)))
        {
            AddObjectToLayer(prefab, EditorPlayhead.Singleton.SongPosMS);
        }*/

        if (currentlySelectedLayer == this)
        {
            layerStuffBg.color = selectedColor;
        }
        else
        {
            layerStuffBg.color = defaultColor;
        }

        if (OSB_LevelEditorManager.Singleton.isRecording)
        {
            if (Input.GetKeyDown(keybind))
            {
                
                GameObject instance = Instantiate(objToClone);
                // HELL YEAH I MADE IT WORK
                // I'M A GENIUS
                // hacky solution though
                // WHO CARES?????
                instance.GetComponent<OSBEditorObject>().InitInstance();
                instance.GetComponent<OSBEditorObject>().assignedActor.objParams = JsonConvert.DeserializeObject<Dictionary<string, ActorParam>>(JsonConvert.SerializeObject(thingToClone.objParams));
                
                instance.GetComponent<OSBEditorObject>().assignedActor.objParams["Time"].number.Value = EditorPlayhead.Singleton.SongPosMS;
                instance.transform.SetParent(objContainer.transform);
                
                
                //instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(instance.GetComponent<RectTransform>().anchoredPosition.x, 0);
                Debug.Log(instance.GetComponent<OSBEditorObject>());
                objectsOnLayer.Add(instance.GetComponent<OSBEditorObject>());
            }
        }
    }

    public void SetAsSelectedLayer()
    {
        currentlySelectedLayer = this;
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
            objectsOnLayer.Clear();
        } }, { KeyCode.Escape, ()=>{ } } } );
    }

    public void CallbackObjectClick(BaseEventData data_)
    {
        PointerEventData data = data_ as PointerEventData;

        switch (data.button)
        {
            case PointerEventData.InputButton.Left:
                GameObject window = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/ParamsWindow"));
                window.GetComponent<ParamsWindowController>().actor = thingToClone;
                window.transform.SetParent(OSB_LevelEditorManager.Singleton.transform, false);
                window.GetComponent<ParamsWindowController>().InitWindow();
                break;
            case PointerEventData.InputButton.Right:
                LvlEditorInventory.OpenInventory((GameObject editorObject, string actorName) =>
                {
                    Type actorType = Type.GetType(actorName);
                    if(actorType == null)
                    {
                        Notification.CreateNotification("[_<_INVALID OBJECT!_>_]\nSomething went wrong and a wrong type of object was given.\nSend Palo/GameSharp the Unity logs along with what you were doing.\nSorry bout this!", "[enter] fine", new Dictionary<KeyCode, UnityEngine.Events.UnityAction>() { { KeyCode.Return, () => { } } });
                        return;
                    }

                    thingToClone = Activator.CreateInstance(actorType) as LevelActor;
                    objToClone = editorObject;
                    Debug.Log("Loaded " + actorName);
                });
                break;
        }
    }

    // hell yeah i love long function names
    IEnumerator WaitForKeyPressAndAssign()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        foreach(KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
            {
                keybind = key;
            }
        }
        Debug.Log(keybind);
    }

    

    public void CallbackKeyClicked()
    {
        StartCoroutine(WaitForKeyPressAndAssign());
    }
}
