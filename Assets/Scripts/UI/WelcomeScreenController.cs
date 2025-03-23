using DG.Tweening;
using TMPro;
using UnityEngine;

public class WelcomeScreenController : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public CanvasGroup triangleCG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Singleton.PlaySound("Sound/SFX/SFX_STARTUP");

        welcomeText.alpha = 0f;
        welcomeText.DOFade(1f, 0.5f);
        triangleCG.DOFade(1f, 4f);
        welcomeText.transform.DOScale(1.2f, 3.75f).OnComplete(()=>
        {
            UIController.FadeOut(0.2f);
            
            UIController.OpenMenu(UIMenus.MAIN_MENU);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
