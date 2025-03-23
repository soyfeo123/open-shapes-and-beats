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
    public TextMeshProUGUI introLogoFooter;
    public CanvasGroup MainMenuContainerGroup;
    public GameObject playlistContainer;

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

    [Header("Playlist Stuff")]
    public Transform songContainer;
    public GameObject playlistEntry;
    public List<LvlMetadataV1> playlistSongs = new List<LvlMetadataV1>();


    float[] audioSamp = new float[256];

    [HideInInspector]
    public float averageMusicFreq;

    public void Start()
    {
        menuMusic = new Music();
        PlayRandomSongForMainMenu();


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

        menuMusic.LoadMusic(selected, ()=>
        {
            menuMusic.Play(vol: 35);
        });
    }

    private void Update()
    {
        if (menuMusic.audioSrc.isPlaying)
        {
            menuMusic.audioSrc.GetSpectrumData(audioSamp, 0, FFTWindow.Blackman);
            averageMusicFreq = GetAverageAmplitude();
            foreach (ObjectToPulseWithMusic objectToPulseWithMusic in objectsToPulse)
            {
                objectToPulseWithMusic.obj.transform.localScale = Vector3.one * (1 + averageMusicFreq * objectToPulseWithMusic.multiplier);
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

    

    public void TopbarEnter()
    {
        Topbar.GetComponent<RectTransform>().DOAnchorPosY(0, 1f).SetEase(Ease.OutExpo);
    }
    public void TopbarExit()
    {
        Topbar.GetComponent<RectTransform>().DOAnchorPosY(71.3074f, 1f).SetEase(Ease.OutExpo);

        if(gjPanelOpened)
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

    public void OpenPlaylist()
    {
        playlistSongs.Clear();
        foreach(Transform transform_ in songContainer)
        {
            Destroy(transform_.gameObject);
        }

        foreach(string level in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "levelsserialized")))
        {
            if (new FileInfo(level).Extension != ".txt") continue;
            Debug.Log("Loading " + level);
            LvlMetadataV1 metadata = new LvlMetadataV1();
            metadata.LoadFromString(File.ReadAllText(level));
            if (!metadata.IsValid())
            {
                Notification.CreateNotification("[_<_ERROR WHILE LOADING SONG_>_]\nOne of your levels is either corrupt or not a level.\nCheck your levelsserialized directory!", "[enter] awh, got it", new Dictionary<KeyCode, UnityAction>() { { KeyCode.Return, () => { } } });
            }
            else
            {
                metadata.StartLoad();
                playlistSongs.Add(metadata);
                GameObject playlistEntryLocal = Instantiate(playlistEntry);
                playlistEntryLocal.GetComponent<PlaylistTrackSelection>().metadata = metadata;
                playlistEntryLocal.transform.SetParent(songContainer);
            }


        }

        menuMusic.FadeOut(() =>
        {
            //menuMusic.LoadMusic(Path.Combine(Application.streamingAssetsPath, "songs", "defaultEditorSong.mp3"), ()=> { menuMusic.Play(); });
        }, 0.5f);
        MainMenuContainerGroup.DOFade(0f, 0.5f);
        MainMenuContainerGroup.interactable = false;
        playlistContainer.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.OutExpo);
    }

    public void ClosePlaylist()
    {
        menuMusic.FadeOut(() => { }, 0.5f);
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_REJECT);
        ShowRightButtons();
        TopbarEnter();
        MainMenuContainerGroup.DOFade(1f, 0.5f).OnComplete(()=>
        {
            PlayRandomSongForMainMenu();
        });
        MainMenuContainerGroup.interactable = true;
        playlistContainer.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.5f).SetEase(Ease.OutExpo);
    }


    // Button Events

    public void Event_EnterPlaylist()
    {
        
        HideRightButtons();
        TopbarExit();
        OpenPlaylist();
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

    bool introLogoInAnimation;

    public void Event_OpenMainMenu(bool reject)
    {
        introLogoInAnimation = true;
        SoundManager.Singleton.PlaySound(reject ? LoadedSFXEnum.UI_REJECT : LoadedSFXEnum.UI_BIGSUBMIT);
        introLogoFooter.transform.DOScale(0.5f, 0.5f).SetEase(Ease.InBack);
        introLogoFooter.DOFade(0f, 0.5f).SetEase(Ease.InBack, 0.3f);
        IntroLogoVisual.DOFade(0f, 0.5f).SetEase(Ease.InExpo);
        IntroLogo.transform.DOScale(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            MainMenuContainerGroup.DOFade(1f, 0.5f).SetEase(Ease.OutExpo);
            ShowRightButtons();
            TopbarEnter();
            introLogoInAnimation = false;
        });
        
    }

    public void Event_Hover_IntroLogo()
    {
        if (!introLogoInAnimation)
        {
            IntroLogo.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBounce);
        }
    }
    public void Event_HoverExit_IntroLogo()
    {
        if (!introLogoInAnimation)
        {
            IntroLogo.transform.DOScale(1.3f, 0.3f).SetEase(Ease.OutBounce);
        }
    }

    public void Event_QuitGame()
    {
        SoundManager.Singleton.PlaySound(LoadedSFXEnum.UI_REJECT);
        HideRightButtons();
        menuMusic.FadeOut(()=> { }, 1f);
        FadeManager.FadeOut(1f, () =>
        {
            OSBScenes.QuitGame();
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

    public void Event_NextSong()
    {
        PlayRandomSongForMainMenu();
    }
}
