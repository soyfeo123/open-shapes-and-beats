using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
// ReSharper disable once RedundantUsingDirective, required for some parts inside an #ifdef
using System.Linq;
using GameJolt.API.Core;
using GameJolt.API.Internal;
using GameJolt.API.Objects;
using GameJolt.External;
using UnityEngine.Networking;
using System.IO;

namespace GameJolt.API {
	/// <summary>
	/// The Core API Manager.
	/// </summary>
	public class GameJoltAPI : MonoSingleton<GameJoltAPI> {
		#region Fields & Properties
		private const string UserCredentialsPreferences = "GJ-API-User-Credentials";

		[Tooltip("The GameJolt API settings.")] public Settings Settings;
		[Tooltip("Event for the autologin mechanism.")] public AutoLoginEvent AutoLoginEvent;
		[Tooltip("The default trophy image which is used if downloading the image failed.")] public Sprite DefaultTrophy;
		[Tooltip("The default avatar image which is used if downloading the image failed.")] public Sprite DefaultAvatar;
		[Tooltip("The default notification icon which is used if you don't provide an image yourself.")]
		public Sprite DefaultNotificationIcon;

		private HashSet<int> secretTrophies;

		private User currentUser;
		/// <summary>
		/// Gets or sets the current user.
		/// </summary>
		/// <value>The current user.</value>
		public User CurrentUser {
			get { return currentUser; }
			set {
				currentUser = value;

				if(currentUser != null) {
					if(currentUser.IsAuthenticated) {
						StartAutoPing();
						CacheTrophies();
					}
				} else {
					StopAutoPing();
				}
			}
		}

		/// <summary>
		/// Checks if there is a user (which must not necessarily be signed in)
		/// </summary>
		public bool HasUser { get { return currentUser != null; } }

		/// <summary>
		/// Returns true if there is a current user and this user is already authenticated.
		/// </summary>
		public bool HasSignedInUser { get { return HasUser && currentUser.IsAuthenticated; } }
		#endregion Fields & Properties

		#region Init
		/// <summary>
		/// Init this instance.
		/// </summary>
		protected override void Init() {
			Configure();
			StartCoroutine(AutoConnect());
			CacheTables();
		}

		/// <summary>
		/// Configure this instance.
		/// </summary>
		private void Configure() {
			if(Settings == null) {
				LogHelper.Error("Missing settings reference! Fallback to empty default settings.");
				Settings = ScriptableObject.CreateInstance<Settings>();
			} else {
				LogHelper.Level = Settings.LogLevel;
				if(Settings.GameId == 0)
					LogHelper.Error("Missing Game ID.");
				if(Settings.PrivateKey == string.Empty)
					LogHelper.Error("Missing Private Key.");
			}
			secretTrophies = new HashSet<int>(Settings.SecretTrophies ?? new int[0]);
		}
		#endregion Init

		#region Requests
		public IEnumerator GetRequest(string url, ResponseFormat format, Action<Response> callback) {
			if(Settings.GameId == 0 || Settings.PrivateKey == null) {
				callback(new Response("Bad Credentials"));
				yield break;
			}

			float timeout = Time.time + Settings.Timeout;
			using(var request = UnityVersionAbstraction.GetRequest(url, format)) {
				request.SendWebRequest();
				while(!request.isDone) {
					if(Time.time > timeout) {
						request.Abort();
						callback(new Response("Timeout for " + url));
						yield break;
					}
					yield return new WaitForEndOfFrame();
				}
				callback(new Response(request, format));
			}
		}

		public IEnumerator PostRequest(string url, Dictionary<string, string> payload, ResponseFormat format,
			Action<Response> callback) {
			if(Settings.GameId == 0 || Settings.PrivateKey == null) {
				callback(new Response("Bad Credentials"));
				yield break;
			}

			float timeout = Time.time + Settings.Timeout;

			using(var request = UnityWebRequest.Post(url, payload)) {
				request.SendWebRequest();
				while(!request.isDone) {
					if(Time.time > timeout) {
						request.Abort();
						callback(new Response("Timeout for " + url));
						yield break;
					}
					yield return new WaitForEndOfFrame();
				}

				callback(new Response(request, format));
			}
		}
		#endregion Requests

