using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.CanvasAndButtons {
	public class ResultMenuCanvas : MonoBehaviour {
		// Don't change it, made this so we do not need to hook up to UI again.
		// To make things scalable, this is better
		[Header("Panels")]
		[SerializeField] private GameObject resultMenuPanel;
		
		[Header("Buttons")]
		[SerializeField] private Button BackToMainMenuButton;
		[SerializeField] private Button ExitButton;
		
		public void Start()
		{
			BackToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
			
			ExitButton.onClick.AddListener(OnExitClicked);
		}
		
		private void OnBackToMainMenuClicked() {
			Debug.Log("OnBackToMainMenuClicked");
			SceneLoader.Instance.LoadMainMenuScene();
		}
		
		private void OnExitClicked() {
			Debug.Log("OnExitClicked");
			EndApplication.QuitApplication();
		}
		
	}
}
