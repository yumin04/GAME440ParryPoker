using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : Singleton<GameInitializer>
{
    [SerializeField] private GameObject gamePrefab;
    [SerializeField] private GameObject roundPrefab;
    [SerializeField] private GameObject subRoundPrefab;


    public GameObject SpawnGame()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(gamePrefab);
        instance.GetComponent<NetworkObject>().Spawn();
        return instance;
    }

    public GameObject SpawnRound()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(roundPrefab);
        instance.GetComponent<NetworkObject>().Spawn();
        return instance;
    }
    public GameObject SpawnSubRound()
    {
        if (!NetworkManager.Singleton.IsServer) return null;

        GameObject instance = Instantiate(subRoundPrefab);
        instance.GetComponent<NetworkObject>().Spawn();
        return instance;
    }
}