using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : Singleton<GameInitializer>
{
    [SerializeField] private GameObject gamePrefab;
    [SerializeField] private GameObject roundPrefab;
    [SerializeField] private GameObject subRoundPrefab;


    // TODO: IsServer여기서 체크 하지 않아도 괜찮아
    // TODO: 죽이기
    public NetworkObject SpawnGame()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(gamePrefab);
        NetworkObject nObj = instance.GetComponent<NetworkObject>();
        nObj.Spawn();
        return nObj;
    }

    public NetworkObject SpawnRound()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(roundPrefab);
        NetworkObject nObj = instance.GetComponent<NetworkObject>();
        nObj.Spawn();
        return nObj;
    }
    public NetworkObject SpawnSubRound()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(subRoundPrefab);
        
        NetworkObject nObj = instance.GetComponent<NetworkObject>();
        nObj.Spawn();
        return nObj;
    }
}