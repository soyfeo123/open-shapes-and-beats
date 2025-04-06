using UnityEngine;

public class DiscordRCBehaviour : MonoBehaviour
{
    void Update()
    {
        DiscordRC.discordInstance.RunCallbacks();
    }
}
