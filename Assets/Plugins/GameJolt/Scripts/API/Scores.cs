using System;
using System.Collections.Generic;
using GameJolt.API.Objects;

namespace GameJolt.API {
	/// <summary>
	/// Scores API methods
	/// </summary>
	public static class Scores {
		/// <summary>
		/// The cached high score tables.
		/// </summary>
		private static Table[] cachedTables;

		#region Add
		/// <summary>
		/// Add a <see cref="Score"/>.
		/// </summary>
		/// <param name="score">The <see cref="Score"/> to add.</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Add(Score score, int table = 0, Action<bool> callback = null) {
			Add(score.Value, score.Text, score.GuestName, table, score.Extra, callback);
		}

		/// <summary>
		/// Add a <see cref="Score"/>.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="text">The textual value of the score (i.e. "123 Jumps").</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="extra">Extra logging information. Defaults to an empty string.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Add(int value, string text, int table = 0, string extra = "", Action<bool> callback = null) {
			Add(value, text, "", table, extra, callback);
		}

		/// <summary>
		/// Add a score <see cref="Score"/>.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="text">The textual value of the score (i.e. "123 Jumps").</param>
		/// <param name="guestName">The guest name if guest score. Leave blank otherwise.</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="extra">Extra logging information. Defaults to an empty string.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success..</param>
		public static void Add(int value, string text, string guestName, int table = 0, string extra = "",
			Action<bool> callback = null) {
			var guestScore = !string.IsNullOrEmpty(guestName);

			var parameters = new Dictionary<string, string> {{"sort", value.ToString()}, {"score", text}};
			if(guestScore) {
				parameters.Add("guest", guestName);
			}
			if(table != 0) {
				parameters.Add("table_id", table.ToString());
			}
			if(!string.IsNullOrEmpty(extra)) {
				parameters.Add("extra_data", extra);
			}

			Core.Request.Get(Constants.ApiScoresAdd, parameters, response => {
				if(callback != null) {
					callback(response.Success);
				}
			}, !guestScore);
		}
		#endregion Add

		#region Get
		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		/// <param name="currentUserOnly">If set to <c>true</c> only return scores of the current user; otherwise for all the users.</param>
		public static void Get(Action<Score[]> callback, int table = 0, int limit = 10, bool currentUserOnly = false) {
			Get(callback, table, limit, currentUserOnly, "", null, null);
		}

		/// <summary>
		/// Get the specified callback, table, limit and guest.
		/// </summary>
		/// <param name="guest">Only return scores of the given guest.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		public static void Get(string guest, Action<Score[]> callback, int table = 0, int limit = 10) {
			Get(callback, table, limit, false, guest, null, null);
		}

		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="score">Retrieve only scores better than this one.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		/// <param name="currentUserOnly">If set to <c>true</c> only return scores of the current user; otherwise for all the users.</param>
		public static void GetBetterThan(int score, Action<Score[]> callback, int table = 0, int limit = 10, bool currentUserOnly = false) {
			Get(callback, table, limit, currentUserOnly, "", score, null);
		}

		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="guest">Only return scores of the given guest.</param>
		/// <param name="score">Retrieve only scores better than this one.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		public static void GetBetterThan(string guest, int score, Action<Score[]> callback,int table = 0, int limit = 10) {
			Get(callback, table, limit, false, guest, score, null);
		}

		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="score">Retrieve only scores worse than this one.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		/// <param name="currentUserOnly">If set to <c>true</c> only return scores of the current user; otherwise for all the users.</param>
		public static void GetWorseThan(int score, Action<Score[]> callback, int table = 0, int limit = 10, bool currentUserOnly = false) {
			Get(callback, table, limit, currentUserOnly, "", null, score);
		}

		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="guest">Only return scores of the given guest.</param>
		/// <param name="score">Retrieve only scores worse than this one.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="Score"/>s to return. Defaults to 10.</param>
		public static void GetWorseThan(string guest, int score, Action<Score[]> callback, int table = 0, int limit = 10) {
			Get(callback, table, limit, false, guest, null, score);
		}

		private static void Get(Action<Score[]> callback, int table, int limit, bool currentUserOnly, string guest,
			int? betterThan, int? worseThan) {
			var parameters = new Dictionary<string, string>();
			if(table != 0) parameters.Add("table_id", table.ToString());
			if(betterThan != null) parameters.Add("better_than", betterThan.Value.ToString());
			if(worseThan != null) parameters.Add("worse_than", worseThan.Value.ToString());
			parameters.Add("limit", Math.Max(1, limit).ToString());

			// If currentUserOnly is requested, it takes precedence over guest settings.
			// (it might be better to throw an exception to let the devs know they are doing something weird)
			if (!currentUserOnly && guest != "") {
				parameters.Add("guest", guest);
			}

			Core.Request.Get(Constants.ApiScoresFetch, parameters, response => {
				Score[] scores;
				if(response.Success) {
					int count = response.Json["scores"].AsArray.Count;
					scores = new Score[count];

					for(int i = 0; i < count; ++i) {
						scores[i] = new Score(response.Json["scores"][i].AsObject);
					}
				} else {
					scores = null;
				}

				if(callback != null) {
					callback(scores);
				}
			}, currentUserOnly);
		}
		#endregion Get

		#region GetRank
		/// <summary>
		/// Get the score's rank.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="callback">A callback function accepting a single parameter, an int.</param>
		public static void GetRank(int value, int table = 0, Action<int> callback = null) {
			var parameters = new Dictionary<string, string>();
			if(table != 0) {
				parameters.Add("table_id", table.ToString());
			}
			parameters.Add("sort", value.ToString());

			Core.Request.Get(Constants.ApiScoresRank, parameters, response => {
				if(callback != null) {
					var rank = response.Success ? response.Json["rank"].AsInt : 0;
					callback(rank);
				}
			}, false);
		}
		#endregion GetRank

		#region GetTables
		/// <summary>
		/// Get the high score tables.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of HighScore <see cref="Table"/>s.</param>
		public static void GetTables(Action<Table[]> callback) {
			if(cachedTables != null) {
				if(callback != null) {
					callback(cachedTables);
				}
			} else {
				Core.Request.Get(Constants.ApiScoresTablesFetch, null, response => {
					Table[] tables;
					if(response.Success) {
						int count = response.Json["tables"].AsArray.Count;
						tables = new Table[count];

						for(int i = 0; i < count; ++i) {
							tables[i] = new Table(response.Json["tables"][i].AsObject);
						}
					} else {
						tables = null;
					}

					if(GameJoltAPI.Instance.Settings.UseCaching) {
						cachedTables = tables;
					}

					if(callback != null) {
						callback(tables);
					}
				}, false);
			}
		}
		#endregion GetTables
	}
}
