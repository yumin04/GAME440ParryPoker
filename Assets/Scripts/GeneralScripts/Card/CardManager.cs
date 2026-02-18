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
    private Quaternion defaultRotation = Quaternion.Euler(90f, 0f, 0f);
    
    [SerializeField] private CardInstantiator cardInstantiator;
    [SerializeField] private CardRepository cardRepository;


    public CardDataSO[] GetRoundCards(int numSubRound)
    {
        CardDataSO[] roundCards = new CardDataSO[numSubRound];

        for (int i = 0; i < numSubRound; i++)
        {
            roundCards[i] = cardRepository.GetRandomCard();
        }

        return roundCards;
    }
    


    public void HideAllRoundCards()
    {
        GameEvents.HideAllInstantiatedCards.Invoke();
    }

    public void InstantiateSubRoundCard(int cardId)
    {
        Vector3 randomPosition = GenerateRandomXZPosition();
        CardDataSO subRoundCard = cardRepository.GetCardByID(cardId);
        // Generate Random Position
        cardInstantiator.SpawnCard(subRoundCard, randomPosition, defaultRotation);
    }

    private Vector3 GenerateRandomXZPosition()
    {
        float x = Random.Range(xMinBoundary, xMaxBoundary);
        float z = Random.Range(zMinBoundary, zMaxBoundary);

        return new Vector3(x, yPosition, z);
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


    private void InstantiateRoundCards(CardDataSO[] roundCards)
    {
        int index = 0;
        foreach (float x in xPositions)
        {
            foreach (float z in zPositions)
            {
                Vector3 spawnPos = new Vector3(x, yPosition, z);
                cardInstantiator.InstantiateCard(roundCards[index], spawnPos, defaultRotation);
                index++;
            }
        }
    }

}
