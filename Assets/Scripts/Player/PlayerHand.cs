using System;
using System.Collections;
using System.Collections.Generic;
using SOFile;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject playerDisplayCardPrefab;

    [SerializeField] private float radius = 5f;
    [SerializeField] private float maxAngle = 60f;

    private List<GameObject> cards = new();
    

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

    public void AddCard(CardDataSO cardData)
    {
        GameObject card = Instantiate(playerDisplayCardPrefab, transform);
        card.GetComponent<PlayerCard>().Init(cardData);

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

            Vector3 targetPos = new Vector3(x, y, z);
            Quaternion targetRot = Quaternion.Euler(0, -angle, 0);

            // 🔥 마지막 카드만 animate
            if (i == count - 1)
            {
                StartCoroutine(AnimateCard(cards[i].transform, targetPos, targetRot));
            }
            else
            {
                cards[i].transform.localPosition = targetPos;
                cards[i].transform.localRotation = targetRot;
            }

            y += 0.001f;
        }
    }
    private IEnumerator AnimateCard(Transform card, Vector3 targetPos, Quaternion targetRot)
    {
        float duration = 0.35f;
        float t = 0f;

        Vector3 startPos = targetPos + Vector3.up * 2.5f; // 위에서 시작
        Quaternion startRot = targetRot;

        card.localPosition = startPos;
        card.localRotation = startRot;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            // 🔥 ease out (빠르게 시작 → 천천히 멈춤)
            float ease = 1f - Mathf.Pow(1f - normalized, 3f);

            card.localPosition = Vector3.Lerp(startPos, targetPos, ease);

            yield return null;
        }

        card.localPosition = targetPos;
    }
    public void Clear()
    {
        foreach (var card in cards)
            Destroy(card);

        cards.Clear();
    }
}