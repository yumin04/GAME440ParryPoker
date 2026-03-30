using SOFile;
using UnityEngine;

public class TrailerObjectInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject slotMachine;
    [SerializeField] private GameObject crossHair;
    
    
    
    private readonly float[] xPositions = { -4.5f, -3f, -1.5f, 0f, 1.5f };

    private readonly float[] zPositions = { 0f, 2f };

    private const float yPosition = -0.3f;

    private const float xMinBoundary = -5.5f;
    private const float xMaxBoundary = 2f;

    private const float zMinBoundary = -0.5f;
    private const float zMaxBoundary = 2.5f;
    
    [SerializeField] private GameObject Card;
    [SerializeField] private CardRepository cardRepository;
    
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
                Instantiate(Card, spawnPos, Quaternion.identity);
                index++;
            }
        }
    }

    public void InstantiateSubRoundCard(int cardId)
    {
        Vector3 randomPosition = GenerateRandomXZPosition();
        CardDataSO subRoundCard = cardRepository.GetCardByID(cardId);
        // Generate Random Position
        Instantiate(Card, randomPosition, Quaternion.identity);
    }    
    private Vector3 GenerateRandomXZPosition()
    {
        float x = Random.Range(xMinBoundary, xMaxBoundary);
        float z = Random.Range(zMinBoundary, zMaxBoundary);

        return new Vector3(x, yPosition, z);
    }
}