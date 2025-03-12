using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicRemoveOnNoView : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit trigger");
        if(collision.tag == "LevelHitbox")
        {
            Debug.Log("Detected object getting out");
            Destroy(collision.transform.parent);
        }
    }
}
