using UnityEngine;

public class OSBProductionInit : OSBInit
{
    public override void Init()
    {
        base.Init();
        Debug.Log("[OSB]: Production build!");
        UIController.OpenMenu(UIMenus.WELCOME_SCREEN, false);
    }
}
