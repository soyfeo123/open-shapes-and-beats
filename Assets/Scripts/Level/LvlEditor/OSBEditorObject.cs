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
    //public Dictionary<string, object> objParams = new Dictionary<string, object>();
    public float relativeXPos;
    public float actualTime
    {
        get
        {
            return relativeXPos * 10f;
        }
        set
        {
            SetRelativePos(value / 10f);
            assignedActor.objParams["Time"] = relativeXPos * 10f;
        }
    }
    bool mustExecute = true;
    private void Awake()
    {
        

        // FOR THE SAKE OF TESTING
        //assignedActor = new FlyingProjectile();
    }
    private void Start()
    {
        Debug.Log(" " + actorType);
        Type typeOfActor = Type.GetType(actorType);
        /*if (typeOfActor == null)
        {
            
            Notification.CreateNotification("[_<_INVALID OBJECT!_>_]\nPlease contact Palo/GameSharp to report this error, along with what you were doing.\nObject name: " + actorType, "[enter] got it", new() { { KeyCode.Return, () => { } } });
            return;
        }*/

        

        assignedActor = Activator.CreateInstance(typeOfActor) as LevelActor;

        assignedActor.objParams["Time"] = Mathf.Infinity;
        
        OSBLevelEditorStaticValues.onPlay.AddListener((time) =>
        {
            mustExecute = actualTime > time;
            Debug.Log("must execute: " + mustExecute);
        });
        OSBLevelEditorStaticValues.onStop.AddListener(() =>
        {
            mustExecute = false;
            if(assignedActor.mainObject != null)
            {
                assignedActor.Dispose();
            }
        });
    }

    private void Update()
    {
        relativeXPos = GetComponent<RectTransform>().anchoredPosition.x - minX;
        
        //Debug.Log(actualTime);
        if (mustExecute && EditorPlayhead.Singleton.SongPosMS >= actualTime)
        {
            mustExecute = false;
            Debug.Log("executing object");
            assignedActor.Prepare();
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
        rt.anchoredPosition = pos;
        relativeXPos = GetComponent<RectTransform>().anchoredPosition.x - minX;
        assignedActor.objParams["Time"] = relativeXPos * 10f;
    }

    public void OnMouseDrag()
    {
        RectTransform rt = transform.parent.GetComponent<RectTransform>();
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out point);
        SetRelativePos(point.x);
        
    }
}
