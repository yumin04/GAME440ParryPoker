using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Managers {
	public class HostClientManager : Singleton<HostClientManager> {
		// private bool clientConnected = false;
		protected override void Awake() {
			base.Awake();
			Debug.Log("NetworkConnectionManager Awake");
		}

		public void StartHost() {
			// we want to bind to all identities for the best chance of succeeding
			NetworkChangeAddress("0.0.0.0");
			NetworkManager.Singleton.StartHost();
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		public void EndHost() {
			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
		}

		public void StartClient(string address = "127.0.0.1") {
			// bind to a given address, default to standard address because Unity no like localhost
			NetworkChangeAddress(address);
			NetworkManager.Singleton.StartClient();
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		public void EndClient() {
			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
		}

		private static void OnClientConnected(ulong clientId) {
			if (NetworkManager.Singleton.IsHost) {
				if (clientId == NetworkManager.Singleton.LocalClientId) return;
				Debug.Log("Host: Client connected");
				SceneLoader.Instance.LoadRoundScene();
			}
			else if (NetworkManager.Singleton.IsClient) {
				if (clientId == NetworkManager.Singleton.LocalClientId) Debug.Log("Client: Connected to host");
			}
		}

		// Unity doesn't like making this easy for us, so let's just use a private static method
		private static void NetworkChangeAddress(string address) {
			NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = address;
		}
	}
}