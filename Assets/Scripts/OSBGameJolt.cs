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

        if(apiInstance is null)
        {
            apiInstance = Instantiate(Resources.Load("Prefabs/GJ/GameJoltAPI") as GameObject).GetComponent<GameJoltAPI>();
        }

        currentGJUser = new User(PlayerPrefs.GetString("gjuser"), PlayerPrefs.GetString("ScreenResXEnc"));

        SignIn(false);
    }

    public void SignIn(string name, string token)
    {
        currentGJUser.Name = name;
        currentGJUser.Token = token;
        SignIn();
    }

    public void SignIn(bool showFailure = true)
    {
        currentGJUser.SignIn(null, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Signed in as " + currentGJUser.Name + "! Did it break? Sure freaking hope not!");
                onSignIn.Invoke();
                currentGJUser.DownloadAvatar();
                MenuNotifications.Singleton.Show(currentGJUser.DeveloperName, " (@" + currentGJUser.Name + ")", "Signed in as");
                PlayerPrefs.SetString("gjuser", currentGJUser.Name);
                PlayerPrefs.SetString("ScreenResXEnc", currentGJUser.Token);

                Trophies.TryUnlock(261658);
            }
            else if(!string.IsNullOrEmpty(currentGJUser.Name) && showFailure)
            {
                MenuNotifications.Singleton.Show("Could not sign in.", "Check your credentials.");
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
