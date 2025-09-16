using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        ModifierManager.Singleton.Dummy();
        SettingManager.Singleton.Init();
        SettingMenuManager.ApplyAllSettingsAtOnce();
        SoundManager.Singleton.Init();
        DOTween.Init();
    }
}
