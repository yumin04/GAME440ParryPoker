using UnityEngine;
using System.Collections.Generic;
using SOFile;

public class CardRepository : MonoBehaviour
{
    private List<CardDataSO> allCardData = new List<CardDataSO>();
    
    
    private HashSet<int> usedCardIds = new HashSet<int>();
    [SerializeField] private List<int> debugUsedCardIds;
    public void OnEnable()
    {
        GameEvents.OnRoundEnd += Reset;
    }
    public void OnDisable()
    {
        GameEvents.OnRoundEnd -= Reset;
    }
    
    
    private void LoadAllCardData()
    {
        allCardData.Clear();

        string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };

        foreach (string suit in suits)
        {
            CardDataSO[] loaded = Resources.LoadAll<CardDataSO>($"CardData/{suit}");
            allCardData.AddRange(loaded);
        }
    }
    
    public CardDataSO GetRandomCard()
    {
        if (allCardData.Count == 0) LoadAllCardData();

        if (usedCardIds.Count >= allCardData.Count)
        {
            Debug.LogWarning("All cards have been used.");
            return null;
        }

        CardDataSO card;

        do
        {
            card = allCardData[Random.Range(0, allCardData.Count)];
        }
        while (usedCardIds.Contains(card.cardID));

        usedCardIds.Add(card.cardID);
        debugUsedCardIds = new List<int>(usedCardIds);
        return card;
    }
    public void Reset()
    {
        usedCardIds.Clear();
    }

    public CardDataSO GetCardByID(int id)
    {
        if (allCardData.Count == 0) LoadAllCardData();

        foreach (var card in allCardData)
        {
            if (card.cardID == id)
                return card;
        }

        Debug.LogError($"Card with ID {id} not found.");
        return null;
    }
}