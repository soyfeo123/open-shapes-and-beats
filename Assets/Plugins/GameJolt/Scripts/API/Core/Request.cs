using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameJolt.API.Internal;

namespace GameJolt.API.Core {
	/// <summary>
	/// Request object to send API calls.
	/// </summary>
	public static class Request {
		/// <summary>
		/// Make a GET request
		/// </summary>
		/// <param name="method">The API endpoint.</param>
		/// <param name="parameters">The API parameters.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="Response"/>.</param>
		/// <param name="requireVerified">Whether a signed in user is required <c>true</c> or not <c>false</c>.</param>
		/// <param name="format">The <see cref="ResponseFormat"/> to receive the <see cref="Response"/> in.</param>
		public static void Get(
			string method,
			Dictionary<string, string> parameters,
			Action<Response> callback,
			bool requireVerified = true,
			ResponseFormat format = ResponseFormat.Json) {
			var error = Prepare(ref parameters, requireVerified, format);
			if(error != null) {
				callback(new Response(error));
			} else {
				var url = GetRequestUrl(method, parameters, null);
				GameJoltAPI.Instance.StartCoroutine(GameJoltAPI.Instance.GetRequest(url, format, callback));
			}
		}

		/// <summary>
		/// Make a POST request
		/// </summary>
		/// <param name="method">The API endpoint.</param>
		/// <param name="parameters">The API parameters.</param>
		/// <param name="payload">The API body payload.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="Response"/>.</param>
		/// <param name="requireVerified">Whether a signed in user is required <c>true</c> or not <c>false</c>.</param>
		/// <param name="format">The <see cref="ResponseFormat"/> to receive the <see cref="Response"/> in.</param>
		public static void Post(
			string method,
			Dictionary<string, string> parameters,
			Dictionary<string, string> payload,
			Action<Response> callback,
			bool requireVerified = true,
			ResponseFormat format = ResponseFormat.Json) {
			var error = Prepare(ref parameters, requireVerified, format);
			if(error != null) {
				callback(new Response(error));
			} else {
				var url = GetRequestUrl(method, parameters, payload);
				GameJoltAPI.Instance.StartCoroutine(GameJoltAPI.Instance.PostRequest(url, payload, format, callback));
			}
		}

		/// <summary>
		/// Populate the API parameters with common attributes.
		/// </summary>
		/// <param name="parameters">The API parameters.</param>
		/// <param name="requireVerified">Whether a signed in user is required <c>true</c> or not <c>false</c>.</param>
		/// <param name="format">The <see cref="ResponseFormat"/> to receive the <see cref="Response"/> in.</param>
		private static string Prepare(ref Dictionary<string, string> parameters, bool requireVerified, ResponseFormat format) {
			if(parameters == null) {
				parameters = new Dictionary<string, string>();
			}

			if(requireVerified) {
				if(!GameJoltAPI.Instance.HasSignedInUser) {
					return "Missing Authenticated User.";
				}
				parameters.Add("username", GameJoltAPI.Instance.CurrentUser.Name.ToLower());
				parameters.Add("user_token", GameJoltAPI.Instance.CurrentUser.Token.ToLower());
			}

			parameters.Add("format", format.ToString().ToLower());

			return null;
		}

		/// <summary>
		/// Gets the formatted request URL.
		/// </summary>
		/// <returns>The formatted request UR.</returns>
		/// <param name="method">The API endpoint.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="payload">Payload used for POST requests.</param>
		private static string GetRequestUrl(string method, Dictionary<string, string> parameters, Dictionary<string, string> payload) {
			StringBuilder url = new StringBuilder();
			url.Append(Constants.ApiBaseUrl);
			url.Append(method);
			url.Append("?game_id=");
			url.Append(GameJoltAPI.Instance.Settings.GameId);

			foreach(KeyValuePair<string, string> parameter in parameters) {
				url.Append("&");
				url.Append(parameter.Key);
				url.Append("=");
				url.Append(parameter.Value.Replace(" ", "%20"));
			}

			var sb = new StringBuilder();
			sb.Append(url);
			if(payload != null) {
				foreach(var pair in payload.OrderBy(x => x.Key)) {
					sb.Append(pair.Key);
					sb.Append(pair.Value);
				}
			}
			
			string signature = GetSignature(sb.ToString());
			url.Append("&signature=");
			url.Append(signature);

			return url.ToString();
		}

		/// <summary>
		/// Gets the API call signature.
		/// </summary>
		/// <returns>The API call signature.</returns>
		/// <param name="input">The formatted request URL.</param>
		private static string GetSignature(string input) {
			return Md5(input + GameJoltAPI.Instance.Settings.PrivateKey);
		}

		/// <summary>
		/// Make MD5 Hash.
		/// </summary>
		/// <returns>The MD5 Hash.</returns>
		/// <param name="input">Input.</param>
		private static string Md5(string input) {
			var bytes = Encoding.UTF8.GetBytes(input);
			var hashBytes = UnityVersionAbstraction.ComputeHash(bytes);
			var hashString = new StringBuilder();
			foreach(byte b in hashBytes) {
				hashString.Append(b.ToString("x2").ToLower());
			}
			return hashString.ToString().PadLeft(32, '0');
		}
	}
}