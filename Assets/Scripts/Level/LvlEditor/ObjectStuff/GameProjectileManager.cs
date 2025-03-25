using UnityEngine;

public static class GameProjectileManager
{
    static GameObject genericProjectile;

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        genericProjectile = Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/Projectiles/GenericProjectile");
    }

    public static void CreateCircleProjectiles(Vector3 point, int numberOfProjectiles)
    {
        float angle = 360f / (numberOfProjectiles);
        float setRandomValue = Random.Range(-300, 300);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Debug.Log(i + ": " + (angle * i + setRandomValue));
            GameObject projectile = GameObject.Instantiate(genericProjectile);
            projectile.transform.position = point;
            projectile.GetComponent<LogicMoveWithDirection>().direction = angle * i + setRandomValue;
        }
    }
}
