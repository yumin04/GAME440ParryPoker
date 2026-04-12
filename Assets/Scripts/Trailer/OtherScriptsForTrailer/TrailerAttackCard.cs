using UnityEngine;

namespace Trailer.OtherScriptsForTrailer {
	public class TrailerAttackCard : MonoBehaviour {
		private Transform endPos;

		[SerializeField] private float moveSpeed = 20f;

		private bool isInitialized = false;
		private float startTime;

		public void Init(Transform startPos, Transform endPos) {
			this.endPos = endPos;

			transform.position = startPos.position;

			// 시작할 때 바라보게
			var dir = endPos.position - transform.position;
			dir.y = 0f;
			var yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
			startTime = Time.time;
			isInitialized = true;
		}

		private void Update() {
			if (!isInitialized) return;

			MoveTowardsTarget();
		}

		private void MoveTowardsTarget() {
			var direction = (endPos.position - transform.position).normalized;

			transform.position += direction * (moveSpeed * Time.deltaTime);

			// 계속 바라보게 (선택)
			var dir = endPos.position - transform.position;
			dir.y = 0f;

			if (dir.sqrMagnitude > 0.001f) {
				var yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
			}

			// 도착 체크
			if (!(Vector3.Distance(transform.position, endPos.position) < 0.1f)) return;
			transform.position = endPos.position;
			isInitialized = false;
			var elapsed = Time.time - startTime;
			Debug.Log($"[TrailerCard] Travel Time: {elapsed:F2} seconds");
		}
	}
}
