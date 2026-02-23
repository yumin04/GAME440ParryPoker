using UnityEngine;
using System.Collections.Generic;
using SOFile;

public class CardRepository : MonoBehaviour
{
    private List<CardDataSO> allCardData = new List<CardDataSO>();
    
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
        return allCardData[Random.Range(0, allCardData.Count)];
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