using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public static class UIController
{
    public static GameObject currentMenu;

    public static void OpenMenu(GameObject menu, bool overrideCurrentMenu = false)
    {
        Debug.Log("Checking menus");
        if (currentMenu != null)
        {
            if (overrideCurrentMenu)
                GameObject.Destroy(currentMenu);
            else
                return;
        }
        Debug.Log("Opening menu " + menu);
        currentMenu = GameObject.Instantiate(menu);
        Debug.Log(currentMenu);
    }

    public static void FadeOut(float time = 0.5f, UnityAction action = null)
    {
        CanvasGroup cg = currentMenu.AddComponent<CanvasGroup>();
        GameObject menuObj = currentMenu;
        currentMenu = null;
        cg.DOFade(0, time).OnComplete(()=>
        {
            GameObject.Destroy(menuObj);
            action?.Invoke();
        });
    }
}


public static class UIMenus
{
    public static GameObject MAIN_MENU;
    public static GameObject WELCOME_SCREEN;
    public static GameObject LEVEL_COMPLETE;
    
    [RuntimeInitializeOnLoadMethod]
    static void InitResources()
    {
        Debug.Log("[OSB Enum]: Init UIMenus resources");

        // for gosh's sake i will throw my cat out of my window if this doesn't work
        MAIN_MENU = Resources.Load<GameObject>("Prefabs/OSBMenu");
        WELCOME_SCREEN = Resources.Load<GameObject>("Prefabs/Welcome");
        LEVEL_COMPLETE = Resources.Load<GameObject>("Prefabs/LevelCompleteScreen");
    }
}
