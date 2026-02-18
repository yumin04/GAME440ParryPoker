using System;
using System.Collections;
using SOFile;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Linq;
using Random = UnityEngine.Random;

public class Round : NetworkBehaviour
{

    private NetworkList<int> roundCardIDs = new NetworkList<int>();

    public void OnEnable()
    {
        GameEvents.OnSubRoundEnd += CheckSubRoundRemaining;
    }

    public void OnDisable()
    {
        GameEvents.OnSubRoundEnd -= CheckSubRoundRemaining;
    }

    private void CheckSubRoundRemaining()
    {
        if (!IsServer) return;

        if (roundCardIDs.Count == 0)
        {
            Debug.Log("No more cards. Ending round.");

            EndRound();
        }
        else
        {
            Debug.Log("Cards remaining. Starting next subround.");

            StartSubRound();
        }
    }

    private void EndRound()
    {
        // TODO: we need to implement this
        Debug.Log("[DEBUG] EndRound, For now, ending Game");
        Game.Instance.EndGame();
        
    }


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
        // TODO: Do we need this?
        if (!IsClient) return;

    }

    
    private void StartRound()
    {
        // Reset();
        Debug.Log("[DEBUG] Choosing 10 Cards");
        Choose10RoundCards();
        Debug.Log("[DEBUG] Chose 10 Cards: " + roundCardIDs.Count);
        // Wait 10 seconds
        WaitForMemorization();
    }

    private void WaitForMemorization()
    {
        StartCoroutine(MemorizationCoroutine());
    }

    private IEnumerator MemorizationCoroutine()
    {
        if (!IsServer) yield break;

        yield return new WaitForSeconds(10f);
        RunAfterMemorizationRPC();
    }

    
    [Rpc(SendTo.ClientsAndHost)]
    private void RunAfterMemorizationRPC()
    {
        CardManager.Instance.HideAllRoundCards();
        StartSubRound();
    }
    
    private void StartSubRound()
    {
        if (!IsServer) return;

        if (roundCardIDs.Count == 0) return;

        int randomIndex = Random.Range(0, roundCardIDs.Count);
        int chosenCardID = roundCardIDs[randomIndex];
        
        roundCardIDs.RemoveAt(randomIndex);

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

    
        CardManager.Instance.InstantiateRoundCardsByID(roundCardIDs);
        

    }

    private int[] ToArray(NetworkList<int> list)
    {
        int[] ids = new int[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            ids[i] = list[i];
        }
        return ids;
    }
    

}