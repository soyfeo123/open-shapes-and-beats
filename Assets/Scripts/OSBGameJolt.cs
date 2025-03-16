using UnityEngine;
using GameJolt.API;
using GameJolt.API.Objects;
using UnityEngine.Events;

public class OSBGameJolt : MBSingleton<OSBGameJolt>
{
    public GameJoltAPI apiInstance;
    public User currentGJUser;
    public UnityEvent onSignIn = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();

        onSignIn.AddListener(() =>
        {
            // bandade fix so unity doesn't go crying
        });

        if(apiInstance == null)
        {
            apiInstance = Instantiate(Resources.Load("Prefabs/GJ/GameJoltAPI") as GameObject).GetComponent<GameJoltAPI>();
        }

        currentGJUser = new User(PlayerPrefs.GetString("gjuser"), PlayerPrefs.GetString("ScreenResXEnc"));

        SignIn();
    }

    public void SignIn(string name, string token)
    {
        currentGJUser.Name = name;
        currentGJUser.Token = token;
        SignIn();
    }

    public void SignIn()
    {
        currentGJUser.SignIn(null, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Signed in as " + currentGJUser.Name + "! Did it break? Sure freaking hope not!");
                onSignIn.Invoke();
                currentGJUser.DownloadAvatar();
                GameJolt.UI.GameJoltUI.Instance.QueueNotification("Signed in as @" + currentGJUser.Name + " (" + currentGJUser.DeveloperName + ")");
                PlayerPrefs.SetString("gjuser", currentGJUser.Name);
                PlayerPrefs.SetString("ScreenResXEnc", currentGJUser.Token);

                Trophies.TryUnlock(261658);
            }
            else if(!string.IsNullOrEmpty(currentGJUser.Name))
            {
                GameJolt.UI.GameJoltUI.Instance.QueueNotification("Could not sign-in. Check your credentials and use your token.");
            }
        });
    }

    public void SignOut()
    {
        currentGJUser.SignOut();
        currentGJUser = new User("", "");
        currentGJUser.Avatar = null;
        PlayerPrefs.SetString("gjuser", "");
        PlayerPrefs.SetString("ScreenResXEnc", "");
    }
}
