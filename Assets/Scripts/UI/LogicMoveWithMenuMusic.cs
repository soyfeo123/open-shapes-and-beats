using UnityEngine;

public class LogicMoveWithMenuMusic : MonoBehaviour
{
    bool flipped;
    Vector3 orgPosition;
    public float multiplier = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orgPosition = transform.localPosition;
    }

    void Update()
    {
        if (!MainMenuManager.HasInstance) return;
        flipped = !flipped;

        float y = 1 * (1 + MainMenuManager.Singleton.averageMusicFreq * 1500);

        transform.localPosition = orgPosition + new Vector3(0, y * multiplier, 0);
    }
}
