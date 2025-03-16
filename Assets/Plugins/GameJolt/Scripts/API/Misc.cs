using UnityEngine;
using System;
using System.Collections.Generic;

namespace GameJolt.API {
	/// <summary>
	/// Misc API methods.
	/// </summary>
	public static class Misc {
		private static readonly Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>();
		private static readonly Dictionary<string, Action<Sprite>> OpenRequests = new Dictionary<string, Action<Sprite>>();

		/// <summary>
		/// Destroys all sprites in the spritecache.
		/// </summary>
		public static void ClearSpriteCache() {
			foreach(var sprite in SpriteCache.Values) {
				UnityEngine.Object.Destroy(sprite);
			}
			SpriteCache.Clear();
		}

		/// <summary>
		/// Downloads an image.
		/// </summary>
		/// <param name="url">The image URL.</param>
		/// <param name="callback">A callback function accepting a single parameter, a UnityEngine.Sprite.</param>
		public static void DownloadImage(string url, Action<Sprite> callback) {
			// check if the sprite is already cached
			Sprite cachedSprite;
			if(SpriteCache.TryGetValue(url, out cachedSprite)) {
				if(cachedSprite) {
					// check if sprite is not destroyed
					if(callback != null) callback(cachedSprite);
					return;
				}
				// sprite was cached, but it is already destroyed
				SpriteCache.Remove(url);
			}

			// check if there is already an open request
			Action<Sprite> cachedMulticast;
			if(callback != null && OpenRequests.TryGetValue(url, out cachedMulticast)) {
				// there is already a request for that image
				OpenRequests[url] = cachedMulticast + callback; // add callback to multicast
			} else {
				// no open request -> initiate one
				OpenRequests[url] = callback;
				GameJoltAPI.Instance.StartCoroutine(GameJoltAPI.Instance.GetRequest(url, Core.ResponseFormat.Texture, response => {
					Sprite sprite = null;
					if(response.Success) {
						sprite = Sprite.Create(
							response.Texture,
							new Rect(0, 0, response.Texture.width, response.Texture.height),
							new Vector2(.5f, .5f),
							response.Texture.width);
						SpriteCache[url] = sprite;
					}

					Action<Sprite> multicast;
					if(OpenRequests.TryGetValue(url, out multicast)) {
						OpenRequests.Remove(url);
						multicast(sprite);
					}
				}));
			}
		}

		/// <summary>
		/// Get the server time.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a System.DateTime.</param>
		public static void GetTime(Action<DateTime> callback) {
			Core.Request.Get(Constants.ApiTimeGet, null, response => {
				if(callback != null) {
					double timestamp = response.Json["timestamp"].AsDouble;
					DateTime serverTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
					serverTime = serverTime.AddSeconds(timestamp);
					callback(serverTime);
				}
			}, false);
		}
	}
}