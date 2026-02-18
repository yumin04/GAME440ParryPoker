using System;
using SOFile;
using Unity.Netcode;
using UnityEngine;

public class Card : NetworkBehaviour
{
    [SerializeField] private CardDataSO cardData;
    
    public void Start()
    {
        Debug.Log("Card Start");
    }
    public void Init(CardDataSO data)
    {
        cardData = data;
        // TODO: This is where we change the texture of the card.
    }

    public void OnEnable()
    {
        GameEvents.HideAllInstantiatedCards += HideCard;
    }

    public void OnDisable()
    {
        GameEvents.HideAllInstantiatedCards -= HideCard;
    }
    
    private void HideCard()
    {
        // Or move to a different place
        gameObject.SetActive(false);
    }

    private void ShowCard()
    {
        gameObject.SetActive(true);
    }
    

}