		#region Actions
		private IEnumerator AutoConnect() {
			yield return null; // delay by one frame to make sure that the whole UI is already initialized
#if UNITY_WEBGL
			#region Autoconnect Web
#if UNITY_EDITOR
			if(Settings.DebugAutoConnect) {
				if(Settings.DebugUser != string.Empty && Settings.DebugToken != string.Empty) {
					var user = new User(Settings.DebugUser, Settings.DebugToken);
					user.SignIn(success => {
						LogHelper.Info("AutoConnect user '{0}': {1}", user.Name, success ? "success" : "failed");
					});
				} else {
					LogHelper.Warning("Cannot simulate WebPlayer AutoConnect. Missing user and/or token in debug settings.");
				}
			}
#else
			var user = GetUserFromUrl();
			if(user != null) {
				user.SignIn(success => AutoLoginEvent.Invoke(success ? AutoLoginResult.Success : AutoLoginResult.Failed));
			} else {
				LogHelper.Info("Cannot AutoConnect.");
				AutoLoginEvent.Invoke(AutoLoginResult.MissingCredentials);
			}
#endif

			#endregion
#else

			#region Autoconnect Non Web
			// if started via the GameJolt app, then the credentials are placed next to our game
			foreach(var path in new string[] { ".gj-credentials", "../.gj-credentials", "../../.gj-credentials" }) {
				try {
					if(File.Exists(path)) {
						var lines = File.ReadAllLines(path);
						// file content: version \n username \n token
						var user = new User(lines[1], lines[2]);
						user.SignIn(success => AutoLoginEvent.Invoke(success ? AutoLoginResult.Success : AutoLoginResult.Failed));
						yield break;
					}
				} catch(Exception ex) {
					Debug.LogException(ex);
				}
			}
			string username, token;
			if(GetStoredUserCredentials(out username, out token)) {
				var user = new User(username, token);
				user.SignIn(success => AutoLoginEvent.Invoke(success ? AutoLoginResult.Success : AutoLoginResult.Failed));
			} else {
				AutoLoginEvent.Invoke(AutoLoginResult.MissingCredentials);
			}
			#endregion

#endif
		}

#if UNITY_WEBGL && !UNITY_EDITOR
		/// <summary>
		/// Helper method used to parse the user credentials from the webgl url.
		/// </summary>
		/// <returns></returns>
		private User GetUserFromUrl() {
			var uri = new Uri(Application.absoluteURL);
			if(uri.Host.EndsWith("gamejolt.net") || uri.Host.EndsWith("gamejolt.com")) {
				var args = uri.Query.Split('?', '&') // split all arguments
					.Where(x => x.Contains('=')) // ignore invalid arguments
					.Select(x => x.Split('=')) // split into key and value
					.ToLookup(x => x[0], x => x[1]); // ToLookup instead of ToDictionary, because the url may contain duplciate keys
				if(args.Contains("gjapi_username") && args.Contains("gjapi_token")) {
					string username = args["gjapi_username"].First();
					string token = args["gjapi_token"].First();
					if(username != "" && token != "") {
						return new User(username, token);
					}
				}
			} else {
				LogHelper.Warning("Cannot AutoConnect, the game is not hosted on GameJolt.");
			}
			return null;
		}
#endif

		private void StartAutoPing() {
			if(!Settings.AutoPing) {
				return;
			}

			Sessions.Open(success => {
				// What should we do if it fails? Retry later?
				// Without smart handling, it will probably just fail again...
				if(success) {
					Invoke("Ping", 30f);
				}
			});
		}

		private void Ping() {
			Sessions.Ping(SessionStatus.Active, success => {
				// Sessions are automatically closed after 120 seconds
				// which will happen if the application has been in the background for too long.
				// It would be nice to Ping an Idle state when the app is in the background,
				// but because Unity apps don't run in the background by default, this is doomed to failure.
				// Let it error out and reconnect.
				if(!success) {
					Invoke("StartAutoPing", 1f); // Try reconnecting.
				} else {
					Invoke("Ping", 30f); // Ping again.
				}
			});
		}

		private void StopAutoPing() {
			if(Settings.AutoPing) {
				CancelInvoke("StartAutoPing");
				CancelInvoke("Ping");
			}
		}

		private void CacheTables() {
			if(Settings.UseCaching) {
				Scores.GetTables(null);
			}
		}

		private void CacheTrophies() {
			if(Settings.UseCaching) {
				Trophies.Get(trophies => {
					if(trophies != null) {
						foreach(Trophy trophy in trophies) {
							trophy.DownloadImage();
						}
					}
				});
			}
		}
		#endregion Actions

		#region Helper
		/// <summary>
		/// If the user's credentials are stored in PlayerPrefs, this method will retrieve them.
		/// </summary>
		/// <param name="username">Contains the username if retrieval was successfull, empty string otherwise.</param>
		/// <param name="token">Contains the token if retrieval was successfull, empty string otherwise.</param>
		/// <returns>Whether retrieval was successfull or not.</returns>
		public bool GetStoredUserCredentials(out string username, out string token) {
			username = token = "";
			if(string.IsNullOrEmpty(UserCredentialsPreferences) || string.IsNullOrEmpty(Settings.EncryptionKey) ||
			   !PlayerPrefs.HasKey(UserCredentialsPreferences)) return false;
			var credentials = PlayerPrefs.GetString(UserCredentialsPreferences).Split('#');
			if(credentials.Length != 2) return false;
			try {
				username = XTEA.Decrypt(credentials[0], Settings.EncryptionKey);
				token = XTEA.Decrypt(credentials[1], Settings.EncryptionKey);
				return true;
			} catch {
				LogHelper.Warning("Failed to retrieve user credentials.");
				return false;
			}
		}

		/// <summary>
		/// Stores the user's credentials in PlayerPrefs.
		/// </summary>
		/// <param name="username">The username to store.</param>
		/// <param name="token">The token to store.</param>
		/// <returns>Whether the operations was successfull.</returns>
		public bool RememberUserCredentials(string username, string token) {
			if(string.IsNullOrEmpty(UserCredentialsPreferences) || string.IsNullOrEmpty(Settings.EncryptionKey) ||
			   string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token)) return false;
			var credentials = XTEA.Encrypt(username, Settings.EncryptionKey) + "#" + XTEA.Encrypt(token, Settings.EncryptionKey);
			PlayerPrefs.SetString(UserCredentialsPreferences, credentials);
			PlayerPrefs.Save();
			return true;
		}

		/// <summary>
		/// Clears the stored credentials.
		/// </summary>
		public void ClearUserCredentials() {
			if(string.IsNullOrEmpty(UserCredentialsPreferences)) return;
			PlayerPrefs.DeleteKey(UserCredentialsPreferences);
			PlayerPrefs.Save();
		}

		/// <summary>
		/// Returns true if the Settings.SecretTrophies setting contains the provided trophy id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool IsSecretTrophy(int id) {
			return secretTrophies.Contains(id);
		}
		#endregion
	}
}