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
        if (!IsServer) return;
        Debug.Log("[DEBUG] Starting Keep");
        GameEvents.OnSubRoundEnd?.Invoke();

        NetworkObject.Despawn(true);
    }
    

    private void OnAttackClicked()
    {
        Debug.Log("[DEBUG] Starting Attack");
    }
}
