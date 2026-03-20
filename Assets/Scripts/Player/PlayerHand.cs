using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // Hopefully we can delete this and refactor more
    [SerializeField] private GameObject playerDisplayCardPrefab;
    [SerializeField] private float spacing = 1.5f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float maxAngle = 60f;

    // This is going to be deleted after the 10 cards destroy or whatever
    private List<GameObject> cards = new();

    // public void Awake()
    // {
    //     Rearrange();
    // }
    //
    // public void OnEnable()
    // {
    //     Rearrange();
    // }

    public void AddCard(int cardId)
    {
        GameObject card = Instantiate(playerDisplayCardPrefab, transform);
        
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
        int count = cards.Count;
        if (count == 0) return;

        float angleStep = count == 1 ? 0 : maxAngle / (count - 1);
        float startAngle = -maxAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(rad) * radius;
            float y = Mathf.Cos(rad) * radius - radius;

            cards[i].transform.localPosition = new Vector3(x, y, 0);
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    private void Clear()
    {
        foreach (var card in cards)
            Destroy(card);

        cards.Clear();
    }
}