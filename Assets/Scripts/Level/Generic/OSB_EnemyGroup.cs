using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [Header("Time-Related Stuff")]
    public int TimeMS;
    public int WarningMS;
    public int DelayBetweenObjectsMS;

    [Header("Position-Related Stuff")]
    public bool XScatter = false;
    public bool YScatter = false;
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;


    int lastMS;
    int childrenCount;

    // Start is called before the first frame update
    void Start()
    {
        childrenCount = transform.childCount;
        for(int i = 0; i < childrenCount; i++)
        {
            transform.GetChild(i).GetComponent<LevelSpawn>().TimeMS = TimeMS + (DelayBetweenObjectsMS * i);
            transform.GetChild(i).GetComponent<LevelSpawn>().WarningMS = WarningMS;
            
            transform.GetChild(i).position = new Vector3(XScatter ? Random.Range(MinX, MaxX) : transform.GetChild(i).position.x, YScatter ? Random.Range(MinY, MaxY) : transform.GetChild(i).position.y, transform.GetChild(i).position.z);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
