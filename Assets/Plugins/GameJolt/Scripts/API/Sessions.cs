using System;
using System.Collections.Generic;

namespace GameJolt.API {
	/// <summary>
	/// Session statuses.
	/// </summary>
	public enum SessionStatus {
		Active,
		Idle
	}

	/// <summary>
	/// Sessions API methods
	/// </summary>
	public static class Sessions {
		/// <summary>
		/// Open a session (on the GameJolt).
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Open(Action<bool> callback = null) {
			Core.Request.Get(Constants.ApiSessionsOpen, null, response => {
				if(callback != null) {
					callback(response.Success);
				}
			});
		}

		/// <summary>
		/// Ping (i.e. keep alive) a session (on the GameJolt).
		/// </summary>
		/// <param name="status">The <see cref="SessionStatus"/> to set the session to.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Ping(SessionStatus status = SessionStatus.Active, Action<bool> callback = null) {
			var parameters = new Dictionary<string, string> {{"status", status.ToString().ToLower()}};

			Core.Request.Get(Constants.ApiSessionsPing, parameters, response => {
				if(callback != null) {
					callback(response.Success);
				}
			});
		}

		/// <summary>
		/// Close a session (on the GameJolt).
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Close(Action<bool> callback = null) {
			Core.Request.Get(Constants.ApiSessionsClose, null, response => {
				if(callback != null) {
					callback(response.Success);
				}
			});
		}

		/// <summary>
		/// Checks to see if there is an open session for the user. 
		/// Can be used to see if a particular user account is active in the game.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, 
		/// a nullable boolean indicating whether there is an open session or not.
		/// If this argument is null, then there was an error.
		/// </param>
		public static void Check(Action<bool?> callback) {
			if(callback == null) return;
			Core.Request.Get(Constants.ApiSessionsCheck, null, response => {
				// TODO: fix this workaround once GameJolt has fixed the Sessions.Check call.
				// Also see the Response constructor
				if(response.Success)
					callback(true);
				else if(response.Json != null && string.IsNullOrEmpty(response.Json["message"].Value))
					callback(false);
				else
					callback(null);
			});
		}
	}
}
