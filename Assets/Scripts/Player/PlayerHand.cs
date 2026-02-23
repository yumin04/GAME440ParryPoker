using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // Hopefully we can delete this and refactor more
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float spacing = 1.5f;

    // This is going to be deleted after the 10 cards destroy or whatever
    private List<GameObject> cards = new();

    public void AddCard(int cardId)
    {
        GameObject card = Instantiate(cardPrefab, transform);
        
        // card.GetComponent<Card>().Initialize(cardId);

        cards.Add(card);
        Rearrange();
    }

    public void DisplayCards(List<int> cardIds)
    {
        Clear();

        foreach (int id in cardIds)
            AddCard(id);
    }

    private void Rearrange()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 pos = new Vector3(i * spacing, 0, 0);
            cards[i].transform.localPosition = pos;
        }
    }

    private void Clear()
    {
        foreach (var card in cards)
            Destroy(card);

        cards.Clear();
    }
}