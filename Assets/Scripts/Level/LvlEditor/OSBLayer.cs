/*
 * OSBLayer.cs contributors:
 * GXGamerRU
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class OSBLayer : MonoBehaviour
{
    public List<OSBEditorObject> objectsOnLayer = new List<OSBEditorObject>();
    public GameObject objContainer;

    public static OSBLayer currentlySelectedLayer;

    public Image layerStuffBg;
    public Color selectedColor;
    public EditorTooltip bindKeyTooltip;
    Color defaultColor;
    LevelActor thingToClone;
    GameObject objToClone;
    public EditorActions m_keybinds;
    string keybind = "";

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

    void OnEnable()
    {
        m_keybinds = new EditorActions();

        m_keybinds.Main.Enable();
        m_keybinds.Main.Record.performed += Record_performed;
    }

    private void Record_performed(InputAction.CallbackContext obj)
    {
        // we only want a key from a keyboard

        //Debug.Log("pressed key? " + "ERM WHAT THE SIGAM");
        //if (!(obj.control is KeyControl keyControl)) return;

        //KeyCode pressedKey = (KeyCode)(int)keyControl.keyCode;

        //Debug.Log("pressed key? " + pressedKey);
        //// if the key pressed is not the key assigned, forget it all
        //if (pressedKey != keybind) return;

        // not needed because keybinds are set but hey just in case :)

        if (!OSB_LevelEditorManager.Singleton.isRecording) return;

        SpawnObjectAt((int)EditorPlayhead.Singleton.SongPosMS);
    }

    void OnDisable()
    {
        m_keybinds.Main.Disable();
        m_keybinds.Main.Record.performed -= Record_performed;
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

    int framesElapsedSinceProejctileLaunch = 0;

    private void Update()
    {
        /*if (!OSB_LevelEditorManager.IsPointerFocusedInputField() && ( Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K)))
        {
            AddObjectToLayer(prefab, EditorPlayhead.Singleton.SongPosMS);
        }*/

        bindKeyTooltip.tooltipText = "Key: " + keybind.ToString();

        if (currentlySelectedLayer == this)
        {
            layerStuffBg.color = selectedColor;
        }
        else
        {
            layerStuffBg.color = defaultColor;
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
    void WaitForKeyPressAndAssign()
    {
        m_keybinds.Disable();

        ScreenMessage msg = ScreenMessages.Create("<b>PRESS ANY KEY</b>\nto bind the layer");
        Debug.Log("Press any key");

        m_keybinds.Main.Record.PerformInteractiveRebinding().WithControlsExcluding("Mouse").OnComplete(op =>
        {
            Debug.Log("Pressed");
            
            
            var control = m_keybinds.Main.Record;

            keybind = op.selectedControl.path;

            msg.RemoveFromScreen();

            op.Dispose();
        }).Start();

        m_keybinds.Enable();
    }

    

    public void CallbackKeyClicked()
    {
        WaitForKeyPressAndAssign();
    }

    private void SpawnObjectAt(int timestamp)
    {
        GameObject instance = Instantiate(objToClone);
        
        instance.GetComponent<OSBEditorObject>().InitInstance();
        instance.GetComponent<OSBEditorObject>().assignedActor.objParams = JsonConvert.DeserializeObject<Dictionary<string, ActorParam>>(JsonConvert.SerializeObject(thingToClone.objParams));

        instance.GetComponent<OSBEditorObject>().assignedActor.objParams["Time"].number.expression = timestamp.ToString();
        instance.transform.SetParent(objContainer.transform);

        //if (keybind == KeyCode.Mouse0)
        //{
        //    instance.GetComponent<OSBEditorObject>().assignedActor.OverridePositionParam(0, 0);
        //}

        Debug.Log(instance.GetComponent<OSBEditorObject>());
        objectsOnLayer.Add(instance.GetComponent<OSBEditorObject>());
    }

    public string Save()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("LAYER>a");

        void AppendProperty(string name, string value)
        {
            builder.Append(name + ":" + value + ",");
        }

        foreach(OSBEditorObject obj in objectsOnLayer)
        {
            builder.Append("OBJ>");

            AppendProperty("d_Class", obj.actorType);
            foreach(KeyValuePair<string, ActorParam> param in obj.assignedActor.objParams)
            {
                AppendProperty(param.Key, param.Value.number.expression);
            }
            builder.AppendLine();
        }
        builder.AppendLine("END");
        return builder.ToString();
    }

    public void Load(string[] objects)
    {
        foreach(string objectLine in objects)
        {
            if (string.IsNullOrEmpty(objectLine))
            {
                continue;
            }
            Debug.Log(objectLine);
            string[] paramsSplit = objectLine.Split(',');
            string[] dClass = paramsSplit[0].Split(':');
            GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/" + dClass[1]));
            
            instance.GetComponent<OSBEditorObject>().actorType = dClass[1];
            instance.GetComponent<OSBEditorObject>().InitInstance();
            instance.transform.SetParent(objContainer.transform);

            foreach (string param in paramsSplit)
            {
                
                string[] paramNameAndKey = param.Split(':');
                
                
                string paramName = paramNameAndKey[0];
                if (string.IsNullOrEmpty(paramName))
                {
                    continue;
                }

                if(paramName == "d_Class")
                {
                    continue;
                }

                ActorParam value = new ActorParam(paramNameAndKey[1], paramNameAndKey[1]);

                instance.GetComponent<OSBEditorObject>().assignedActor.objParams[paramName] = value;
            }

            objectsOnLayer.Add(instance.GetComponent<OSBEditorObject>());
            

        }
    }
}

/*
LEVEL FORMAT

(level song path)
LAYER>a
OBJ>d_Class:(auto filled using OSBEditorObject),Warning:4721,Time:8643.32
OBJ>d_Class:(auto filled using OSBEditorObject),Warning:4721,Time:8643.32
LAYER>a
OBJ>d_Class:(auto filled using OSBEditorObject),Warning:4721,Time:8643.32
OBJ>d_Class:(auto filled using OSBEditorObject),Warning:4721,Time:8643.32
 */