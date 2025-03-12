using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSpawn : MonoBehaviour
{
    [Header("Time-related Params")]
    public int WarningMS;
    public int TimeMS;
    public int LengthMS;

    [Header("Activation Methods")]
    public bool ActivatedThroughTime = true;

    bool hasWarningBeenActivated = false;
    bool hasBeenActivated = false;
    bool hasCompleted = false;

    [Header("References")]
    public GameObject WarningObj;
    public GameObject VisualObj;
    public GameObject HitboxObj;

    // Start is called before the first frame update
    void Start()
    {
        WarningObj.SetActive(false);
        VisualObj.SetActive(false);
        HitboxObj.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(MainLevelManager.Singleton.msTime >= TimeMS - WarningMS && !hasWarningBeenActivated)
        {
            ActivateWarning();
        }
        if (MainLevelManager.Singleton.msTime >= TimeMS && !hasBeenActivated)
        {
            ActivateMainObject();
        }

        if (MainLevelManager.Singleton.msTime >= TimeMS + LengthMS && !hasCompleted)
        {
            CompleteObjectCycle();
        }
    }


    public virtual void ActivateWarning()
    {
        hasWarningBeenActivated = true;
        WarningObj.SetActive(true);
    }
    public virtual void ActivateMainObject()
    {
        hasBeenActivated = true;
        HitboxObj.SetActive(true);
        VisualObj.SetActive(true);
    }
    public virtual void CompleteObjectCycle()
    {
        hasCompleted = true;
        HitboxObj.SetActive(false);
        WarningObj.SetActive(false);

        if(VisualObj.GetComponent<Animator>() != null)
        VisualObj.GetComponent<Animator>().enabled = false;

        VisualObj.transform.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.InExpo);
    }
}
