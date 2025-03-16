using UnityEngine;

// ReSharper disable UnusedMember.Global
namespace GameJolt.API {
	/// <summary>
	/// Static helper class for logging.
	/// </summary>
	public static class LogHelper {
		/// <summary>
		/// All messages below this level are discarded.
		/// This value is automatically set by the <see cref="GameJoltAPI"/> class.
		/// </summary>
		public static LogLevel Level = LogLevel.Warning;
		
		public static void Info(object obj) {
			if(Level > LogLevel.Info) return;
			Debug.Log(obj);
		}

		public static void Info(string format, params object[] args) {
			if(Level > LogLevel.Info) return;
			Debug.Log(string.Format(format, args));
		}

		public static void Warning(object obj) {
			if(Level > LogLevel.Warning) return;
			Debug.LogWarning(obj);
		}

		public static void Warning(string format, params object[] args) {
			if(Level > LogLevel.Warning) return;
			Debug.LogWarning(string.Format(format, args));
		}

		public static void Error(object obj) {
			if(Level > LogLevel.Error) return;
			Debug.LogError(obj);
		}

		public static void Error(string format, params object[] args) {
			if(Level > LogLevel.Error) return;
			Debug.LogError(string.Format(format, args));
		}

		public enum LogLevel {
			Info,
			Warning,
			Error,
			Silent
		}
	}
}
