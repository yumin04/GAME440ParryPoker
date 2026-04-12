using UnityEditor;

namespace Managers.ApplicationEssentials {
	public static class EndApplication {
		public static void QuitApplication() {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
		}
	}
}
