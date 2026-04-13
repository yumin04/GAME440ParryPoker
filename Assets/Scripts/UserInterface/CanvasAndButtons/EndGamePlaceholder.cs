using GeneralScripts.Gameplay;
using GenericHelpers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UserInterface.CanvasAndButtons {
	public class EndGamePlaceholder : Singleton<EndGamePlaceholder> {
		[FormerlySerializedAs("EndGameButton")]
		[SerializeField] private Button endGameButton;

		public void Start() {
			endGameButton.onClick.AddListener(EndGame);
		}

		private static void EndGame() {
			Game.Instance.EndGame();
		}
	}
}
