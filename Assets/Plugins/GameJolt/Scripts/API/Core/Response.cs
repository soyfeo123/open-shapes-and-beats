using UnityEngine;
using GameJolt.External.SimpleJSON;
using UnityEngine.Networking;

namespace GameJolt.API.Core {
	/// <summary>
	/// API Response Formats.
	/// </summary>
	public enum ResponseFormat {
		Dump,
		Json,
		Raw,
		Texture
	}

	/// <summary>
	/// Response object to parse API responses.
	/// </summary>
	public class Response {
		/// <summary>
		/// The Response Format.
		/// </summary>
		public readonly ResponseFormat Format;

		/// <summary>
		/// Whether the response is successful.
		/// </summary>
		public readonly bool Success;

		/// <summary>
		/// The response bytes.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Raw`. 
		/// </para>
		/// </remarks>
		public readonly byte[] Bytes;

		/// <summary>
		/// The response dump.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Dump`.
		/// </para>
		/// </remarks>
		public readonly string Dump;

		/// <summary>
		/// The response JSON.
		/// </summary>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Json`.
		/// </para>
		public readonly JSONNode Json;

		/// <summary>
		/// The response texture.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Texture`. 
		/// </para>
		/// </remarks>
		public readonly Texture2D Texture;

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="errorMessage">Error message.</param>
		public Response(string errorMessage) {
			Success = false;
			LogHelper.Warning(errorMessage);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="response">The API Response.</param>
		/// <param name="format">The format of the response.</param>
		public Response(UnityWebRequest response, ResponseFormat format = ResponseFormat.Json) {
			if(!string.IsNullOrEmpty(response.error)) {
				Success = false;
				LogHelper.Warning(response.error);
				return;
			}

			Format = format;

			switch(format) {
				case ResponseFormat.Dump:
					var text = response.downloadHandler.text;
					Success = text.StartsWith("SUCCESS");
					var returnIndex = text.IndexOf('\n');
					if(returnIndex != -1) {
						Dump = text.Substring(returnIndex + 1);
					}

					if(!Success) {
						LogHelper.Warning(Dump);
						Dump = null;
					}
					break;
				case ResponseFormat.Json:
					Json = JSON.Parse(response.downloadHandler.text)["response"];
					Success = Json["success"].AsBool;
					var msg = Json["message"].Value;
					// success determines whether a request has succeeded or failed, but unfortunatelly
					// currently GameJolt also uses the success variable for the Sessions.Check API.
					// In that case even if success is false, it might still have succeeded and just tells us that 
					// there is no open session. To distuingish these cases we have to also look at the message.
					// In the case of an actually failed request, there must be a message field present. 
					// This field is missing if the call was just a check-call and there was no open session.
					// TODO: remove this workaround when GameJolt has fixed the behavior of Sessions.Check
					// Also see Sessions.Check method.
					if(!Success && !string.IsNullOrEmpty(msg)) {
						LogHelper.Warning(msg);
						Json = null;
					}
					break;
				case ResponseFormat.Raw:
					Success = true;
					Bytes = response.downloadHandler.data;
					break;
				case ResponseFormat.Texture:
					Success = true;
					Texture = ((DownloadHandlerTexture)response.downloadHandler).texture;
					break;
				default:
					Success = false;
					LogHelper.Warning("Unknown format. Cannot process response.");
					break;
			}
		}
	}
}
