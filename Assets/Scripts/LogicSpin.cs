using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSpin : MonoBehaviour
{
    public float speed;
    public bool startAtRandomDir;
    public bool randomClockDirection;
    bool clockDir = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(startAtRandomDir ? new Vector3(0, 0, Random.Range(-180, 180)) : transform.rotation.eulerAngles);

        if (randomClockDirection)
            clockDir = Random.Range(0, 2) == 0 ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * (clockDir ? 1 : -1) * Time.deltaTime);
    }
}
