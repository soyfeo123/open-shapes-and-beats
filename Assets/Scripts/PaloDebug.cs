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
        UIController.OpenMenu(UIMenus.WELCOME_SCREEN);

        //MainLevelManager.Singleton.LoadLevel("FinishLineTest", new Modifier[] {  });

        //ObzFormat level = new ObzFormat("/Users/palo/Documents/Projects/Big/Open Shapes and Beats/Levels/OSBEditor.obz");
        //level.Import();

        //UIController.OpenMenu(UIMenus.LEVEL_COMPLETE);
    }
}
