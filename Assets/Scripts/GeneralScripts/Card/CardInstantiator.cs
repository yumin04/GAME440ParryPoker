using SOFile;
using Unity.Netcode;
using UnityEngine;

public class CardInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject clickableCardPrefab;

    
    // TODO: Decide whether to get CardID instead of Card Data maybe?
    public void SpawnCard(
        CardDataSO cardData,
        Vector3 position,
        Quaternion rotation)
    {
        GameObject obj = Instantiate(cardPrefab, position, rotation);

        NetworkObject netObj = obj.GetComponent<NetworkObject>();
        netObj.Spawn(true);

        Card card = obj.GetComponent<Card>();
        card.Init(cardData.cardID);
        

    }


    public void SpawnClickableCard(
        CardDataSO cardData,
        Vector3 position,
        Quaternion rotation)
    {
        
        GameObject obj = Instantiate(clickableCardPrefab, position, rotation);
        
        NetworkObject netObj = obj.GetComponent<NetworkObject>();
        netObj.Spawn();
        
        ClickableCard card = obj.GetComponent<ClickableCard>();
        card.Init(cardData.cardID);
        
    }
    
}