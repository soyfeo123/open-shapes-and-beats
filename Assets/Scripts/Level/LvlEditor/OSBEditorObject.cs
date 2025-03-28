using UnityEngine;
using OSB.Editor;
using System.Reflection;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OSBEditorObject : MonoBehaviour
{
    public string actorType;
    public LevelActor assignedActor;
    public bool objectNeedsWarning = false;
    public RectTransform activeZone;
    public RectTransform warningZone;
    public RectTransform activeZoneDrag;
    public RectTransform warningZoneDrag;

    public Dictionary<string, ActorParam> paramTemplate;

    //public Dictionary<string, object> objParams = new Dictionary<string, object>();
    public float relativeXPos;
    public bool initializeActor = true;
    public float actualTime
    {
        get
        {
            return relativeXPos * 10f;
        }
        set
        {
            

            SetRelativePos(value / 10f);

            assignedActor.objParams["Time"].number.expression = (relativeXPos * 10f).ToString();
        }
    }
    bool mustExecute = false;

    bool hasWarned;
    bool hasActivated;

    public void InitInstance()
    {
        Debug.Log(" " + actorType);
        Type typeOfActor = Type.GetType(actorType);
        if (typeOfActor == null)
        {

            Notification.CreateNotification("[_<_INVALID OBJECT!_>_]\nPlease contact Palo/GameSharp to report this error, along with what you were doing.\nObject name: " + actorType, "[enter] got it", new() { { KeyCode.Return, () => { } } });
            return;
        }



        assignedActor = Activator.CreateInstance(typeOfActor) as LevelActor;

        assignedActor.objParams["Time"].number.expression = actualTime.ToString();
    }

    private void Awake()
    {
        if(assignedActor == null)
        {
            InitInstance();
        }


        // FOR THE SAKE OF TESTING
        //assignedActor = new FlyingProjectile();
    }
    private void Start()
    {
        if(paramTemplate != null)
        {
            assignedActor.objParams = new Dictionary<string, ActorParam>(paramTemplate);

        }

        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 0);
        
        OSBLevelEditorStaticValues.onPlay.AddListener((time) =>
        {
            mustExecute = actualTime > time;
            Debug.Log("must execute: " + mustExecute);
        });
        OSBLevelEditorStaticValues.onStop.AddListener(() =>
        {
            mustExecute = false;

            hasWarned = false;
            hasActivated = false;
            assignedActor.hasActivated = false;
            assignedActor.hasPrepared = false;

            if(assignedActor.mainObject != null)
            {
                assignedActor.Dispose();
            }
        });
    }

    private void Update()
    {
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition;
        pos.x = assignedActor.objParams["Time"].number.GetValue() * 0.1f;
        actualTime = assignedActor.objParams["Time"].number.GetValue();
        GetComponent<RectTransform>().anchoredPosition = pos;

        if (objectNeedsWarning)
        {
            Vector2 warningZoneSize = warningZone.sizeDelta;
            warningZoneSize.x = assignedActor.objParams["Warning"].number.GetValue() * 0.1f;
            warningZone.sizeDelta = warningZoneSize;

            Vector2 activeZoneSize = activeZone.sizeDelta;
            activeZoneSize.x = assignedActor.objParams["Duration"].number.GetValue() * 0.1f;
            activeZone.sizeDelta = activeZoneSize;

            var activeZoneDragPos = activeZoneDrag.GetComponent<RectTransform>().anchoredPosition;
            activeZoneDragPos.x = activeZone.sizeDelta.x;
            activeZoneDrag.GetComponent<RectTransform>().anchoredPosition = activeZoneDragPos;

            var warningZoneDragPos = activeZoneDrag.GetComponent<RectTransform>().anchoredPosition;
            warningZoneDragPos.x = warningZone.sizeDelta.x;
            warningZoneDrag.GetComponent<RectTransform>().anchoredPosition = -warningZoneDragPos;
        }
        

        //Debug.Log(actualTime);
        if (mustExecute && EditorPlayhead.Singleton.SongPosMS >= actualTime && !objectNeedsWarning)
        {
            mustExecute = false;
            Debug.Log("executing object");
            assignedActor.Prepare();
        }

        if (mustExecute && !hasWarned && objectNeedsWarning && EditorPlayhead.Singleton.SongPosMS >= actualTime - assignedActor.objParams["Warning"].number.GetValue())
        {
            hasWarned = true;
            assignedActor.Prepare();
        }
        if(mustExecute && !hasActivated && objectNeedsWarning && EditorPlayhead.Singleton.SongPosMS >= actualTime)
        {
            hasActivated = true;
            assignedActor.ActivateAttack();
        }
        if (mustExecute &&objectNeedsWarning && EditorPlayhead.Singleton.SongPosMS >= actualTime + assignedActor.objParams["Duration"].number.GetValue())
        {
            mustExecute = false;
            assignedActor.Dispose();
        }
    }

    public float minX = 0;
    public float maxX = 9999f;

    public void SetPositionWithTime(float timeInMS)
    {
        SetRelativePos(timeInMS / 10f);
    }

    public void SetRelativePos(float xPos)
    {
        RectTransform rt = GetComponent<RectTransform>();
        xPos = Mathf.Clamp(xPos, 0, 99999f);
        Vector2 pos = rt.anchoredPosition;
        pos.x = xPos + minX;
        pos.y = 0;
        rt.anchoredPosition = pos;
        relativeXPos = GetComponent<RectTransform>().anchoredPosition.x - minX;
        assignedActor.objParams["Time"].number.expression = (relativeXPos * 10f).ToString();
    }

    float offset = 0;

    bool isDrag = false;

    public void OnMouseDrag()
    {

        isDrag = true;
        RectTransform rt = transform.parent.GetComponent<RectTransform>();
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out point);
        SetRelativePos(point.x - offset);
        
    }

    public void SetOffsetForDrag(BaseEventData data_)
    {
        isDrag = false;
        PointerEventData data = data_ as PointerEventData;
        if (data.button != PointerEventData.InputButton.Left) return;

        RectTransform rt = transform.GetComponent<RectTransform>();
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out point);
        offset = point.x;
        Debug.Log(offset);
    }

    public void OnRightClicked(BaseEventData data_)
    {
        PointerEventData data = data_ as PointerEventData;
        switch (data.button)
        {
            default:
                if (!isDrag)
                {
                    GameObject window = Instantiate(Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/ParamsWindow"));
                    window.GetComponent<ParamsWindowController>().actor = assignedActor;
                    window.transform.SetParent(OSB_LevelEditorManager.Singleton.transform, false);
                    window.GetComponent<ParamsWindowController>().InitWindow();
                }

                break;

            case PointerEventData.InputButton.Right:
                Debug.Log("yes");
                transform.parent.parent.GetComponent<OSBLayer>().objectsOnLayer.Remove(this);
                Destroy(gameObject);

                break;
        }
    }


    float warningZoneOffset = 0;

    public void SetOffsetForDragWarning(BaseEventData data_)
    {
        PointerEventData data = data_ as PointerEventData;
        if (data.button != PointerEventData.InputButton.Left) return;

        
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(warningZoneDrag, Input.mousePosition, null, out point);
        warningZoneOffset = point.x;
        
    }

    public void DragWarningZone()
    {
        

        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(warningZone, Input.mousePosition, null, out point);

        assignedActor.objParams["Warning"].number.expression = ((-point.x + warningZoneOffset) * 10f).ToString();
    }



    float activeZoneOffset = 0;

    public void SetOffsetForDragActive(BaseEventData data_)
    {
        PointerEventData data = data_ as PointerEventData;
        if (data.button != PointerEventData.InputButton.Left) return;


        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(activeZoneDrag, Input.mousePosition, null, out point);
        activeZoneOffset = point.x;

    }

    public void DragActiveZone()
    {


        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(activeZone, Input.mousePosition, null, out point);

        assignedActor.objParams["Duration"].number.expression = ((point.x - activeZoneOffset) * 10f).ToString();
    }
}
