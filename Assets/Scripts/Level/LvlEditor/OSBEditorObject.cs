using UnityEngine;
using OSB.Editor;

public class OSBEditorObject : MonoBehaviour
{
    public LevelActor assignedActor;
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
        }
    }
    bool mustExecute = true;
    private void Awake()
    {
        // FOR THE SAKE OF TESTING
        assignedActor = new FlyingProjectile();
    }
    private void Start()
    {
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
    }
}
