using System.Collections;
using SOFile;
using UnityEngine;

public class TrailerObjectInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject slotMachine;
    
    private GameObject currentSlotMachine;
    [SerializeField] private GameObject crossHair;
    
    
    
    private readonly float[] xPositions = { -4.5f, -3f, -1.5f, 0f, 1.5f };

    private readonly float[] zPositions = { 0f, 2f };

    private const float yPosition = -0.3f;

    private const float xMinBoundary = -5.5f;
    private const float xMaxBoundary = 2f;

    private const float zMinBoundary = -0.5f;
    private const float zMaxBoundary = 2.5f;
    
    [Header("Card")]    
    [SerializeField] private GameObject Card;
    private GameObject currentCard;
    
    [Header("Mouse Pointer")]
    [SerializeField] private GameObject mousePointer;
    [SerializeField] private GameObject pointerPanel;

    [Header("Objects")]
    [SerializeField] private CardRepository cardRepository;
    [SerializeField] private PlayerHand player1Hand;
    [SerializeField] private PlayerHand player2Hand;

    [Header("Positions")]
    [SerializeField] private Transform p1CheckCardPosition;
    [SerializeField] private Transform p2CheckCardPosition;
    [SerializeField] private Transform slotMachinePosition;
    
    [SerializeField] private RectTransform initialMousePointerPosition;
    [SerializeField] private RectTransform attackMousePointerPosition;
    [SerializeField] private RectTransform keepMousePointerPosition;
    

    # region Player Hands
    public void AddCardToPlayer1(CardDataSO cardData)
    {
        player1Hand.AddCard(cardData);
    }
    public void AddCardToPlayer2(CardDataSO cardData)
    {
        player2Hand.AddCard(cardData);
    }
    
    public void AddMultipleCardsToPlayer1(int[] player1Cards)
    {
        for (int i = 0; i < player1Cards.Length; i++)
        {
            AddCardToPlayer1(cardRepository.GetCardByID(player1Cards[i]));
        }
    }
    public void AddMultipleCardsToPlayer2(int[] player2Cards)
    {
        for (int i = 0; i < player2Cards.Length; i++)
        {
            AddCardToPlayer2(cardRepository.GetCardByID(player2Cards[i]));
        }
    }
    public void DisablePlayer1Hand() => player1Hand.gameObject.SetActive(false);
    public void DisablePlayer2Hand() => player2Hand.gameObject.SetActive(false);
    public void EnablePlayer1Hand() => player1Hand.gameObject.SetActive(true);
    
    public void EnablePlayer2Hand() => player2Hand.gameObject.SetActive(true);
        
    
    # endregion
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
        currentCard = Instantiate(Card, randomPosition, Quaternion.identity);
        TrailerCard c = currentCard.GetComponent<TrailerCard>();
        c.Init(subRoundCard);
    }    
    private Vector3 GenerateRandomXZPosition()
    {
        float x = Random.Range(xMinBoundary, xMaxBoundary);
        float z = Random.Range(zMinBoundary, zMaxBoundary);

        return new Vector3(x, yPosition, z);
    }

    public void MoveCardToP1CheckPosition()
    {
        currentCard.transform.position = p1CheckCardPosition.position;
        currentCard.transform.rotation = p1CheckCardPosition.rotation;
    }
    public void MoveCardToP2CheckPosition()
    {
        currentCard.transform.position = p2CheckCardPosition.position;
        currentCard.transform.rotation = p2CheckCardPosition.rotation;
    }

    public void DisableCard()
    {
        currentCard.SetActive(false);
    }

    public void EnableCard()
    {
        currentCard.SetActive(true);
    }
    
    public void EnableMousePointerUI()
    {
        pointerPanel.SetActive(true);
    }

    public void DisableMousePointerUI()
    {
        pointerPanel.SetActive(true);
    }
    public void MoveMousePointerToAttack(float duration = 0.5f)
    {
        StartMovingMousePointer(mousePointer, attackMousePointerPosition, duration);
    }
    

    public void MoveMousePointerToKeep(float duration = 0.5f)
    {
        StartMovingMousePointer(mousePointer, keepMousePointerPosition, duration);
    }
    
    private void StartMovingMousePointer(GameObject o, Transform target, float duration)
    {
        StartCoroutine(MovingMousePointer(o, target, duration));
    }

    private IEnumerator MovingMousePointer(GameObject o, Transform target, float duration)
    {
        float elapsed = 0f;

        Vector3 startPos = o.transform.position;
        Vector3 endPos = target.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            t = t * t * (3f - 2f * t);

            o.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;    }
        
        o.transform.position = endPos;
    }

    public void InstantiateSlotMachine()
    {
        currentSlotMachine = Instantiate(slotMachine, slotMachinePosition.position, slotMachinePosition.rotation);
    }

    public void DisableSlotMachine()
    {
        currentSlotMachine.SetActive(false);
    }
}