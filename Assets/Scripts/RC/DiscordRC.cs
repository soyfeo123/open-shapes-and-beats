using UnityEngine;
using Discord;
using System;

public static class DiscordRC
{
    public static Discord.Discord discordInstance;

    [RuntimeInitializeOnLoadMethod]
    static void init()
    {
        discordInstance = new Discord.Discord(long.Parse(APISecretKey.GetKey("Discord")), (UInt64)Discord.CreateFlags.NoRequireDiscord);
        GameObject behaviour = new GameObject("DiscordRC", typeof(DiscordRCBehaviour));
        GameObject.DontDestroyOnLoad(behaviour);

        Debug.Log("Initialized Discord");
    }
}
