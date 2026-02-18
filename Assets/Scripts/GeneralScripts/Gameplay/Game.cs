using Unity.Netcode;
using UnityEngine;


public class Game : NetworkSingleton<Game>
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // show opponent character
        // show opponent health
        
        // After all animation and visuals are done, spawn round
        GameInitializer.Instance.SpawnRound();
    }
    
    public void EndGame()
    {
        if (IsServer)
        {
            EndGame_Server();
        }
        else
        {
            RequestEndGameRpc();
        }
    }

    
    [Rpc(SendTo.Server)]
    private void RequestEndGameRpc()
    {
        EndGame_Server();
    }
    
    
    private void EndGame_Server()
    {
        if (!IsServer) return;

        GameEvents.OnGameEnd?.Invoke();
        
        NetworkManager.SceneManager.LoadScene("ResultScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}