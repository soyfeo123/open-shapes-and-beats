using UnityEngine;
using System;
using System.Collections.Generic;
using GameJolt.External.SimpleJSON;
using GameJolt.UI;

namespace GameJolt.API.Objects {
	/// <summary>
	/// User types.
	/// </summary>
	public enum UserType {
		Undefined,
		User,
		Developer,
		Moderator,
		Admin
	};

	/// <summary>
	/// User statuses.
	/// </summary>
	public enum UserStatus {
		Undefined,
		Active,
		Banned
	};

	/// <summary>
	/// User objects.
	/// </summary>
	public sealed class User : Base {
		#region Fields & Properties
		private string name = "";
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		/// <remarks>
		/// <para>
		/// Setting the name to a different value (case insensitive)
		/// will cause an authenticated <see cref="User"/> to not be authenticated anymore.
		/// </para>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Name {
			get { return name; }
			set {
				if(name.ToLower() != value.ToLower()) {
					IsAuthenticated = false;
				}
				name = value;
			}
		}

		private string token = "";
		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		/// <value>The token.</value>
		/// <remarks>
		/// <para>
		/// Setting the token to a different value (case insensitive)
		/// will cause an authenticated <see cref="User"/> to not be authenticated anymore.
		/// </para>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Token {
			get { return token; }
			set {
				if(token.ToLower() != value.ToLower()) {
					IsAuthenticated = false;
				}
				token = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="User"/> is authenticated.
		/// </summary>
		/// <value><c>true</c> if this <see cref="User"/> is authenticated; otherwise, <c>false</c>.</value>
		public bool IsAuthenticated { get; private set; }

		/// <summary>
		/// Gets or sets the ID.
		/// </summary>
		/// <value>The ID.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		// ReSharper disable once InconsistentNaming
		public int ID { get; set; }

		/// <summary>
		/// Gets the user type.
		/// </summary>
		/// <value>The user type.</value>
		public UserType Type { get; private set; }

		/// <summary>
		/// Gets the user status.
		/// </summary>
		/// <value>The user status.</value>
		public UserStatus Status { get; private set; }

		/// <summary>
		/// Gets or sets the user avatar URL.
		/// </summary>
		/// <value>The user avatar URL.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		// ReSharper disable once InconsistentNaming
		public string AvatarURL { get; set; }

		/// <summary>
		/// Gets or sets the user avatar.
		/// </summary>
		/// <value>The user avatar.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public Sprite Avatar { get; set; }

		/// <summary>
		/// How long ago the user signed up. 
		/// Example: "1 year ago"
		/// </summary>
		public string SignedUp { get; private set; }

		/// <summary>
		/// The timestamp (in seconds) of when the user signed up. 
		/// Example: 1502471604
		/// </summary>
		public int SignedUpTimestamp { get; private set; }

		/// <summary>
		/// How long ago the user was last logged in. Will be "Online Now" if the user is currently online. 
		/// Example: 2 minutes ago
		/// </summary>
		public string LastLoggedIn { get; private set; }

		/// <summary>
		/// The timestamp (in seconds) of when the user was last logged in. 
		/// Example: 1502471604
		/// </summary>
		public int LastLoggedInTimestamp { get; private set; }

		/// <summary>
		/// Whether this user is currently logged in into Game Jolt.
		/// </summary>
		public bool IsOnline { get { return LastLoggedIn == "Online Now"; } }

		/// <summary>
		/// The user's display name.
		/// </summary>
		public string DeveloperName { get; private set; }

		/// <summary>
		/// The user's website (or empty string if not specified) 
		/// </summary>
		public string DeveloperWebsite { get; private set; }

		/// <summary>
		/// The user's profile description. HTML tags and line breaks will be removed. 
		/// </summary>
		public string DeveloperDescription { get; private set; }

		#endregion Fields & Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="id">The <see cref="User"/> ID.</param>
		public User(int id) {
			IsAuthenticated = false;

			ID = id;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="name">The <see cref="User"/> name.</param>
		/// <param name="token">The <see cref="User"/> token.</param>
		public User(string name, string token) {
			IsAuthenticated = false;

			Name = name;
			Token = token;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="data">API JSON data.</param>
		public User(JSONClass data) {
			IsAuthenticated = false;
			PopulateFromJson(data);
		}
		#endregion Constructors

		#region Update Attributes
		/// <summary>
		/// Map JSON data to the object's attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		protected override void PopulateFromJson(JSONClass data) {
			Name = data["username"].Value;
			ID = data["id"].AsInt;
			AvatarURL = data["avatar_url"].Value;
			SignedUp = data["signed_up"].Value;
			SignedUpTimestamp = data["signed_up_timestamp"].AsInt;
			LastLoggedIn = data["last_logged_in"].Value;
			LastLoggedInTimestamp = data["last_logged_in_timestamp"].AsInt;
			DeveloperName = data["developer_name"].Value;
			DeveloperWebsite = data["developer_website"].Value;
			DeveloperDescription = data["developer_description"].Value;


			try {
				Type = (UserType)Enum.Parse(typeof(UserType), data["type"].Value);
			} catch {
				Type = UserType.Undefined;
			}

			try {
				Status = (UserStatus)Enum.Parse(typeof(UserStatus), data["status"].Value);
			} catch {
				Status = UserStatus.Undefined;
			}
		}
		#endregion Update Attributes

		#region Interface
		/// <summary>
		/// Signs in the user.
		/// </summary>
		/// <param name="signedInCallback">A callback function accepting a single parameter, a boolean indicating whether the user has been signed-in successfully.</param>
		/// <param name="userFetchedCallback">A callback function accepting a single parameter, a boolean indicating whether the user's information have been fetched successfully.</param>
		/// <param name="rememberMe">Whether the user's credentials should be stored in the player prefs.</param>
		public void SignIn(Action<bool> signedInCallback = null, Action<bool> userFetchedCallback = null,
			bool rememberMe = false) {
			if(GameJoltAPI.Instance.HasUser) {
				LogHelper.Warning("Another user is currently signed in. Sign it out first.");

				if(signedInCallback != null) {
					signedInCallback(false);
				}
				if(userFetchedCallback != null) {
					userFetchedCallback(false);
				}

				return;
			}

			var parameters = new Dictionary<string, string> {{"username", Name.ToLower()}, {"user_token", Token.ToLower()}};
			Core.Request.Get(Constants.ApiUsersAuth, parameters, response => {
				IsAuthenticated = response.Success;

				if(response.Success) {
					if(GameJoltAPI.Instance.Settings.AutoSignInOutMessage)
						GameJoltUI.Instance.QueueNotification(string.Format(GameJoltAPI.Instance.Settings.SignInMessage, Name));
					GameJoltAPI.Instance.CurrentUser = this;

					if(rememberMe) {
						GameJoltAPI.Instance.RememberUserCredentials(Name, Token);
					}

					if(signedInCallback != null) {
						signedInCallback(true);
					}

					Get((user) => {
						if(userFetchedCallback != null) {
							userFetchedCallback(user != null);
						}
					});
				} else {
					if(signedInCallback != null) {
						signedInCallback(false);
					}
					if(userFetchedCallback != null) {
						userFetchedCallback(false);
					}
				}
			}, false);
		}

		/// <summary>
		/// Signs out the user.
		/// </summary>
		public void SignOut() {
			if(GameJoltAPI.Instance.CurrentUser == this) {
				if(GameJoltAPI.Instance.Settings.AutoSignInOutMessage)
					GameJoltUI.Instance.QueueNotification(string.Format(GameJoltAPI.Instance.Settings.SignOutMessage, Name));
				GameJoltAPI.Instance.CurrentUser = null;
				GameJoltAPI.Instance.ClearUserCredentials();
			}
		}

		/// <summary>
		/// Get the <see cref="User"/> information.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="User"/>.</param>
		/// <remarks>
		/// <para>
		/// Shortcut for <c>GameJolt.API.Users.Get(this);</c>
		/// </para>
		/// </remarks>
		public void Get(Action<User> callback = null) {
			var me = this;
			Users.Get(me, callback);
		}

		/// <summary>
		/// Downloads the <see cref="User"/> avatar.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		/// <remarks>
		/// <para>
		/// Will set the `Avatar` field on the user. 
		/// </para>
		/// </remarks>
		public void DownloadAvatar(Action<bool> callback = null) {
			if(!string.IsNullOrEmpty(AvatarURL)) {
				Misc.DownloadImage(AvatarURL, avatar => {
					Avatar = avatar ?? GameJoltAPI.Instance.DefaultAvatar;

					if(callback != null) {
						callback(avatar != null);
					}
				});
			} else {
				if(callback != null) {
					callback(false);
				}
			}
		}
		#endregion Interface

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="User"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents the current <see cref="User"/>.</returns>
		public override string ToString() {
			return string.Format("GameJolt.API.Objects.User: {0} - {1} - Authenticated: {2} - Status: {3}", Name, ID,
				IsAuthenticated, Status);
		}
	}
}
