using GameJolt.API.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace GameJolt.API.Internal {
	/// <summary>
	/// Helper class used to handle Unity version dependant code. 
	/// For example Unity has removed some methods in 2017 and later, which are required for 5.5 and 5.6
	/// </summary>
	internal static class UnityVersionAbstraction {
		public static UnityWebRequest GetTextureRequest(string url) {
#if UNITY_2017_1_OR_NEWER
			return UnityWebRequestTexture.GetTexture(url);
#else
			return UnityWebRequest.GetTexture(url);
#endif
		}

		public static UnityWebRequest GetRequest(string url, ResponseFormat format) {
			return format == ResponseFormat.Texture
				? GetTextureRequest(url)
				: UnityWebRequest.Get(url);
		}

#if !UNITY_2017_2_OR_NEWER
		/// <summary>
		/// Prior to Unity 2017, a WebRequest was transmitted by calling request.Send, from 2017 and onwards
		/// a new method called SendWebRequest was introduced.
		/// </summary>
		public static AsyncOperation SendWebRequest(this UnityWebRequest request) {
			return request.Send();
		}
#endif

#if UNITY_WINRT
		public static byte[] ComputeHash(byte[] bytes) {
			return UnityEngine.Windows.Crypto.ComputeMD5Hash(bytes);
		}
#else
		private static readonly System.Security.Cryptography.MD5 HashAlgorithm =
			new System.Security.Cryptography.MD5CryptoServiceProvider();
		public static byte[] ComputeHash(byte[] bytes) {
			return HashAlgorithm.ComputeHash(bytes);
		}
#endif
	}
}
