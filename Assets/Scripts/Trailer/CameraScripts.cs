using UnityEngine;
using System.Collections;

public class CameraScripts : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] private Transform cameraTransform;
    
    
    
    [Header("Timing")]
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float stayDuration = 1.0f;
    
    [Header("Scripts")]
    [SerializeField] private TrailerObjectInstantiator trailerObjectInstantiator;
    [SerializeField] private PlayerObjectSwitcher playerObjectSwitcher;
    
    [Header("Player Position")]
    [SerializeField] private Transform player1Transform;
    [SerializeField] private Transform player2Transform;
    
    [SerializeField] private Transform player1Grab;
    [SerializeField] private Transform player2Grab;

    [SerializeField] private Transform player1Lay;
    [SerializeField] private Transform player2Lay;
    
    private int[] ids = new[] { 1, 2, 3, 4 };
    private bool isCardInitialized = false;
    private GameObject pose;
    
    
    private void Start()
    {
        StartCoroutine(PlayCameraSequence());
    }

    private IEnumerator PlayCameraSequence()
    {
        foreach (CameraPosition pos in System.Enum.GetValues(typeof(CameraPosition)))
        {
            Debug.Log("[DEBUG] Position: " + pos);

            Transform target = TrailerCameraPosition.cameraPositions[(int)pos];

            if (target == null) continue;
            
            yield return StartCoroutine(MoveCamera(target, moveDuration));
            
            switch (pos)
            {
                case CameraPosition.P1Hit_39:
                case CameraPosition.P1Hit_33:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogHit, player1Transform);
                    playerObjectSwitcher.DeactivatePlayer1();
                    break;
                case CameraPosition.P2HitByAttack_17:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogHit, player2Transform);
                    playerObjectSwitcher.DeactivatePlayer2();
                    break;
                
                case CameraPosition.P1AimP2AndShoot_16:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogShoot, player1Transform);
                    playerObjectSwitcher.DeactivatePlayer1();
                    break;
                
                case CameraPosition.P2AimP1AndShoot_32:
                case CameraPosition.P2AimP1AndShoot_38:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogShoot, player2Transform);
                    playerObjectSwitcher.DeactivatePlayer2();
                    break;
                
                case CameraPosition.P1CatchCard_5:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.cardCatch, player1Transform);
                    playerObjectSwitcher.DeactivatePlayer1();
                    break;
                case CameraPosition.P1Grab_10:
                    
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogGrab, player1Grab);
                    playerObjectSwitcher.DeactivatePlayer1();
                    break;
                
                
                case CameraPosition.P2Grab_28_2:
                case CameraPosition.P2Grab_34_2:
                case CameraPosition.P2Grab_23:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.frogGrab, player2Grab);
                    playerObjectSwitcher.DeactivatePlayer2();
                    break;
                
                case CameraPosition.P1ShowHand_6:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.handLay, player1Lay);
                    playerObjectSwitcher.DeactivatePlayer1();
                    break;
                case CameraPosition.P2ShowHand_7:
                    pose = playerObjectSwitcher.SwapPose(playerObjectSwitcher.handLay, player2Lay);
                    playerObjectSwitcher.DeactivatePlayer2();
                    break;
                
                
                case CameraPosition.InitializeSubRoundCard_22:
                case CameraPosition.InitializeCard_34_1:
                case CameraPosition.InitializeCard_9_1:
                case CameraPosition.InitializeCard_28_1:
                    trailerObjectInstantiator.InstantiateSubRoundCard(ids[0]);
                    isCardInitialized = true;
                    break;
                
                case CameraPosition.Memorize_9:
                    trailerObjectInstantiator.InstantiateRoundCardsByID(ids);
                    isCardInitialized = true;
                    break;
                default:
                    break;
                
            }
            yield return new WaitForSeconds(stayDuration);
            playerObjectSwitcher.ActivatePlayer1();
            playerObjectSwitcher.ActivatePlayer2();
            if (isCardInitialized)
            {
                GameEvents.DestroyAllInstantiatedCards?.Invoke();
                isCardInitialized = false;
            }
            
            if (pose != null) Destroy(pose);

        }
    }

    private IEnumerator MoveCamera(Transform target, float duration)
    {
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            cameraTransform.position = Vector3.Lerp(startPos, endPos, t);
            cameraTransform.rotation = Quaternion.Slerp(startRot, endRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 마지막 보정
        cameraTransform.position = endPos;
        cameraTransform.rotation = endRot;
    }
}