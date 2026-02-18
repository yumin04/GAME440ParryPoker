using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ClickableCard : Card
{
    public NetworkVariable<ulong> CurrentPriorityClientId = new NetworkVariable<ulong>();
    public NetworkVariable<bool> isClickable = new(true);

    private void OnMouseDown()
    {
        Debug.Log($"LOCAL CLICK by client {NetworkManager.Singleton.LocalClientId}");
        ClickRpc();

    }



    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ClickRpc(RpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;

        if (!isClickable.Value) return;

        isClickable.Value = false;

        CurrentPriorityClientId.Value = senderId;

        BroadcastPriorityClientRpc(senderId);
    }



    [Rpc(SendTo.ClientsAndHost)]
    private void BroadcastPriorityClientRpc(ulong newPriorityId)
    {
        ulong localId = NetworkManager.Singleton.LocalClientId;

        if (localId == newPriorityId)
        {
            GameEvents.OnHavingPriority?.Invoke();
            StartCoroutine(MoveToPriorityPosition());
        }
        else
        {
            GameEvents.OnLosingPriority?.Invoke();
        }
    }
    private IEnumerator MoveToPriorityPosition()
    {
        Transform target = GameParameters.CardPriorityPosition;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            transform.localScale = Vector3.Lerp(startScale, target.localScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
        transform.localScale = target.localScale;
    }


}