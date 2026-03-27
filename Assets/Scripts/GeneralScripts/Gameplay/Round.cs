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
    
    // TODO: make sure we are having correct seconds
    private float memorizationSeconds = 1f;

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
            

            ulong loserID = CalculateRoundWinner();

            // TODO:
            // instead of 10, have a round damage
            // or damage by hand rank?
            
            Game.Instance.DecreasePlayerHealth(loserID, 10);    
            

            EndRound();
        }
        else
        {
            Debug.Log("Cards remaining. Starting next subround.");
            StartSubRound();
        }
    }

    private ulong CalculateRoundWinner()
    {
        // Stub 
        Debug.LogWarning("[STUB] No Winner Calculation Implemented, player 2 lose");
        // TODO: 
        // 서버가 각각의 player에게 hand calculation 요청
        // 그다음 hand 반환
        return 1;
    }

    private void EndRound()
    {
        Debug.Log("[DEBUG] EndRound");
        GameEvents.OnRoundEnd?.Invoke();
        
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
        // TODO:
        // 여기서 SubRoundText 바꾸는거 ㄱㅊ할듯?
        // 이거 둘다 돌려지는지 확인
        if (!IsClient) return;
    }

    
    private void StartRound()
    {
        // Reset();
        Debug.Log("[DEBUG] Choosing 10 Cards");
        
        // Give 2 cards to each player
        Give2CardsToEachPlayer();
        
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
        
        
        yield return new WaitForSeconds(memorizationSeconds);
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
        
        // TODO: UserInterface.Instance.
        int randomIndex = Random.Range(0, roundCardIDs.Count);
        int chosenCardID = roundCardIDs[randomIndex];
        
        roundCardIDs.RemoveAt(randomIndex);

        GameObject subRoundObj = GameInitializer.Instance.SpawnSubRound();

        SubRound subRound = subRoundObj.GetComponent<SubRound>();
        subRound.Initialize(chosenCardID);
    }

    private void Give2CardsToEachPlayer()
    {
        if (!IsServer) return;
        
        // Refactor?
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            ulong clientId = client.ClientId;

            var cards = CardManager.Instance.GetCards(2);

            foreach (var card in cards)
            {
                GameEvents.OnPlayerKeepCard?.Invoke(clientId, card.cardID);
            }
        }
    }
    
    private void Choose10RoundCards()
    {
        if (!IsServer) return;

        roundCardIDs.Clear();

        CardDataSO[] roundCardData = CardManager.Instance.GetCards(10);

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