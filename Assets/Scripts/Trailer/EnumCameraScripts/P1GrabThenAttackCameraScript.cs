using System;
using System.Collections;
using Trailer.OtherScriptsForTrailer;
using UnityEngine;

namespace Trailer.EnumCameraScripts {
	public class P1GrabThenAttackCameraScript : MonoBehaviour {
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

		private void Start() {
			StartCoroutine(PlayCameraSequence());
		}

		private IEnumerator PlayCameraSequence() {
			foreach (P1GrabThenAttackCameraPosition pos in
			         System.Enum.GetValues(typeof(P1GrabThenAttackCameraPosition))) {
				Debug.Log("[DEBUG] Position: " + pos);

				var target = EnumPositionStorage<P1GrabThenAttackCameraPosition>.Positions[(int)pos];

				if (!target) continue;
				switch (pos) {
					case P1GrabThenAttackCameraPosition.InitializeCard:
						yield return StartCoroutine(MoveCamera(target, moveDuration));
						// Initialize SubRound Card
						yield return new WaitForSeconds(stayDuration);
						break;
					case P1GrabThenAttackCameraPosition.P1Grab:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// No Need to Move Camera
						// Initialize Animation
						break;
					case P1GrabThenAttackCameraPosition.P1CheckCard:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// follow Card movement, then go to Player 1's perspective
						break;

					case P1GrabThenAttackCameraPosition.P1OptionSelection:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// Option Pops Up
						// Mouse starts in between the options
						yield return new WaitForSeconds(0.5f);
						break;
					case P1GrabThenAttackCameraPosition.P1ChoosingAttack:
						yield return StartCoroutine(MoveCamera(target, 0.7f));
						// move the mouse toward Attack, and click
						yield return new WaitForSeconds(0.5f);
						blackScreen.SetActive(false);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			yield return new WaitForSeconds(stayDuration);
		}

		private IEnumerator OrbitAroundPlayer(Transform player, float duration, float maxAngle) {
			var timer = 0f;

			var startOffset = transform.position - player.position;

			var flatOffset = new Vector3(startOffset.x, 0f, startOffset.z);

			var radius = flatOffset.magnitude;

			flatOffset = flatOffset.normalized;

			// Y 고정
			var fixedY = transform.position.y;

			var initialEuler = transform.eulerAngles;

			while (timer < duration) {
				var t = timer / duration;
				t = Mathf.SmoothStep(0f, 1f, t);

				var moveAngle = Mathf.Lerp(0f, maxAngle, t);

				// 👉 시작 방향 기준으로 회전
				var rot = Quaternion.AngleAxis(moveAngle, Vector3.up);
				var dir = rot * flatOffset;

				var newPos = player.position + dir * radius;
				newPos.y = fixedY; // Y 유지

				transform.position = newPos;

				// 👉 Y rotation만 맞추기
				var lookDir = player.position - transform.position;
				lookDir.y = 0f;

				var yAngle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(initialEuler.x, yAngle, initialEuler.z);

				timer += Time.deltaTime;
				yield return null;
			}
		}

		private IEnumerator MoveCamera(Transform target, float duration) {
			var startPos = cameraTransform.position;
			var startRot = cameraTransform.rotation;

			var endPos = target.position;
			var endRot = target.rotation;

			var time = 0f;

			while (time < duration) {
				var t = time / duration;

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
}
