using UnityEngine;

public class LogicMoveWithDirection : MonoBehaviour
{
    public float direction;
    float innerDirection;
    public float speed = 9f;
    public bool isPooled = false;

    private void Awake()
    {
        OSBLevelEditorStaticValues.onStop.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    void Update()
    {
        transform.position += new Vector3(Mathf.Sin(direction * Mathf.Deg2Rad) * speed * OSBLevelEditorStaticValues.deltaTime, Mathf.Cos(direction * Mathf.Deg2Rad) * speed * OSBLevelEditorStaticValues.deltaTime);

        if (transform.position.x > 10.5f || transform.position.x < -10.5f || transform.position.y > 6 || transform.position.y < -6)
        {
            if (isPooled)
                GameProjectileManager.ReturnProjectileToPool(gameObject);
            else
                Destroy(this.gameObject);
        }
    }
}