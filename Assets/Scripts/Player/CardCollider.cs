
using Unity.Netcode;
using UnityEngine;

// initialize when defend is out.

// 이거는 Damage 계산이고
public class CardCollider : MonoBehaviour
{
    public void EnableCollider()
    {
        gameObject.SetActive(true);
    }

    private void DisableCollider()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        // TODO: Currently Decreasing health by 10 for every hit
        
        Game.Instance.DecreasePlayerHealth(NetworkManager.Singleton.LocalClientId, 10);
        
        GameEvents.OnAttackEnd.Invoke();
        var netObj = other.GetComponent<AttackCard>();
        if (netObj != null && netObj.IsSpawned)
        {
            netObj.DespawnGameObject();
        }
        DisableCollider();
    }
    
}
