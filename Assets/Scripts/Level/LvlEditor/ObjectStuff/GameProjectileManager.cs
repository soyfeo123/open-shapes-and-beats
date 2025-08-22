using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameProjectileManager
{
    static GameObject genericProjectile;
    static GameObject squareProjectile;
    static List<PooledObjectInfo> pools = new List<PooledObjectInfo>();

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        genericProjectile = Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/Projectiles/GenericProjectile");
        squareProjectile = Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/Projectiles/SquareProjectile");
    }

    public static List<GameObject> GetAllActiveProjectiles()
    {
        List<GameObject> activeObjects = new List<GameObject>();

        foreach (var pool in pools)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>()
                .Where(obj => obj.name.StartsWith(pool.id))
                .ToArray();

            foreach (var obj in allObjects)
            {
                if (obj.activeInHierarchy)
                {
                    activeObjects.Add(obj);
                }
            }
        }

        return activeObjects;
    }

    public static void ReturnAllProjectiles()
    {
        foreach(var projectile in GetAllActiveProjectiles())
        {
            GameObject.Destroy(projectile);
        }
    }

    static GameObject SpawnPoolObject(GameObject objectToSpawn)
    {
        PooledObjectInfo pool = pools.Find(p => p.id == objectToSpawn.name);

        if(pool == null)
        {
            pool = new PooledObjectInfo();
            pool.id = objectToSpawn.name;
            pools.Add(pool);
        }

        GameObject spawnobj = pool.inactiveObjects.FirstOrDefault();

        if(spawnobj == null)
        {
            spawnobj = GameObject.Instantiate(objectToSpawn);
        }
        else
        {
            pool.inactiveObjects.Remove(spawnobj);
            spawnobj.SetActive(true);
            
        }

        return spawnobj;
    }

    public static void ReturnProjectileToPool(GameObject obj)
    {
        string g_name = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = pools.Find(p => p.id == g_name);
        if(pool == null)
        {
            Notification.CreateNotification(@"[_<_OOPS... SOMETHING WENT WRONG_>_]
Attempted to release an object that's not pooled.
Contact Palo for more info!", "[enter] awh", new() { { KeyCode.Return, () => { } } });
        }
        else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }

    public static void CreateCircleProjectiles(Vector3 point, int numberOfProjectiles)
    {
        float angle = 360f / (numberOfProjectiles);
        float setRandomValue = Random.Range(-300, 300);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Debug.Log(i + ": " + (angle * i + setRandomValue));
            GameObject projectile = SpawnPoolObject(genericProjectile);
            projectile.transform.position = point;
            projectile.GetComponent<LogicMoveWithDirection>().direction = angle * i + setRandomValue;
        }
    }

    public static void CreateSquareProjectile(Vector3 point, float direction)
    {
        GameObject projectile = SpawnPoolObject(squareProjectile);
        projectile.transform.position = point;
        projectile.GetComponent<LogicMoveWithDirection>().direction = direction;
    }
}

public class PooledObjectInfo
{
    public string id;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}