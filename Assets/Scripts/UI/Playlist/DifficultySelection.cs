using DG.Tweening;
using TMPro;
using UnityEngine;

public static class DifficultySelection
{
    public static void Open(string levelToLoad)
    {
        GameObject instance = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/DifficultySelection"));
        CanvasGroup group = instance.AddComponent<CanvasGroup>();
        group.alpha = 0;
        group.DOFade(1f, 0.1f);

        RectTransform mainWindow = instance.transform.Find("Bg/MainWindow").GetComponent<RectTransform>();
        TextMeshProUGUI windowTitle = instance.transform.Find("Bg/MainWindow/Container/Title").GetComponent<TextMeshProUGUI>();
        CanvasGroup buttonListGroup = instance.transform.Find("Bg/MainWindow/ButtonList").gameObject.AddComponent<CanvasGroup>();
        buttonListGroup.alpha = 0f;
        windowTitle.alpha = 0f;
        float defaultSizeY = mainWindow.sizeDelta.y;
        mainWindow.sizeDelta = new Vector2(mainWindow.sizeDelta.x, 0);

        Utils.Timer(0.3f, () =>
        {
            mainWindow.DOSizeDelta(new Vector2(mainWindow.sizeDelta.x, defaultSizeY), 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                windowTitle.DOFade(1f, 0.2f);
                buttonListGroup.DOFade(1f, 0.2f);

                LogicEventToKey.Add(instance.transform.Find("Bg/MainWindow/Keybinds").gameObject, KeyCode.Escape, () =>
                {
                    GameObject.Destroy(instance);
                });
            });
            
        });

        instance.transform.Find("Bg/MainWindow/ButtonList/Normal/Button").GetComponent<DifficultySelectionButton>().onClick.AddListener(() =>
        {
            MainLevelManager.Singleton.currentLevelMode = LevelMode.Normal;
            OpenLevel(levelToLoad, instance);
        });

        instance.transform.Find("Bg/MainWindow/ButtonList/Zen/Button").GetComponent<DifficultySelectionButton>().onClick.AddListener(() =>
        {
            MainLevelManager.Singleton.currentLevelMode = LevelMode.ZenMode;
            OpenLevel(levelToLoad, instance);
        });
    }

    static void OpenLevel(string levelToLoad, GameObject instance)
    {
        CanvasGroup group = instance.GetComponent<CanvasGroup>();

        group.DOFade(0, 0.25f);
        Camera.main.backgroundColor = Color.black;

        MainMenuManager.Singleton.RemoveBackground();
        MainMenuManager.Singleton.menuMusic.FadeOut(() => { }, 0.25f);
        UIController.FadeOut(0.25f, () =>
        {
            MainLevelManager.Singleton.LoadLevel(levelToLoad, ModifierManager.Singleton.GetActiveModifiers());
            GameObject.Destroy(instance);
        });
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_SONGSUBMIT);
    }
}
