using GameJolt.API.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers {
	public class TrophyItem : MonoBehaviour {
		public CanvasGroup Group;
		public Image Image;
		public Text Title;
		public Text Description;

		public void Init(Trophy trophy) {
			Group.alpha = trophy.Unlocked ? 1f : .6f;
			Title.text = trophy.Title;
			Description.text = trophy.Description;

			if(trophy.Image != null) {
				Image.sprite = trophy.Image;
			} else {
				trophy.DownloadImage((success) => {
					if(success) {
						Image.sprite = trophy.Image;
					}
				});
			}
		}
	}
}