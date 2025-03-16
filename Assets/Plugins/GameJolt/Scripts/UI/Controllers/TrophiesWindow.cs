using UnityEngine;
using System;
using System.Linq;
using GameJolt.API;

namespace GameJolt.UI.Controllers {
	public class TrophiesWindow : BaseWindow {
		public RectTransform Container;
		public GameObject TrophyItem;

		private Action<bool> callback;

		public override void Show(Action<bool> callback) {
			Animator.SetTrigger("Trophies");
			Animator.SetTrigger("ShowLoadingIndicator");
			this.callback = callback;

			Trophies.Get(trophies => {
				if(trophies != null) {
					trophies = trophies.Where(x => !x.IsSecret || x.Unlocked).ToArray();
					// Create children if there are not enough
					while(Container.childCount < trophies.Length) {
						var tr = Instantiate(TrophyItem).transform;
						tr.SetParent(Container);
						tr.SetAsLastSibling();
					}

					// Update children's text.
					for(int i = 0; i < trophies.Length; ++i) {
						Container.GetChild(i).GetComponent<TrophyItem>().Init(trophies[i]);
					}

					Animator.SetTrigger("HideLoadingIndicator");
					Animator.SetTrigger("Unlock");
				} else {
					// TODO: Show error notification
					Animator.SetTrigger("HideLoadingIndicator");
					Dismiss(false);
				}
			});
		}

		public override void Dismiss(bool success) {
			Animator.SetTrigger("Dismiss");
			if(callback != null) {
				callback(success);
				callback = null;
			}
		}
	}
}