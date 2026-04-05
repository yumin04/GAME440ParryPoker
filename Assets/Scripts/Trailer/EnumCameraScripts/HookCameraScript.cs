using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class HookCameraScript : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Timing")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float stayDuration = 1.0f;
    
    [Header("Player Positions")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    
    [Header("Other Objects")]
    [SerializeField] private TrailerAttackCard trailerAttackCard;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private GameObject blackScreen;
    
         private void Start()
     {
         StartCoroutine(PlayCameraSequence());
     }

     private IEnumerator PlayCameraSequence()
     {
         foreach (HookCameraPosition pos in System.Enum.GetValues(typeof(HookCameraPosition)))
         {
            Debug.Log("[DEBUG] Position: " + pos);

            Transform target = EnumPositionStorage<HookCameraPosition>.Positions[(int)pos];

            if (target == null) continue;
            float maxAngle;
            switch (pos)
            {
                // TODO: This can go after hook and start with the close up
                case HookCameraPosition.PlayerStareAtEachOther:
                    // Always Moving First
                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    // I liked how Maddy did this in the trailer, lets look at that or use the same one
                    yield return new WaitForSeconds(stayDuration);
                    break;
                case HookCameraPosition.P1CloseUpGrabPose:
                    // 여기 더 빠르게, 0.5초?
                    yield return StartCoroutine(MoveCamera(target, 0.3f));
                    // have the camera rotate a little bit as it close up on the Grab Pose
                    // Wait하면서 움직이면서 Player은 계속 비추게끔
                    // 그러니까 원형으로 도는거지?
                    maxAngle = 7f;
                    yield return StartCoroutine(OrbitAroundPlayer(player1.transform, 2f, maxAngle));
                    break;
                case HookCameraPosition.P2CloseUpShootPose:
                    // Same as above
                    yield return StartCoroutine(MoveCamera(target, 0.3f));
                    maxAngle = -7f;
                    yield return StartCoroutine(OrbitAroundPlayer(player2.transform, 2f, maxAngle));
                    break;
                
                case HookCameraPosition.P2ShootCard:
                    yield return StartCoroutine(MoveCamera(target, 0.3f));
                    // Front Face of the Shooting and animation to Shoot
                    yield return new WaitForSeconds(0.5f);
                    Debug.Log("[DEBUG] Animation Play: Shoot CARD");
                    // Start Moving The Card
                    // Initialize Card Here?
                    // Two Position,
                        // Start Position on the Gun
                        // End Position On the Player1 Catch
                    trailerAttackCard.Init(startPos, endPos);
                    Debug.Log("[DEBUG] After Init");
                    // Wait for it to pass close to the Camera
                    yield return new WaitForSeconds(0.5f);
                    break;
                case HookCameraPosition.FollowCard:
                    // New Coroutine, Follow Card
                    // TODO: Second Position?
                    trailerAttackCard.Init(startPos, endPos);
                    yield return StartCoroutine(FollowCard(target, 0.7f));
                    // After the card moves towards the camera
                    // Follow along with the Card
                    // 여기에 Sudden BlackOut
                    Debug.Log("[DEBUG] Animation Play: P1 Catch");
                    blackScreen.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    blackScreen.SetActive(false);
                    break;
                case HookCameraPosition.P1CatchCard:
                    // TODO: Second Position?
                    trailerAttackCard.Init(startPos, endPos);
                    yield return StartCoroutine(MoveCamera(target, 0f));
                    // show Catch Animation
                    // 여기서 뭐 Animator.PlayCatchAnimation();
                    // 이런거, 그리고 나서 끝?
                    Debug.Log("[DEBUG] Animation Play: P1 Catch");
                    yield return new WaitForSeconds(stayDuration);
                    trailerAttackCard.gameObject.SetActive(false);
                    break;

                case HookCameraPosition.P1ShowHand:
                    // Activate Both Hands
                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    // 대각선 위에 카메라
                    Debug.Log("[DEBUG] Animation Play: P1 Lay Card");
                    yield return new WaitForSeconds(stayDuration);
                    break;
                case HookCameraPosition.P2ShowHand:
                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    Debug.Log("[DEBUG] Animation Play: P2 Lay Card");
                    // 대각선 위에 카메라
                    yield return new WaitForSeconds(stayDuration);
                    // Deactivate Both Hands
                    break;
                case HookCameraPosition.Pose:
                    Debug.Log("[DEBUG] Animation Play: P1 Win");
                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    // P1 Win pose
                    yield return new WaitForSeconds(stayDuration);
                    break;
            }
            
         }
         
         
         yield return new WaitForSeconds(stayDuration);
     }

     private IEnumerator OrbitAroundPlayer(Transform player, float duration, float maxAngle)
     {
         float timer = 0f;

         Vector3 startOffset = transform.position - player.position;
         
         Vector3 flatOffset = new Vector3(startOffset.x, 0f, startOffset.z);
         
         float radius = flatOffset.magnitude;
         
         flatOffset = flatOffset.normalized;

         // Y 고정
         float fixedY = transform.position.y;

         Vector3 initialEuler = transform.eulerAngles;

         while (timer < duration)
         {
             float t = timer / duration;
             t = Mathf.SmoothStep(0f, 1f, t);

             float moveAngle = Mathf.Lerp(0f, maxAngle, t);

             // 👉 시작 방향 기준으로 회전
             Quaternion rot = Quaternion.AngleAxis(moveAngle, Vector3.up);
             Vector3 dir = rot * flatOffset;

             Vector3 newPos = player.position + dir * radius;
             newPos.y = fixedY; // Y 유지

             transform.position = newPos;

             // 👉 Y rotation만 맞추기
             Vector3 lookDir = player.position - transform.position;
             lookDir.y = 0f;

             float yAngle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
             transform.rotation = Quaternion.Euler(initialEuler.x, yAngle, initialEuler.z);

             timer += Time.deltaTime;
             yield return null;
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
     
     private IEnumerator FollowCard(Transform card, float duration)
     {
         float timer = 0f;

         transform.rotation = card.rotation;
         // 카메라와 카드 사이 offset 유지

         while (timer < duration)
         {
             transform.position = card.position;

             timer += Time.deltaTime;
             yield return null;
         }
     }
}