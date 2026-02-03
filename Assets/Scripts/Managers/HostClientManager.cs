using UnityEngine;
using Unity.Netcode;

public class HostClientManager : Singleton<HostClientManager>
{
    // private bool clientConnected = false;
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("NetworkConnectionManager Awake");
    }
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    
    public void EndHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public void EndClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
    
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("Host: Client connected");
                SceneLoader.Instance.LoadRoundScene();
            }
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("Client: Connected to host");
            }
        }
    }

}