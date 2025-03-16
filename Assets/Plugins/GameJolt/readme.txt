Asset Setup
1. Navigate to Plugins/GameJolt and select the GJAPISettings file
2. Edit the settings in your inspector window, you MUST enter your "Game ID" and "Private Key" (these can be obtained from your game's Game Jolt site under: Settings -> Game API -> API Settings).
3. Try out an example scene in order to see if everything works correctly.

For more detailed instructions, have a look at the documentation: https://infectedbytes.github.io/gj-unity-api/


Other settings
Timeout: The time in seconds before an API call should timeout and return failure.
Auto Ping: Automatically create and ping sessions once a user has been authentified.
Use Caching: Cache High Score Tables and Trophies information for faster display.
Encryption Key: The key used to encrypt the user credentials, if the user checks the "Remember Me" option. (The initial password is randomly generated)


Editor only settings
Auto Connect: AutoConnect in the Editor as if the game was hosted on GameJolt.
User: The username to use for AutoConnect.
Token: The token to use for AutoConnect.