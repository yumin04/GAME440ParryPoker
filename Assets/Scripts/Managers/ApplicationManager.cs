using UnityEditor;

namespace Managers {
	public static class ApplicationManager {
		// Already Declared as Singleton from "GenericHelpers"
		public static void QuitApplication() {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}