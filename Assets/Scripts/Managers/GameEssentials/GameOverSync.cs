using Unity.Netcode;
using UnityEngine;

// Syncs "game over" to all players. When anyone ends the game, everyone sees the Go Home button.
// Put this on the same GameObject as NetworkObject in the Round scene.
public class GameOverSync : NetworkBehaviour
{
    // Called on server when someone requests game over. Then tells all clients.
    [ServerRpc(RequireOwnership = false)]
    public void RequestEndGameServerRpc()
    {
        NotifyGameOverClientRpc();
    }

    // Runs on every client (and host). Each player shows their own Go Home button.
    [ClientRpc]
    void NotifyGameOverClientRpc()
    {
        GameEvents.OnRoundEnd?.Invoke();
    }
}
