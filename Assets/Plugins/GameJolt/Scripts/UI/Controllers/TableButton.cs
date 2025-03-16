using GameJolt.API.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers {
	public class TableButton : MonoBehaviour {
		public Text Title;
		public Image BackgroundImage;
		public Color DefaultBackgroundColour = Color.grey;
		public Color ActiveBackgroundColour = Color.green;

		private Button button;
		private int tabIndex;
		private LeaderboardsWindow windowController;
		private bool active;

		public void Awake() {
			button = GetComponent<Button>();
		}

		public void Init(Table table, int index, LeaderboardsWindow controller, bool active = false) {
			Title.text = table.Name;
			tabIndex = index;
			windowController = controller;
			SetActive(active);


			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(Clicked);
		}

		public void SetActive(bool active) {
			this.active = active;
			BackgroundImage.color = active ? ActiveBackgroundColour : DefaultBackgroundColour;
		}

		public void Clicked() {
			if(!active) {
				SetActive(!active);
				windowController.ShowTab(tabIndex);
			}
		}
	}
}