using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.GameEssentials {
	public class Round : MonoBehaviour {
		public void Start() {
			GameEvents.OnRoundStart?.Invoke();
		}

		private void Update() {
			// Press G to end the game. In multiplayer we tell everyone; in solo we just do it locally.
			/* TODO: Use generics from Unity input system */
			if (Keyboard.current == null || !Keyboard.current.gKey.wasPressedThisFrame)
				return;

			var net = NetworkManager.Singleton;
			if (net && net.IsListening) {
				var sync = FindAnyObjectByType(typeof(GameOverSync)) as GameOverSync;
				if (sync)
					sync.RequestEndGameServerRpc();
			}
			else {
				GameEvents.OnRoundEnd?.Invoke();
			}
		}
	}
}
