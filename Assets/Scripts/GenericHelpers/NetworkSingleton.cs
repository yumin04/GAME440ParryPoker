using Unity.Netcode;

namespace GenericHelpers {
	public class NetworkSingleton<T> : NetworkBehaviour where T : NetworkBehaviour {
		public static T Instance { get; private set; }

		protected virtual void Awake() {
			if (Instance && Instance != this) {
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
			DontDestroyOnLoad(gameObject);
		}

		public override void OnNetworkDespawn() {
			if (IsServer) {
				Destroy(gameObject);
			}
		}
	}
}
