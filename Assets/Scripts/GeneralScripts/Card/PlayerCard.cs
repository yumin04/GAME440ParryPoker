using System;
using SOFile;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private CardDataSO cardData;
    private int cardId;
    
    public void Init(int cardId)
    {
        this.cardId = cardId;
    }

    private void OnEnable()
    {
        // Probably on Attack Phase Start
    }

    private void OnDisable()
    {
        
    }

    private void HidePlayerCard()
    {
        
    }
}
