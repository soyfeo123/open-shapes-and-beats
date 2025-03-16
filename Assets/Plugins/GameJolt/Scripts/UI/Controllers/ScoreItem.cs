using UnityEngine;
using GameJolt.API;
using GameJolt.API.Objects;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers {
	public class ScoreItem : MonoBehaviour {
		public Text Username;
		public Text Value;

		public Color DefaultColour = Color.white;
		public Color HighlightColour = Color.green;

		public void Init(Score score) {
			Username.text = score.PlayerName;
			Value.text = score.Text;

			bool isUserScore = score.UserID != 0 && GameJoltAPI.Instance.HasUser &&
			                   GameJoltAPI.Instance.CurrentUser.ID == score.UserID;
			Username.color = isUserScore ? HighlightColour : DefaultColour;
			Value.color = isUserScore ? HighlightColour : DefaultColour;
		}
	}
}