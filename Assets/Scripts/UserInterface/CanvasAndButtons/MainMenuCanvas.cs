using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UserInterface.CanvasAndButtons {
	public class MainMenuCanvas : MonoBehaviour {
		[Header("Panels")]
		[SerializeField] private GameObject mainMenuPanel;
		[SerializeField] private GameObject hostClientPanel;
		[SerializeField] private GameObject addressPanel;
		[Header("Text Fields")]
		[SerializeField] private TMP_InputField addressTextInput;

		// mainMenuPanel Interactions
		public void OnStartClicked() {
			Debug.Log("OnStartClicked");
			// make the "Host Client Panel" Pop Up
			hostClientPanel.SetActive(true);
		}

		public void OnTutorialClicked() {
			Debug.Log("OnTutorialClicked");
			// After all game is made
			// Doesn't need to be Tutorial?
		}

		public void OnExitClicked() {
			Debug.Log("OnExitClicked");
			ApplicationManager.QuitApplication();
		}

		// HostClientPanel Interactions
		public void OnBackClicked() {
			Debug.Log("OnBackClicked");
			hostClientPanel.SetActive(false);
		}

		public void OnHostClicked() {
			if (NetworkManager.Singleton == null) throw new NullReferenceException();

			HostClientManager.StartHost();
			Debug.Log("StartHost");
			// Move On To Next Scene
		}

		public void OnClientClicked() {
			hostClientPanel.SetActive(false);
			addressPanel.SetActive(true);
		}
		
		public void OnAddressClicked() {
			if (NetworkManager.Singleton == null) throw new NullReferenceException();

			HostClientManager.StartClient(addressTextInput.text, OnClientDisconnect);
			Debug.Log("StartClient");
			// Move On To Next Scene
		}

		private static void OnClientDisconnect(ulong clientID) {
			if (NetworkManager.Singleton.DisconnectReason != null) {
				Debug.LogError(NetworkManager.Singleton.DisconnectReason);
			}
		}
	}
}
