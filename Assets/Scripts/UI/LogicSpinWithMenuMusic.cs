using UnityEngine;

public class LogicSpinWithMenuMusic : MonoBehaviour
{
    public float Dir = 45;
    bool flipped;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {





    }

    float ConvertTo360Range(float angle)
    {
        return (angle % 360 + 360) % 360;
    }

    void Update()
    {
        flipped = !flipped;

        Dir = 1 * (MainMenuManager.Singleton.averageMusicFreq * 600);

        //Debug.Log($"Dir: {Dir}, flipped: {flipped}, multiplier: {(flipped ? -1 : 1)}, result: {Dir * (flipped ? -1 : 1)}");
        //Debug.Log($"Before Rotate: {transform.eulerAngles.z}");
        transform.eulerAngles = new Vector3(0, 0, Dir * (flipped ? -1 : 1));
        //Debug.Log($"After Rotate: {transform.eulerAngles.z}");
    }

}
