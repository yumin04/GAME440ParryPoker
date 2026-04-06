using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject playerDisplayCardPrefab;

    [SerializeField] private float radius = 5f;
    [SerializeField] private float maxAngle = 60f;

    private List<GameObject> cards = new();
    
    public void Awake()
    {
        AddCard(1);
        AddCard(2);
        AddCard(3);
        AddCard(1);
        AddCard(2);
        AddCard(3);
        AddCard(1);
        Rearrange();
    }

    public void OnEnable()
    {
        Rearrange();
    }
    

    public void AddCard(int cardId)
    {
        GameObject card = Instantiate(playerDisplayCardPrefab, transform);
        
        card.GetComponent<PlayerCard>().Init(cardId);

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
        float y = 0;
        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(rad) * radius;
            float z = -Mathf.Cos(rad) * radius + radius;
            
            cards[i].transform.localPosition = new Vector3(x, y, z);
            
            cards[i].transform.localRotation = Quaternion.Euler(0, -angle, 0);
            y += 0.001f;
        }
    }

    public void Clear()
    {
        foreach (var card in cards)
            Destroy(card);

        cards.Clear();
    }
}