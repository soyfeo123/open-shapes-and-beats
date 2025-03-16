namespace GameJolt.API
{
	public static class Constants
	{
		public const string Version = "2.5.8";

		public const string ApiProtocol = "https://";
		public const string ApiRoot = "api.gamejolt.com/api/game/";
		public const string ApiVersion = "1_2"; // `1_1` actually targets the API version `1.2`..
		public const string ApiBaseUrl = ApiProtocol + ApiRoot + "v" + ApiVersion;

		public const string ApiUsersAuth = "/users/auth";
		public const string ApiUsersFetch = "/users";

		public const string ApiFriendsFetch= "/friends";

		public const string ApiSessionsOpen = "/sessions/open";
		public const string ApiSessionsPing = "/sessions/ping";
		public const string ApiSessionsClose = "/sessions/close";
		public const string ApiSessionsCheck = "/sessions/check";

		public const string ApiScoresAdd = "/scores/add";
		public const string ApiScoresFetch = "/scores";
		public const string ApiScoresRank = "/scores/get-rank";
		public const string ApiScoresTablesFetch = "/scores/tables";

		public const string ApiTrophiesAdd = "/trophies/add-achieved";
		public const string ApiTrophiesRemove = "/trophies/remove-achieved";
		public const string ApiTrophiesFetch = "/trophies";

		public const string ApiDatastoreSet = "/data-store/set";
		public const string ApiDatastoreUpdate = "/data-store/update";
		public const string ApiDatastoreFetch = "/data-store";
		public const string ApiDatastoreRemove = "/data-store/remove";
		public const string ApiDatastoreKeysFetch = "/data-store/get-keys";

		public const string ApiTimeGet = "/time";
	}
}
