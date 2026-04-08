using System.Collections;
using SOFile;
using UnityEngine;
using UnityEngine.Serialization;

public class P1GrabThenAttackCameraScript : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Timing")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float stayDuration = 1.0f;
    
    [Header("Players")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    
    [SerializeField] private Animator player1Animation;
    [SerializeField] private Animator player2Animation;
    /// <summary>
    /// anim.SetTrigger("Hit");
    /// anim.SetTrigger("Catch");
    /// anim.SetTrigger("Grab");
    /// anim.SetTrigger("Victory");
    /// anim.SetTrigger("Victory");
    /// anim.SetBool("ReadyToCatch", true);
    /// anim.SetBool("Shoot", true);
    /// anim.SetBool("ReadyToCatch", false);
    /// anim.SetBool("Shoot", false);
    /// anim.Play("Seated");
    /// </summary>
    [Header("Other Objects")] 
    // [SerializeField] private TrailerAttackCard trailerAttackCard;
    [SerializeField] private TrailerObjectInstantiator trailerObjectInstantiator;

    [SerializeField] private int subRoundNumber;
    private CardDataSO cardData;
    
     private void Start()
     {
         InitializePlayersHand();
         StartCoroutine(PlayCameraSequence());
     }

     private void InitializePlayersHand()
     {
         trailerObjectInstantiator.AddMultipleCardsToPlayer1(EnumPositionStorage<P1GrabThenAttackCameraPosition>.player1Cards);
         trailerObjectInstantiator.AddMultipleCardsToPlayer2(EnumPositionStorage<P1GrabThenAttackCameraPosition>.player2Cards);
     }

     private IEnumerator PlayCameraSequence()
     {
         foreach (P1GrabThenAttackCameraPosition pos in System.Enum.GetValues(typeof(P1GrabThenAttackCameraPosition)))
         {
            Debug.Log("[DEBUG] Position: " + pos);

            Transform target = EnumPositionStorage<P1GrabThenAttackCameraPosition>.Positions[(int)pos];

            if (target == null) continue;
            switch (pos)
            {
                case P1GrabThenAttackCameraPosition.InitializeCard:
                    trailerObjectInstantiator.DisablePlayer1Hand();
                    trailerObjectInstantiator.DisablePlayer2Hand();

                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    trailerObjectInstantiator.InstantiateSubRoundCard(EnumPositionStorage<P1GrabThenAttackCameraPosition>.roundCards[subRoundNumber]);
                    yield return new WaitForSeconds(2f);
                    break;
                case P1GrabThenAttackCameraPosition.P1Grab:
                    yield return StartCoroutine(MoveCamera(target, 0.3f));

                    string stateName = "Grab";
                    player1Animation.SetTrigger(stateName);
                    yield return StartCoroutine(WaitUntilState(stateName, player1Animation));
                    float duration = GetAnimationDuration(stateName, player1Animation);
                    yield return new WaitForSeconds(duration/2);

                    trailerObjectInstantiator.DisableCard();
                    yield return new WaitForSeconds(duration/2);
                    trailerObjectInstantiator.MoveCardToP1CheckPosition();
                    yield return new WaitForSeconds(0.2f);
                    break;
                case P1GrabThenAttackCameraPosition.P1CheckCard:
                    yield return StartCoroutine(MoveCamera(target, 0.3f));
                    // After Animation, Fast Zoom to card check position
                    trailerObjectInstantiator.EnableCard();
                    trailerObjectInstantiator.EnablePlayer1Hand();
                    trailerObjectInstantiator.EnablePlayer2Hand();
                    yield return new WaitForSeconds(0.5f);
                    break;
                
                case P1GrabThenAttackCameraPosition.P1OptionSelection:
                    yield return StartCoroutine(MoveCamera(target, 0.3f));
                    // Option Pops Up
                    // Mouse Pointer starts in between the options
                    trailerObjectInstantiator.EnableMousePointerUI();
                    yield return new WaitForSeconds(0.5f);
                    break;
                 case P1GrabThenAttackCameraPosition.P1ChoosingAttack:
                    // move the mouse toward Attack, and click
                    trailerObjectInstantiator.MoveMousePointerToAttack();
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
            
         }
         
         
         yield return new WaitForSeconds(stayDuration);
     }
     private IEnumerator WaitUntilState(string stateName, Animator animator)
     {
         while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
             yield return null;
     }
     private float GetAnimationDuration(string stateName, Animator animator)
     {
         AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

         // 아직 해당 state 아니면 fallback
         if (!stateInfo.IsName(stateName))
             return 0f;

         return stateInfo.length / animator.speed;
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
     

}