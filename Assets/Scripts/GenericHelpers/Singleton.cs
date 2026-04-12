using UnityEngine;

namespace GenericHelpers {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
		public static T Instance { get; private set; }

		protected virtual void Awake() {
			if (Instance && Instance != this) {
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
	}
}
