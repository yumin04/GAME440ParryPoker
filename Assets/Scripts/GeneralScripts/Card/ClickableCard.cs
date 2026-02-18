using Unity.Netcode;
using UnityEngine;

public class ClickableCard : NetworkBehaviour
{
    private void OnMouseDown()
    {
        ClickRpc();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ClickRpc()
    {
        HandleClick();
    }
    
    private void HandleClick()
    {
        Debug.Log("Not Implemented: Handle Click!");
    }
}