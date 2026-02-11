using Unity.Netcode;

// Syncs "game over" to all players. When anyone ends the game, everyone sees the Go Home button.
// Put this on the same GameObject as NetworkObject in the Round scene.
namespace Managers.GameEssentials {
	public class GameOverSync : NetworkBehaviour {
		// Called on server when someone requests game over. Then tells all clients.
		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestEndGameServerRpc() {
			NotifyGameOverClientRpc();
		}

		// Runs on every client (and host). Each player shows their own Go Home button.
		[Rpc(SendTo.ClientsAndHost)]
		private void NotifyGameOverClientRpc() {
			GameEvents.OnRoundEnd?.Invoke();
		}
	}
}
