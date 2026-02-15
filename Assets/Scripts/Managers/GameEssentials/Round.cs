using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.GameEssentials {
	public class Round : MonoBehaviour {
		private GameOverSync sync;

		public void Start() {
			sync = FindAnyObjectByType<GameOverSync>();
			GameEvents.OnRoundStart?.Invoke();
		}

		private void Update() {
			// Press G to end the game. In multiplayer we tell everyone; in solo we just do it locally.
			if (Keyboard.current == null || !Keyboard.current.gKey.wasPressedThisFrame)
				return;

			if (NetworkManager.Singleton?.IsListening ?? false) {
				if (sync)
					sync.RequestEndGameServerRpc();
			}
			else {
				GameEvents.OnRoundEnd?.Invoke();
			}
		}
	}
}
