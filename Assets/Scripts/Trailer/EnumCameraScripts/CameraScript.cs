using System.Collections;
using UnityEngine;

namespace Trailer.EnumCameraScripts {
	public class CameraScript : MonoBehaviour {
		[Header("Main Camera")]
		[SerializeField] private Transform cameraTransform;

		[Header("Timing")]
		[SerializeField] protected float moveDuration = 0.5f;
		[SerializeField] protected float stayDuration = 1.0f;

		[Header("Player Positions")]
		[SerializeField] protected GameObject player1;
		[SerializeField] protected GameObject player2;

		[Header("Black Screen")]
		[SerializeField] protected GameObject blackScreen;

		protected IEnumerator MoveCamera(Transform target, float duration) {
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

		protected IEnumerator OrbitAroundPlayer(Transform player, float duration, float maxAngle) {
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
	}
}
