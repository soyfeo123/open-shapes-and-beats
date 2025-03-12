using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSBInit : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        ThePlayersParents.Singleton.InitPlayerStuffOnBoot();
    }
}
