using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Game Objects")]
    [SerializeField] Camera playerCamera;

    [Header("Parameters")]
    [SerializeField] private float cameraTransitionTime = 4f;
    
    // TODO: This happens when "start client" and "start host" has been called.
    // Some parts of the logic needs to be changed
    public override void OnNetworkSpawn()
    {
        playerCamera.gameObject.SetActive(IsOwner);
        // position and rotation determined by the server
        SetPlayerPosition();

    }

    public void OnEnable()
    {
        // TODO: We will be changing the camera view was we show what number of round we are in
        GameEvents.OnRoundStart += MoveCameraToFullView;
    }
    

    public void OnDisable()
    {
        GameEvents.OnRoundStart -= MoveCameraToFullView;
    }
    
    
    #region PrivateMethods
    private void SetPlayerPosition()
    {
        // only server changes position
        if (!IsServer) return;
        
        if (OwnerClientId == 0)
        {
            transform.position = new Vector3(0, 0, -10);
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.position = new Vector3(0, 0, 10);
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

    
}