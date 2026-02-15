using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Managers {
	public class SceneLoader : Singleton<SceneLoader> {
		private enum SceneType {
			MainMenuScene,
			RoundScene,
			ResultScene
		}

		// =========================
		// Public Scene APIs
		// =========================

		public void LoadMainMenuScene() {
			Load(SceneType.MainMenuScene);
		}

		public void LoadResultScene() {
			Load(SceneType.ResultScene);
		}

		public void LoadRoundScene() {
			Load(SceneType.RoundScene);
		}

		// =========================
		// Internal Loader
		// =========================

		private static void Load(SceneType sceneType) {
			var sceneName = sceneType.ToString();

			// Going back to main menu: shut down network so we can start a new Host/Client next time
			if (sceneType == SceneType.MainMenuScene) NetworkManager.Singleton.Shutdown();

			if ((NetworkManager.Singleton?.IsServer ?? false) || (NetworkManager.Singleton?.IsHost ?? false))
				NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			else if (!(NetworkManager.Singleton?.IsListening ?? false)) SceneManager.LoadScene(sceneName);
		}
	}
}