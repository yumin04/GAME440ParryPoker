using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UserInterface.CanvasAndButtons {
	public class MainMenuCanvas : MonoBehaviour {
		[Header("Panels")]
		[SerializeField] private GameObject mainMenuPanel;

		[SerializeField] private GameObject netPanels;
		[SerializeField] private GameObject hostClientPanel;
		[SerializeField] private GameObject addressPanel;

		[Header("Text Fields")]
		[SerializeField] private TMP_InputField addressTextInput;

		private readonly Stack<GameObject> panelStack = new();

		// mainMenuPanel Interactions
		public void OnStartClicked() {
			Debug.Log("OnStartClicked");
			// make the "Host Client Panel" Pop Up
			netPanels.SetActive(true);
			panelStack.Push(netPanels);
			panelStack.Push(hostClientPanel);
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

		// NetPanel Interactions
		public void OnBackClicked() {
			Debug.Log("OnBackClicked");

			try {
				if (panelStack.Count != 2) {
					panelStack.Pop().SetActive(false);
					panelStack.Peek().SetActive(true);
				}
				// hack to hide NetPanels
				else {
					panelStack.Pop().SetActive(true);
					panelStack.Pop().SetActive(false);
				}
			}
			catch (InvalidOperationException) { }
		}

		// HostClientPanel Interactions
		public void OnHostClicked() {
			if (NetworkManager.Singleton == null) throw new NullReferenceException();

			HostClientManager.StartHost();
			Debug.Log("StartHost");
			// Move On To Next Scene
		}

		public void OnClientClicked() {
			hostClientPanel.SetActive(false);
			addressPanel.SetActive(true);
			panelStack.Push(addressPanel);
		}

		/* address panel interactions */
		public void OnAddressClicked() {
			if (NetworkManager.Singleton == null) throw new NullReferenceException();

			var address = addressTextInput.text;
			// if no one put anything in the box, use the placeholder
			if (address.Length == 0) {
				var placeholder = addressTextInput.placeholder as TMP_Text;
				// if someone sets the placeholder up stupid, fallback to loopback address
				address = placeholder != null ? placeholder.text : "127.0.0.1";
			}

			HostClientManager.StartClient(address, OnClientDisconnect);
			Debug.Log("StartClient");
			// Move On To Next Scene
		}

		private static void OnClientDisconnect(ulong clientID) {
			if (NetworkManager.Singleton.DisconnectReason != null)
				Debug.LogError(NetworkManager.Singleton.DisconnectReason);
		}
	}
}