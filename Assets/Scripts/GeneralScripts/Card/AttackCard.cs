using System;
using SOFile;
using UnityEngine;
using Unity.Netcode;

public class AttackCard : NetworkBehaviour
{

    
    // TODO: 
    // 이거 카드 texture은 앞뒤 둘다 같은 texture이 나을거 같고
    // 그냥 component, prefab일듯
    
    [SerializeField] private CardDataSO cardData;

    private int cardId;
    private bool clickable = false;
    public NetworkVariable<Vector3> endPosition = new();
    
    // TODO: Correct Speed
    private float movementSpeed = 15f;



    
    public void Init(Vector3 endPosition)
    {
        this.endPosition.Value = endPosition;
        Debug.Log("[DEBUG] endPosition: " + endPosition);
    }
    
    public void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            endPosition.Value,
            movementSpeed * Time.deltaTime
        );
    }
    // Modifier pattern 사용
    // Sine이나 cosine사용하면 될듯?
    // 날라가는 position에 부착
    private void OnMouseDown()
    {
        if (!clickable) return;
        
        HandleCardClick();
        DespawnGameObject();
    }


    private void HandleCardClick()
    {
        GameEvents.OnAttackEnd.Invoke();
    }
    
    
    public void DespawnGameObject()
    {
        DespawnGameObjectRPC();
    }
    
    [Rpc(SendTo.Server)]
    private void DespawnGameObjectRPC()
    {
        NetworkObject.Despawn(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        EnableClick();
    }
    
    private void EnableClick() => clickable = true;
}