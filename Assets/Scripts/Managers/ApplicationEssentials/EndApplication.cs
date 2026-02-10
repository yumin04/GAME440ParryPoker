using UnityEditor;


public static class EndApplication {
	// Already Declared as Singleton from "GenericHelpers"
	public static void QuitApplication() {
		#if UNITY_EDITOR
				EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}
}
