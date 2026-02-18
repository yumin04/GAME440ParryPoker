using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class SubRound : NetworkBehaviour
{
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 3f;

    public NetworkVariable<int> CardID = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            StartCoroutine(SubRoundRoutine());
    }
    public void Initialize(int id) { if (IsServer) CardID.Value = id; }

    private IEnumerator SubRoundRoutine()
    {
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);

        if (IsServer)
            InstantiateCardClientRpc(CardID.Value);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void InstantiateCardClientRpc(int id)
    {
        CardManager.Instance.InstantiateSubRoundCard(id);
    }
}
