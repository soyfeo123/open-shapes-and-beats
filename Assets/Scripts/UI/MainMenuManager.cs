using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Linq;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MBSingletonDestroy<MainMenuManager>
{
    [System.Serializable]
    public class ObjectToPulseWithMusic
    {
        public GameObject obj;
        public float multiplier;
    }

    public Music menuMusic;
    public Music playlistMusic;
    public List<ObjectToPulseWithMusic> objectsToPulse;
    public GameObject Topbar;
    public GameObject MenuRightButtons;
    public Image IntroLogo;
    public Image IntroLogoVisual;
    public CanvasGroup MainMenuContainerGroup;

    [Header("Updating Text References")]
    public TextMeshProUGUI topbarTime;
    public TextMeshProUGUI gamejoltTopbarUser;
    public string signedOutGamejoltUsername;

    [Header("GameJolt Stuff")]
    public GameObject GJPanel;
    public GameObject GJPanel_SignInPanel;
    public GameObject GJPanel_SignedInPanel;
    public Image GJ_Topbar_Icon;
    public TMP_InputField GJPanel_SignInPanel_Username;
    public TMP_InputField GJPanel_SignInPanel_Token;
    bool gjPanelOpened = false;
    public Sprite defaultUser;

    float[] audioSamp = new float[256];



    public void Start()
    {
        PlayRandomSongForMainMenu();
        

        FadeManager.FadeIn(1f);
        MainMenuContainerGroup.alpha = 0;
        MainMenuContainerGroup.interactable = false;
        HideRightButtons();
        Topbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(Topbar.GetComponent<RectTransform>().anchoredPosition.x, 71.3074f);

        defaultUser = Resources.Load<Sprite>("Textures/UI/DefaultUser");
    }

    float GetAverageAmplitude()
    {
        float sum = 0;
        for (int i = 0; i < 40; i++)
        {
            sum += audioSamp[i];
        }
        return (sum / 256) * 2;
    }

    public void PlayRandomSongForMainMenu()
    {
        string supportedExt = "*.mp3,*.wav,*.ogg";
        string[] possibleSongs = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "songs")).Where(s => supportedExt.Contains(Path.GetExtension(s).ToLower())).ToArray();
        int randomI = Random.Range(0, (int)possibleSongs.Length);
        Debug.Log("random: " + randomI + " from " + possibleSongs.Length);
        string selected = possibleSongs[randomI];

        StartCoroutine(LoadSong(selected, () =>
        {
            menuMusic.Play(0, 35);
        }));
    }

    private void Update()
    {
        if (menuMusic.audioSrc.isPlaying)
        {
            menuMusic.audioSrc.GetSpectrumData(audioSamp, 0, FFTWindow.Blackman);
            float average = GetAverageAmplitude();
            foreach (ObjectToPulseWithMusic objectToPulseWithMusic in objectsToPulse)
            {
                objectToPulseWithMusic.obj.transform.localScale = Vector3.one * (1 + average * objectToPulseWithMusic.multiplier);
            }
        }

        topbarTime.text = System.DateTime.Now.ToString("ddd dd MMM  hh:mm tt");
        if (OSBGameJolt.Singleton.currentGJUser.IsAuthenticated)
        {
            gamejoltTopbarUser.text = "<b>" + OSBGameJolt.Singleton.currentGJUser.DeveloperName + "</b>\n<size=17>@" + OSBGameJolt.Singleton.currentGJUser.Name;
            GJPanel_SignedInPanel.SetActive(true);
            GJPanel_SignInPanel.SetActive(false);
            if(OSBGameJolt.Singleton.currentGJUser.Avatar)
            {
                GJ_Topbar_Icon.sprite = OSBGameJolt.Singleton.currentGJUser.Avatar;
            }
        }
        else
        {
            gamejoltTopbarUser.text = signedOutGamejoltUsername;
            GJPanel_SignedInPanel.SetActive(false);
            GJPanel_SignInPanel.SetActive(true);
            GJ_Topbar_Icon.sprite = defaultUser;
        }

#if UNITY_2017_1_OR_NEWER
        /*if (Input.GetKeyDown(KeyCode.Y))
        {
            HideRightButtons();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ShowRightButtons();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("OSB_Debug");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayRandomSongForMainMenu();
            Event_OpenMainMenu();
        }*/
#endif
    }

    IEnumerator LoadSong(string filepath, UnityAction onComplete)
    {
        AudioType type = AudioType.UNKNOWN;

        switch (new FileInfo(filepath).Extension)
        {
            case ".wav":
                type = AudioType.WAV;
                break;
            case ".mp3":
                type = AudioType.MPEG;
                break;
            case ".ogg":
                type = AudioType.OGGVORBIS;
                break;
            default:
                type = AudioType.MPEG;
                break;
        }

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filepath, type);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (menuMusic != null)
            {
                menuMusic.Dispose();
            }

            menuMusic = SoundManager.Singleton.CreateMusic(DownloadHandlerAudioClip.GetContent(request));
            Debug.Log("Loaded " + new FileInfo(filepath).Name + "! You didn't screw it up!");
            onComplete.Invoke();
        }

        else
        {
            Debug.LogError("Error while loading audioclip! " + request.result + " | Message: " + request.error);
        }
    }

    public void TopbarEnter()
    {
        Topbar.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).SetEase(Ease.OutExpo);
    }
    public void TopbarExit()
    {
        Topbar.GetComponent<RectTransform>().DOAnchorPosY(71.3074f, 1f).SetEase(Ease.OutExpo);

        gjPanelOpened = true;
        Event_TopBar_GJUser();
    }

    public void ShowRightButtons()
    {
        MenuRightButtons.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutExpo);
    }
    public void HideRightButtons()
    {
        MenuRightButtons.transform.DOScaleX(0, 0.5f).SetEase(Ease.OutExpo);
    }


    // Button Events

    public void Event_EnterPlaylist()
    {
        menuMusic.FadeOut(()=>
        {
            
        }, 0.5f);
        HideRightButtons();
        TopbarExit();
        MainMenuContainerGroup.DOFade(0f, 0.5f);
        MainMenuContainerGroup.interactable = false;
    }

    public void Event_LevelEditorOpen()
    {
        GameJolt.API.Trophies.TryUnlock(261659);

        menuMusic.FadeOut(() => { }, 0.5f);
        FadeManager.FadeOut(0.5f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor");
        });
    }

    public void Event_OpenMainMenu()
    {
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_BIGSUBMIT);
        IntroLogoVisual.DOFade(0f, 0.5f).SetEase(Ease.InExpo);
        IntroLogo.transform.DOScale(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            MainMenuContainerGroup.DOFade(1f, 0.5f).SetEase(Ease.OutExpo);
            ShowRightButtons();
            TopbarEnter();
        });
        
    }

    public void Event_QuitGame()
    {
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_REJECT);
        HideRightButtons();
        menuMusic.FadeOut(()=> { }, 1f);
        FadeManager.FadeOut(1f, () =>
        {
            Application.Quit();
        });
    }

    public void Event_TopBar_GJUser()
    {
        gjPanelOpened = !gjPanelOpened;
        if (gjPanelOpened)
        {
            GJPanel.transform.DOScaleY(1, 0.2f).SetEase(Ease.OutBack);
            SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_PANEL_OPEN);
        }
        else
        {
            GJPanel.transform.DOScaleY(0, 0.2f).SetEase(Ease.InBack);
            SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_PANEL_CLOSE);
        }
    }

    public void Event_TopBar_GJ_SignIn()
    {
        OSBGameJolt.Singleton.SignIn(GJPanel_SignInPanel_Username.text, GJPanel_SignInPanel_Token.text);
    }

    public void Event_TopBar_GJ_SeeTrophies()
    {
        GameJolt.UI.GameJoltUI.Instance.ShowTrophies();
    }

    public void Event_TopBar_GJ_SignOut()
    {
        OSBGameJolt.Singleton.SignOut();
    }
}
