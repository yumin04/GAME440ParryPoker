using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using GeneralScripts;
using GenericHelpers;

public class HostClientManager : Singleton<HostClientManager> {
		// private bool clientConnected = false;
		protected override void Awake() {
			base.Awake();
			Debug.Log("NetworkConnectionManager Awake");
		}

		private void OnEnable() {
			GameEvents.OnLoadResultScene += EndHost;
		}

		private void OnDisable() {
			GameEvents.OnLoadResultScene -= EndHost;
		}

		public void StartHost() {
			NetworkManager.Singleton.StartHost();
			NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		
		public string GetLocalIP()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
					return ip.ToString();
			}
			return "No IPv4";
		}


		public void StartClient(string ipAddress = "127.0.0.1") {
			SetIPAddress(ipAddress);
			Debug.Log("ipAddress: " + ipAddress);
			NetworkManager.Singleton.StartClient();
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		}

		public void EndHost() {
			NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoaded;
			Debug.Log("NetworkConnectionManager EndHost");
			EndNetworkSession();
		}

		public void EndClient() {
			Debug.Log("NetworkConnectionManager EndClient");
			EndNetworkSession();
		}

		#region private methods

		private void EndNetworkSession() {
			if (!NetworkManager.Singleton) return;

			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;

			if (NetworkManager.Singleton.IsListening) NetworkManager.Singleton.Shutdown();
		}

		private void OnClientConnected(ulong clientId) {
			if (NetworkManager.Singleton.IsHost) {
				if (clientId != NetworkManager.Singleton.LocalClientId) {
					Debug.Log("Host: Client connected");
					SceneLoader.Instance.LoadRoundScene();
				}
			}
			else if (NetworkManager.Singleton.IsClient) {
				if (clientId == NetworkManager.Singleton.LocalClientId) Debug.Log("Client: Connected to host");
			}
		}

		private void OnSceneLoaded(
			ulong clientId,
			string sceneName,
			LoadSceneMode mode) {
			if (!NetworkManager.Singleton.IsServer) return;

			if (sceneName == "RoundScene" &&
			    clientId == NetworkManager.Singleton.LocalClientId)
				GameInitializer.Instance.SpawnGame();
		}

		private void SetIPAddress(string ipAddress = "127.0.0.1") {
			// I hate this code
			var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
			if (transport) transport.ConnectionData.Address = ipAddress;
		}

		#endregion
	}
