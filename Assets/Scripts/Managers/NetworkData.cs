using Unity.Netcode;

public class NetworkData : NetworkSingleton<NetworkData>
{
    public NetworkList<int> RoundCardIDs;
    public NetworkVariable<bool> RoundReady = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            RoundCardIDs = new NetworkList<int>();
    }
}