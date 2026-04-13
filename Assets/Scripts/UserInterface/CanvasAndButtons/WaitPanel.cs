using GeneralScripts;
using UnityEngine;

namespace UserInterface.CanvasAndButtons {
	public class WaitPanel : MonoBehaviour {
		public void OnEnable() {
			GameEvents.HideWaitAndAttackPanel += DisablePanel;
			// If keep clicked, it will come to SubRoundEnd naturally, so no need to add that

			// If attack is clicked, it needs to move on to attack phase
			// GameEvents.OnAttackClicked += DisablePanel;
		}

		public void OnDisable() {
			GameEvents.HideWaitAndAttackPanel -= DisablePanel;
			// GameEvents.OnAttackClicked -= DisablePanel;
		}

		private void DisablePanel() {
			gameObject.SetActive(false);
		}
	}
}
