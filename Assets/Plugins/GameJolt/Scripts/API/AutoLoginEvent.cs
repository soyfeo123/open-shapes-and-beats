using System;
using UnityEngine.Events;

namespace GameJolt.API {
	/// <summary>
	/// UnityEvent used for the inital auto login.
	/// </summary>
	[Serializable]
	public class AutoLoginEvent : UnityEvent<AutoLoginResult> { }

	/// <summary>
	/// This enum represents the result of an automatic login attempt.
	/// </summary>
	public enum AutoLoginResult {
		/// <summary>
		/// There were no user credentials present.
		/// </summary>
		MissingCredentials,
		/// <summary>
		/// The login failed due to wrong credentials or an network error.
		/// </summary>
		Failed,
		/// <summary>
		/// The user was logged in successfully.
		/// </summary>
		Success
	}
}
