using System;
using GameJolt.API.Core;
using GameJolt.API.Objects;

namespace GameJolt.API {
	/// <summary>
	/// Friends API methods.
	/// </summary>
	public static class Friends {
		/// <summary>
		/// Returns the list of a user's friends.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of user ids.
		/// In case of an error, the array is null.
		/// </param>
		public static void GetIds(Action<int[]> callback) {
			if(callback == null) return;
			Request.Get(Constants.ApiFriendsFetch, null, response => {
				int[] ids = null;
				if(response.Success) {
					var friends = response.Json["friends"].AsArray;
					ids = new int[friends.Count];
					for(int i = 0; i < friends.Count; i++)
						ids[i] = friends[i]["friend_id"].AsInt;
				}

				callback(ids);
			});
		}

		/// <summary>
		/// Returns the list of a user's friends.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of users.
		/// In case of an error, the array is null.
		/// </param>
		public static void Get(Action<User[]> callback) {
			if(callback == null) return;
			GetIds(ids => {
				if(ids == null) callback(null); // query failed
				else Users.Get(ids, callback); // get user infos
			});
		}
	}
}
