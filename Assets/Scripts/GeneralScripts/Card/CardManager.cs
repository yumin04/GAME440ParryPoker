using SOFile;
using Unity.Netcode;
using UnityEngine;


public class CardManager : Singleton<CardManager>
{
    private readonly float[] xPositions = { -3f, -1.5f, 0f, 1.5f, 3f };

    private readonly float[] zPositions = { -1f, 1f };

    private const float yPosition = 2.4f;

    private const float xMinBoundary = -4.5f;
    private const float xMaxBoundary = 4.5f;

    private const float zMinBoundary = -1.5f;
    private const float zMaxBoundary = 1.5f;
    private Quaternion cardShowInTableRotation = Quaternion.Euler(180f, 0f, 0f);
    private Quaternion cardHideInTableRotation = Quaternion.Euler(0f, 0f, 0f);
    
    [SerializeField] private CardInstantiator cardInstantiator;
    [SerializeField] private CardRepository cardRepository;


    public CardDataSO[] GetCards(int numCards)
    {
        CardDataSO[] roundCards = new CardDataSO[numCards];

        for (int i = 0; i < numCards; i++)
        {
            roundCards[i] = cardRepository.GetRandomCard();
        }

        return roundCards;
    }
    public CardDataSO GetCardByID(int newValue)
    {
        return cardRepository.GetCardByID(newValue);
    }
    
    
    public void InstantiateAttackCard(Vector3 startPosition, Vector3 endPosition)
    {
        cardInstantiator.SpawnAttackCard(startPosition, endPosition);
    }
    
    // DONE
    #region RoundCards
    public void HideAllRoundCards()
    {
        // GameEvents.HideAllInstantiatedCards.Invoke();
        GameEvents.DestroyAllInstantiatedCards.Invoke();
    }

    
    public void InstantiateRoundCardsByID(NetworkList<int> ids)
    {
        CardDataSO[] cards = new CardDataSO[ids.Count];

        for (int i = 0; i < ids.Count; i++)
        {
            cards[i] = cardRepository.GetCardByID(ids[i]);
        }

        InstantiateRoundCards(cards);
    }
    
    public void InstantiateRoundCardsByID(int[] ids)
    {
        CardDataSO[] cards = new CardDataSO[ids.Length];

        for (int i = 0; i < ids.Length; i++)
        {
            cards[i] = cardRepository.GetCardByID(ids[i]);
        }

        InstantiateRoundCards(cards);
    }
    private void InstantiateRoundCards(CardDataSO[] roundCards)
    {
        int index = 0;
        foreach (float x in xPositions)
        {
            foreach (float z in zPositions)
            {
                Vector3 spawnPos = new Vector3(x, yPosition, z);
                cardInstantiator.SpawnCard(roundCards[index], spawnPos, cardShowInTableRotation);
                index++;
            }
        }
    }
    #endregion

    #region SubRoundCardOnTable
        
    public void InstantiateSubRoundCard(int cardId)
    {
        Vector3 randomPosition = GenerateRandomXZPosition();
        CardDataSO subRoundCard = cardRepository.GetCardByID(cardId);
        // Generate Random Position
        cardInstantiator.SpawnClickableCard(subRoundCard, randomPosition, cardHideInTableRotation);
    }

    private Vector3 GenerateRandomXZPosition()
    {
        float x = Random.Range(xMinBoundary, xMaxBoundary);
        float z = Random.Range(zMinBoundary, zMaxBoundary);

        return new Vector3(x, yPosition, z);
    }
    #endregion

    


}
