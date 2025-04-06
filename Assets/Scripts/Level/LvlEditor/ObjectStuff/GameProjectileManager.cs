using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameProjectileManager
{
    static GameObject genericProjectile;
    static List<PooledObjectInfo> pools = new List<PooledObjectInfo>();

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        genericProjectile = Resources.Load<GameObject>("Prefabs/LevelEditorPrefabs/Projectiles/GenericProjectile");
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
}

public class PooledObjectInfo
{
    public string id;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}