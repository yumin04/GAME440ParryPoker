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


    [Header("Parameters")]
    [SerializeField] private float cameraTransitionTime = 4f;
    
    [Header("Player Data")]
    private NetworkList<int> playerCardIds = new();

    #region Debugging Purpose Only
    [Header("Debug Only")]
    [SerializeField] private List<int> debugPlayerCardIds = new();
    private void OnCardsChanged(NetworkListEvent<int> changeEvent)
    {
        SyncDebugList();
    }
    private void SyncDebugList()
    {
        debugPlayerCardIds.Clear();

        foreach (var id in playerCardIds)
            debugPlayerCardIds.Add(id);
    }
    #endregion

    
    // TODO: This happens when "start client" and "start host" has been called.
    // Some parts of the logic needs to be changed
    public override void OnNetworkSpawn()
    {
        // TODO: ONLY FOR DEBUG. Delete
        playerCardIds.OnListChanged += OnCardsChanged;
        // Delete.
        
        playerCamera.gameObject.SetActive(IsOwner);
        // position and rotation determined by the server
        SetPlayerPosition();
    }

    public void OnEnable()
    {
        GameEvents.OnRoundStart += MoveCameraToFullView;
        GameEvents.OnHavingPriority += HavePriority;
        GameEvents.OnLosingPriority += LosePriority;

        GameEvents.OnPlayerKeepCard += OnPlayerKeepCard;
    }

    public void OnDisable()
    {
        GameEvents.OnRoundStart -= MoveCameraToFullView;
        GameEvents.OnHavingPriority -= HavePriority;
        GameEvents.OnLosingPriority -= LosePriority;

        GameEvents.OnPlayerKeepCard -= OnPlayerKeepCard;
    }
    
    
    #region PrivateMethods
    private void SetPlayerPosition()
    {
        // only server changes position
        if (!IsServer) return;
        
        if (OwnerClientId == 0)
        {
            transform.position = new Vector3(-10, 10, 0);
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.position = new Vector3(10, 10, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    #endregion
    
    # region CameraMovement
    
    private void MoveCameraToFullView()
    {
        playerCamera.transform.SetParent(null);
        StartCoroutine(MoveCameraCoroutine(cameraTransitionTime));
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


    private void HavePriority()
    {
        if (!IsOwner) return;
        
        Debug.Log("Have Priority");
        
        ShowPriorityUI();
    }


    private void LosePriority()
    {
        if (!IsOwner) return;
        Debug.Log("Lost Priority");
        
        ShowWaitingUI();
        // TODO: maybe "waiting for opponent" or smth like this
    }

    private void ShowWaitingUI()
    {
        waitPanel.SetActive(true);
    }

    private void ShowPriorityUI()
    { 
        priorityPanel.SetActive(true);
    }

    private void OnPlayerKeepCard(ulong clientId, int cardId)
    {
        if (OwnerClientId != clientId) return;

        Debug.Log($"Player {clientId} received card {cardId}");

        playerCardIds.Add(cardId);
        
        // UpdateHandUI(cardId);
    }
    

    private void UpdateHandUI(int cardId)
    {
        playerHand.AddCard(cardId);
    }
}