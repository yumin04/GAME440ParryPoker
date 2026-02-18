using SOFile;
using Unity.Netcode;
using UnityEngine;

public class CardInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    
    // This is network object, it needs to be "spawned"
    public void InstantiateCard(
        CardDataSO cardData,
        Vector3 position,
        Quaternion rotation)
    {
        Card card = Instantiate(cardPrefab, position, rotation)
            .GetComponent<Card>();

        card.Init(cardData);

    }

    public void SpawnCard(
        CardDataSO cardData,
        Vector3 position,
        Quaternion rotation)
    {
        GameObject obj = Instantiate(cardPrefab, position, rotation);

        NetworkObject netObj = obj.GetComponent<NetworkObject>();
        netObj.Spawn();

        Card card = obj.GetComponent<Card>();
        card.Init(cardData);

        obj.AddComponent<ClickableCard>();
    }
    
}