using System;
using SOFile;
using Unity.Netcode;
using UnityEngine;

public class Card : NetworkBehaviour
{
    public NetworkVariable<int> cardId = new NetworkVariable<int>();
    
    [SerializeField] private CardDataSO cardData;
    
    public void Init(int cardId)
    {
        this.cardId.Value = cardId;
    }

    
    public void OnEnable()
    {
        GameEvents.HideAllInstantiatedCards += HideCard;
        GameEvents.DestroyAllInstantiatedCards += DestroyCard;
        cardId.OnValueChanged += OnCardDataChanged;
        GameEvents.OnKeepClicked += TryDespawn;
    }

    
    
    public void OnDisable()
    {
        GameEvents.HideAllInstantiatedCards -= HideCard;
        GameEvents.DestroyAllInstantiatedCards -= DestroyCard;
        cardId.OnValueChanged -= OnCardDataChanged;
        GameEvents.OnKeepClicked -= TryDespawn;
    }
    
    // TODO: Refactor this so Card does not need to know this maybe?
    private void OnCardDataChanged(int previousValue, int newValue)
    {
        cardData = CardManager.Instance.GetCardByID(newValue);
        // TODO: This is where we change the texture of the card.
    }    
    
    private void TryDespawn()
    {
        // if (!IsOwner) return;
        DespawnCardRPC();
    }
    [Rpc(SendTo.Server)]
    private void DespawnCardRPC(RpcParams rpcParams = default)
    {
        NetworkObject.Despawn(true);
    }

    private void HideCard()
    {
        // Or move to a different place
        gameObject.SetActive(false);
    }

    private void DestroyCard()
    {
        Destroy(gameObject);
    }
    private void ShowCard()
    {
        gameObject.SetActive(true);
    }
    

}