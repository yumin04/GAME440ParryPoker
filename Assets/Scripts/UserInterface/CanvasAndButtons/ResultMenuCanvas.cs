using Managers;
using Managers.ApplicationEssentials;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UserInterface.CanvasAndButtons {
	public class ResultMenuCanvas : MonoBehaviour {
		// Don't change it, made this so we do not need to hook up to UI again.
		// To make things scalable, this is better
		[Header("Panels")]
		[SerializeField] private GameObject resultMenuPanel;

		[Header("Buttons")]
		[FormerlySerializedAs("BackToMainMenuButton")]
		[SerializeField]
		private Button backToMainMenuButton;
		[FormerlySerializedAs("ExitButton")]
		[SerializeField] private Button exitButton;

		public void Start() {
			backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);

			exitButton.onClick.AddListener(OnExitClicked);
		}

		private void OnBackToMainMenuClicked() {
			Debug.Log("OnBackToMainMenuClicked");
			SceneLoader.Instance.LoadMainMenuScene();
		}

		private static void OnExitClicked() {
			Debug.Log("OnExitClicked");
			EndApplication.QuitApplication();
		}
	}
}
