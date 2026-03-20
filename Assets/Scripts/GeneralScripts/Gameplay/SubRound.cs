using System;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class SubRound : NetworkBehaviour
{
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 3f;

    public NetworkVariable<int> CardID = new();

    public void OnEnable()
    {
        GameEvents.OnAttackClicked += OnAttackClicked;
        GameEvents.OnKeepClicked += OnKeepClicked;
    }

    public void OnDisable()
    {
        GameEvents.OnAttackClicked -= OnAttackClicked;
        GameEvents.OnKeepClicked -= OnKeepClicked;
    }
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
        CardManager.Instance.InstantiateSubRoundCard(CardID.Value);
    }
    private void OnKeepClicked()
    {
        RequestKeepRpc();
    }
    
    [Rpc(SendTo.Server)]
    private void RequestKeepRpc(RpcParams rpcParams = default)
    {
        if (!IsServer) return;

        ulong senderId = rpcParams.Receive.SenderClientId;
        
        GameEvents.OnPlayerKeepCard?.Invoke(senderId, CardID.Value);
        
        NotifySubRoundEndClientRpc();

        NetworkObject.Despawn(true);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void NotifySubRoundEndClientRpc()
    {
        GameEvents.HideWaitAndAttackPanel.Invoke();
        GameEvents.OnSubRoundEnd?.Invoke();
    }


    private void OnAttackClicked()
    {
        RequestAttackRpc();
        // Hide Attack and Wait Panel in both player
        // Move camera to attack position (this is done through Player I believe)
        // Slot machine pop up
        // Wait for player "1" pop up
    }
    [Rpc(SendTo.Server)]
    private void RequestAttackRpc(RpcParams rpcParams = default)
    {
        Debug.Log("[DEBUG] Starting Attack");
        
        if (!IsServer) return;
        
        NotifyAttackStartRpc();
        
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void NotifyAttackStartRpc()
    {
        GameEvents.HideWaitAndAttackPanel.Invoke();
        
        
        GameEvents.OnAttackStart.Invoke();
        
    }

}
