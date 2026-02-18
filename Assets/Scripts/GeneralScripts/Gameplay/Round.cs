using System.Collections;
using SOFile;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Round : NetworkBehaviour
{

    private NetworkList<int> roundCardIDs = new NetworkList<int>();
    private NetworkVariable<bool> roundReady = new(false);
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        roundCardIDs.OnListChanged += OnRoundCardsChanged;

        if (IsServer)
        {
            StartRound();
        }
    }

    private void OnRoundCardsChanged(NetworkListEvent<int> changeEvent)
    {
        if (!IsClient) return;
        
        if (roundReady.Value)
        {
            CardManager.Instance.InstantiateRoundCardsByID(roundCardIDs);
            roundReady.Value = false;
        }
    }

    
    private void StartRound()
    {
        // Reset();

        Choose10RoundCards();
        
        // Wait 10 seconds
        WaitForMemorization();
    }

    private void WaitForMemorization()
    {
        StartCoroutine(MemorizationCoroutine());
    }

    private IEnumerator MemorizationCoroutine()
    {
        yield return new WaitForSeconds(10f);
        RunAfterMemorization();
    }

    private void RunAfterMemorization()
    {
        CardManager.Instance.HideAllRoundCards();
        StartSubRound();
    }
    
    private void StartSubRound()
    {
        if (!IsServer) return;

        int randomIndex = Random.Range(0, roundCardIDs.Count);
        int chosenCardID = roundCardIDs[randomIndex];

        GameObject subRoundObj = GameInitializer.Instance.SpawnSubRound();

        SubRound subRound = subRoundObj.GetComponent<SubRound>();
        subRound.Initialize(chosenCardID);
    }

    private void Choose10RoundCards()
    {
        if (!IsServer) return;

        roundCardIDs.Clear();

        CardDataSO[] roundCardData = CardManager.Instance.GetRoundCards(10);

        foreach (var card in roundCardData)
        {
            roundCardIDs.Add(card.cardID);
        }

        roundReady.Value = true;

    }

    
}