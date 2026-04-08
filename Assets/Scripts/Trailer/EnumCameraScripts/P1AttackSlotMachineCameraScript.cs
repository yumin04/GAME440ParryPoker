using System.Collections;
using SOFile;
using UnityEngine;
using UnityEngine.Serialization;

public class P1AttackSlotMachineCameraScript : MonoBehaviour
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
    
    [SerializeField] private Attack attack;


    [Header("Other Objects")] 
    // [SerializeField] private TrailerAttackCard trailerAttackCard;
    [SerializeField] private TrailerObjectInstantiator trailerObjectInstantiator;

    [SerializeField] private int subRoundNumber;
    private CardDataSO cardData;
    
     private void Start()
     {
         // InitializePlayersHand();
         StartCoroutine(PlayCameraSequence());
     }

     private void InitializePlayersHand()
     {
         trailerObjectInstantiator.AddMultipleCardsToPlayer1(EnumPositionStorage<P1AttackCameraPosition>.player1Cards);
         trailerObjectInstantiator.AddMultipleCardsToPlayer2(EnumPositionStorage<P1AttackCameraPosition>.player2Cards);
     }

     private IEnumerator PlayCameraSequence()
     {
         foreach (P1AttackCameraPosition pos in System.Enum.GetValues(typeof(P1AttackCameraPosition)))
         {
            Debug.Log("[DEBUG] Position: " + pos);

            Transform target = EnumPositionStorage<P1AttackCameraPosition>.Positions[(int)pos];

            if (target == null) continue;
            switch (pos)
            {
                case P1AttackCameraPosition.P1SlotMachine:
                    // Initialize Slot Machine
                    yield return StartCoroutine(MoveCamera(target, moveDuration));
                    trailerObjectInstantiator.InstantiateSlotMachine();
                    yield return new WaitForSeconds(2f);
                    // Spin the slot machine
                    // 하나에 안착하게 하기
                    break;
                case P1AttackCameraPosition.P1SlotMachineOptions:


                    trailerObjectInstantiator.DisableSlotMachine();
                    yield return new WaitForSeconds(0.5f);
                    break;
                case P1AttackCameraPosition.P1AimP2AndShoot:
                    // Attack부분 코드 그냥 가져오는걸로

                    attack.StartAttack(5f);
                    yield return new WaitForSeconds(5f);
                    break;
                
                case P1AttackCameraPosition.P2HitByAttack:
                    // Play Animation
                    
                    player2Animation.SetTrigger("Hit");
                    yield return StartCoroutine(WaitForAnimation());
                    // Option Pops Up
                    // Mouse Pointer starts in between the options
                    yield return new WaitForSeconds(0.5f);
                    break;
                 case P1AttackCameraPosition.P2HealthDecrease:
                    // UserInterface.Instance.ChangePlayer1Health(90);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
            
         }
         
         
         yield return new WaitForSeconds(stayDuration);
     }
     private IEnumerator WaitForAnimation()
     {
         yield return null;

         AnimatorStateInfo stateInfo = player1Animation.GetCurrentAnimatorStateInfo(0);
         float duration = stateInfo.length;
         yield return new WaitForSeconds(duration);
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