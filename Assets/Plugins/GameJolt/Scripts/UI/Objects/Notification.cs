using UnityEngine;

namespace GameJolt.UI.Objects {
	/// <summary>
	/// A UI Notification.
	/// </summary>
	public class Notification {
		#region Fields & Properties
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		public Sprite Image { get; private set; }
		#endregion Fields & Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Notification"/> class.
		/// </summary>
		/// <param name="text">The notification text.</param>
		public Notification(string text) : this(text, API.GameJoltAPI.Instance.DefaultNotificationIcon) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Notification"/> class.
		/// </summary>
		/// <param name="text">The notification text.</param>
		/// <param name="image">The notification image.</param>
		public Notification(string text, Sprite image) {
			Text = text;
			Image = image;
		}
		#endregion Constructors
	}
}
