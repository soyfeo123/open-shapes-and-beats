using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSingleton<T> : MonoBehaviour where T : MBSingleton<T>
{
    private static T mb_instance;
    public static T Singleton
    {
        get
        {
            if(mb_instance == null)
            {
                SetInstance(CreateInstance());
            }
            return mb_instance;
        }
    }
    public static bool HasInstance
    {
        get
        {
            return mb_instance != null;
        }
    }

    private static void SetInstance(T instance)
    {
        mb_instance = instance;
        DontDestroyOnLoad(instance.gameObject);
    }

    /// <summary>
    /// TODO: please use base.Awake() when using override Awake() for it to work properly
    /// </summary>
    protected virtual void Awake()
    {
        SetInstance(this as T);
        
    }

    static T CreateInstance() => new GameObject($"{typeof(T).Name}(AutoCreated)").AddComponent<T>();
}

public class MBSingletonDestroy<T> : MonoBehaviour where T : MBSingletonDestroy<T>
{
    private static T mb_instance;
    public static bool HasInstance
    {
        get
        {
            return mb_instance != null;
        }
    }
    public static T Singleton
    {
        get
        {
            if (mb_instance == null)
            {
                //SetInstance(CreateInstance());
                Debug.LogError("NO INSTANCE APPLIED ON SINGLETON!");
                return null;
            }
            return mb_instance;
        }
    }

    private static void SetInstance(T instance)
    {
        mb_instance = instance;
    }

    /// <summary>
    /// TODO: please use base.Awake() when using override Awake() for it to work properly
    /// </summary>
    protected virtual void Awake()
    {
        SetInstance(this as T);

    }

    public void Destroy()
    {
        if (mb_instance)
        {
            GameObject.Destroy(mb_instance.gameObject);
        }
    }

    //static T CreateInstance() => new GameObject($"{typeof(T).Name}(AutoCreated)", typeof(T)).GetComponent<T>();
}
