using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : NetworkBehaviour
{
    [Header("Game Objects")]
    [SerializeField] Camera playerCamera;
    [SerializeField] private GameObject priorityPanel;
    [SerializeField] private GameObject waitPanel;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private SlotMachineInstantiator slotMachineSpawner;

    [SerializeField] private Attack attackObject;
    [SerializeField] private Defend defendObject;
    [SerializeField] private CardCollider cardCollider;

    
    // TODO: Need Better Time
    [Header("Parameters")]
    private float cameraTransitionTime = 1f;
    
    [Header("Player Data")]
    private NetworkList<int> playerCardIds = new();


    private bool isAttacking;
    

    [Header("Debug Only")]
    [SerializeField] private List<int> debugPlayerCardIds = new();
    

    // TODO: This happens when "start client" and "start host" has been called.
    // Some parts of the logic needs to be changed
    public override void OnNetworkSpawn()
    {
        playerCardIds.OnListChanged += OnCardsChanged;
        
        isAttacking = false;
        
        playerCamera.gameObject.SetActive(IsOwner);
        // position and rotation determined by the server
        SetPlayerPosition();
    }    

    public void OnEnable()
    {
        GameEvents.OnRoundStart += MoveCameraToFullView;
        GameEvents.OnRoundStart += ResetPlayer;
        GameEvents.OnHavingPriority += HavePriority;
        GameEvents.OnLosingPriority += LosePriority;

        GameEvents.OnPlayerKeepCard += OnPlayerKeepCard;
        
        GameEvents.OnAttackStart += MoveCameraToOriginal;
        GameEvents.OnAttackStart += StartAttackOrDefend;
        GameEvents.OnAttackStart += ClearHand;
        GameEvents.OnSlotMachineFinished += TriggerAfterSlotMachineRpc;
    
        GameEvents.OnAttackEnd += MoveCameraToFullViewAfterAttack;
        GameEvents.OnAttackEnd += DisplayCards;
    }



    public void OnDisable()
    {
        GameEvents.OnRoundStart -= MoveCameraToFullView;
        GameEvents.OnRoundStart -= ResetPlayer;
        GameEvents.OnHavingPriority -= HavePriority;
        GameEvents.OnLosingPriority -= LosePriority;

        GameEvents.OnPlayerKeepCard -= OnPlayerKeepCard;
        
        GameEvents.OnAttackStart -= MoveCameraToOriginal;
        GameEvents.OnAttackStart -= StartAttackOrDefend;
        GameEvents.OnAttackStart -= ClearHand;
        
        GameEvents.OnSlotMachineFinished -= TriggerAfterSlotMachineRpc;
        GameEvents.OnAttackEnd -= MoveCameraToFullViewAfterAttack;
        GameEvents.OnAttackEnd -= DisplayCards;
    }

    private void ResetPlayer()
    {
        if (!IsServer) return;
        // Reset Card
        playerCardIds.Clear();
    }

    // RPC로 보내야 하는듯?
    private void MoveCameraToOriginal()
    {
        MoveCameraToOriginalRPC();
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void MoveCameraToOriginalRPC()
    {
        // TODO: instead of this, 포지션 다시 static variable + class 로 마무리
        Vector3 originalPosition = transform.position;
        
        // Current Camera position (x,y) = (+-8,7)
        originalPosition.x = originalPosition.x * 0.8f;
        originalPosition.y += 8.5f;
        
        playerCamera.transform.position = originalPosition;
        playerCamera.transform.rotation = transform.rotation;
    }
    
    #region PrivateMethods
    private void SetPlayerPosition()
    {
        if (!IsServer) return;
        
        if (OwnerClientId == 0)
        {
            transform.position = new Vector3(-10, -2, 1);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.position = new Vector3(10, -2, 1);
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    
    #endregion
    
    # region CameraMovement
    
    private void MoveCameraToFullView()
    {
        playerCamera.transform.SetParent(null);
        StartCoroutine(MoveCameraCoroutine(cameraTransitionTime));
    }
    
    
    
    private void MoveCameraToFullViewAfterAttack()
    {
        MoveCameraToFullViewAfterAttackRPC();

    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void MoveCameraToFullViewAfterAttackRPC()
    {
        StartCoroutine(MoveCameraCoroutine(2f));        
    }

    private IEnumerator MoveCameraCoroutine(float duration)
    {
        Transform cam = playerCamera.transform;
        Transform target = GameParameters.FullViewCameraPoint;

        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            cam.position = Vector3.Lerp(startPos, target.position, t);
            cam.rotation = Quaternion.Slerp(startRot, target.rotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        cam.position = target.position;
        cam.rotation = target.rotation;
    }

    #endregion


    private void StartAttackOrDefend()
    {
        if (!IsOwner) return;
        
        Debug.Log("[DEBUG] Start Attack Or Defend");
        Debug.Log("[DEBUG] Inside StartAttackOrDefend isAttacking: "+ isAttacking.ToString());
        if (isAttacking)
        {
            Debug.Log("[DEBUG] Instantiating Slot Machine");
            // Spawn Slot Machine
            slotMachineSpawner.InstantiateSlotMachine();
        }
        else
        {
            // Waiting for slot machine...
            Debug.Log("[DEBUG] Waiting for Slot Machine");
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerAfterSlotMachineRpc()
    {
        TriggerAfterSlotMachine();
    }
    
    private void TriggerAfterSlotMachine()
    {
        if (!IsOwner) return;
        
        if (isAttacking)
        {
            slotMachineSpawner.DestroySlotMachine();
            ActivateAttack();
        }
        else
        {
            ActivateDefend();
            EnableCardCollider();
        }
    }

    private void EnableCardCollider()
    {
        if (cardCollider != null)
        {
            cardCollider.EnableCollider();
        }
    }

    private void ActivateAttack()
    {
        if (attackObject == null) return;
        
        attackObject.StartAttack(-1 * (transform.position.x / 2));
    }

    private void ActivateDefend()
    {
        // nothing here yet?
        if (defendObject == null) return;
        // TODO:
        // SlotMachine Instantiate해서 그거 하고 있다는거 indicate해주고
        // Card Dispenser Instantiate해서 곧 쏠거라는거 알려주고 해야함
        
        // defendObject.gameObject.SetActive(true);
        // defendObject.PlayDefendAnimation();
    }
    
    private void OnCardsChanged(NetworkListEvent<int> changeEvent)
    {
        // TODO: [DEBUG] Delete
        SyncDebugList();

        DisplayCards();

    }
    private void SyncDebugList()
    {
        debugPlayerCardIds.Clear();

        foreach (var id in playerCardIds)
            debugPlayerCardIds.Add(id);
    }
    private void DisplayCards()
    {
        DisplayCardsRPC();
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void DisplayCardsRPC()
    {
        if (!IsOwner) return;
        
        List<int> copy = new List<int>();

        for (int i = 0; i < playerCardIds.Count; i++)
        {
            copy.Add(playerCardIds[i]);
        }

        Debug.Log("[DEBUG] Display Cards");
        playerHand.DisplayCards(copy);   
    }

    private void ClearHand()
    {
        playerHand.Clear();
    }
    

    
    private void OnPlayerKeepCard(ulong clientId, int cardId)
    {
        if (OwnerClientId != clientId) return;

        Debug.Log($"Player {clientId} received card {cardId}");

        playerCardIds.Add(cardId);
        
    }
    
    // DONE
    #region Priority Panel

    private void HavePriority()
    {
        if (!IsOwner) return;
        
        isAttacking = true;
        
        Debug.Log("Have Priority");

        ShowPriorityUI();
        Debug.Log("isAttacking: "+ isAttacking.ToString());
    }


    private void LosePriority()
    {
        if (!IsOwner) return;
        
        isAttacking = false;
        
        Debug.Log("Lost Priority");
        
        ShowWaitingUI();
        // TODO: maybe "waiting for opponent" or smth like this
        Debug.Log("isAttacking: "+ isAttacking.ToString());
    }

    private void ShowWaitingUI()
    {
        waitPanel.SetActive(true);
    }

    private void ShowPriorityUI()
    { 
        priorityPanel.SetActive(true);
    }
    

    #endregion

    public void SpawnAttackCard(Vector3 startPos, Vector3 endPos)
    {
        if (IsServer)
        {
            CardManager.Instance.InstantiateAttackCard(startPos, endPos);
        }
        else
        {
            SpawnAttackCardRPC(startPos, endPos);
        }
    }
    
    
    [Rpc(SendTo.Server)]
    private void SpawnAttackCardRPC(Vector3 startPos, Vector3 endPos)
    {
        CardManager.Instance.InstantiateAttackCard(startPos, endPos);
    }
}