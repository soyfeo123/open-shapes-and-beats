using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;

public class PaloDebug : OSBInit
{
    public override void Init()
    {
        base.Init();

        //MainLevelManager.Singleton.LoadLevel(MainGameLevels.I_SAID_MEOW);
        //UIController.OpenMenu(UIMenus.MAIN_MENU);
        //UIController.OpenMenu(UIMenus.WELCOME_SCREEN);

        MainLevelManager.Singleton.LoadLevel("OSBEditor");

        
    }
}
