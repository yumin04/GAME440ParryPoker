using GenericHelpers;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Managers {
	public class SceneLoader : Singleton<SceneLoader> {
		private enum SceneType {
			MainMenuScene,
			RoundScene,
			ResultScene,
		}

		public void LoadMainMenuScene() {
			Load(SceneType.MainMenuScene);
		}

		public void LoadResultScene() {
			Load(SceneType.ResultScene);
		}

		public void LoadRoundScene() {
			Load(SceneType.RoundScene);
		}

		private void Load(SceneType sceneType) {
			var sceneName = sceneType.ToString();

			if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening) {
				NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
			else {
				SceneManager.LoadScene(sceneName);
			}
		}
	}
}
