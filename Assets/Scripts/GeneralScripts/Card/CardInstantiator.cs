using SOFile;
using Unity.Netcode;
using UnityEngine;

public class CardInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject clickableCardPrefab;
    [SerializeField] private GameObject attackCardPrefab;

    
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

    public void SpawnAttackCard(Vector3 startPosition, Vector3 endPosition)
    {
        // 이거는 가져오면 될듯
        Quaternion defaultRotation = Quaternion.LookRotation(endPosition - startPosition);
        
        GameObject obj = Instantiate(attackCardPrefab, startPosition, defaultRotation);
        

        NetworkObject netObj = obj.GetComponent<NetworkObject>();
        netObj.Spawn();
        
        AttackCard card = obj.GetComponent<AttackCard>();
        card.Init(endPosition);


        
        // 여기서 움직임 trigger도 괜찮을듯?
    }
}