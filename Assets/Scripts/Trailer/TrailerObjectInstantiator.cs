using System.Collections;
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
        Debug.Log("[DEBUG] Cards: " + cards.Length);
        StartCoroutine(InstantiateRoundCards(cards));
    }
    
    private IEnumerator InstantiateRoundCards(CardDataSO[] roundCards)
    {
        int index = 0;
        foreach (float x in xPositions)
        {
            foreach (float z in zPositions)
            {
                Vector3 spawnPos = new Vector3(x, yPosition + 1f, z + 0.5f);
                // TODO: 이거 카드 뒷면이고
                // 여기서 Animate하는걸로
                TrailerCard card = Instantiate(Card, spawnPos, Quaternion.Euler(0f, 0f, 180f)).GetComponent<TrailerCard>();
                Debug.Log("[DEBUG] Card: " + roundCards[index].name);
                card.Init(roundCards[index]);
                index++;
                StartCoroutine(AnimateTrailerCard(card, z));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator AnimateTrailerCard(TrailerCard card, float z)
    {
        Transform t = card.transform;

        Vector3 startPos = t.position; // 이미 y+1 상태
        Vector3 targetPos = new Vector3(startPos.x, yPosition, z);

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t01 = time / duration;

            // 부드럽게 (ease out 느낌)
            float ease = 1f - Mathf.Pow(1f - t01, 3f);

            t.position = Vector3.Lerp(startPos, targetPos, ease);
            yield return null;
        }

        t.position = targetPos;
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