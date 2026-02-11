using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Managers {
	public static class HostClientManager {
		public static void StartHost() {
			SetNetworkAddress("0.0.0.0");
			NetworkManager.Singleton.StartHost();
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		public static void EndHost() {
			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
		}

		public static void StartClient(string address = "127.0.0.1") {
			SetNetworkAddress(address);
			NetworkManager.Singleton.StartClient();
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		public static void EndClient() {
			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
		}

		private static void OnClientConnected(ulong clientId) {
			if (NetworkManager.Singleton.IsHost) {
				if (clientId == NetworkManager.Singleton.LocalClientId) return;
				Debug.Log("Host: Client connected");
				SceneLoader.Instance.LoadRoundScene();
			}
			else if (NetworkManager.Singleton.IsClient && clientId == NetworkManager.Singleton.LocalClientId) {
				Debug.Log("Client: Connected to host");
			}
		}

		private static void SetNetworkAddress(string address) {
			// thanks Unity for making this so easy to read
			NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = address;
		}
	}
}