using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Player {
	public class Player : NetworkBehaviour {
		[Header("Game Objects")]
		[SerializeField] private Camera playerCamera;
		[SerializeField] private GameObject priorityPanel;
		[SerializeField] private GameObject waitPanel;

		[Header("Parameters")]
		[SerializeField] private float cameraTransitionTime = 4f;

		[Header("Player Data")]
		[SerializeField] private int[] playerCardIds;

		// TODO: This happens when "start client" and "start host" has been called.
		// Some parts of the logic needs to be changed
		public override void OnNetworkSpawn() {
			playerCamera.gameObject.SetActive(IsOwner);
			var spotlight = GetComponentInChildren<Light>();
			spotlight.color = OwnerClientId == NetworkManager.LocalClientId ? Color.blue : Color.red;
			// position and rotation determined by the server
			SetPlayerPosition();
		}

		public void OnEnable() {
			// TODO: We will be changing the camera view was we show what number of round we are in
			GameEvents.OnRoundStart += MoveCameraToFullView;
			GameEvents.OnHavingPriority += HavePriority;
			GameEvents.OnLosingPriority += LosePriority;
		}

		public void OnDisable() {
			GameEvents.OnRoundStart -= MoveCameraToFullView;
			GameEvents.OnHavingPriority -= HavePriority;
			GameEvents.OnLosingPriority -= LosePriority;
		}

		#region PrivateMethods

		private void SetPlayerPosition() {
			// only server changes position
			if (!IsServer) return;

			if (OwnerClientId == 0) {
				transform.position = new Vector3(-10, 10, 0);
				transform.rotation = Quaternion.identity;
			}
			else {
				transform.position = new Vector3(10, 10, 0);
				transform.rotation = Quaternion.Euler(0, 180, 0);
			}
		}

		#endregion

		# region CameraMovement

		private void MoveCameraToFullView() {
			playerCamera.transform.SetParent(null);
			StartCoroutine(MoveCameraCoroutine(cameraTransitionTime));
		}

		private IEnumerator MoveCameraCoroutine(float duration) {
			var cam = playerCamera.transform;
			var target = GameParameters.FullViewCameraPoint;

			var startPos = cam.position;
			var startRot = cam.rotation;

			var elapsed = 0f;

			while (elapsed < duration) {
				var t = elapsed / duration;

				cam.position = Vector3.Lerp(startPos, target.position, t);
				cam.rotation = Quaternion.Slerp(startRot, target.rotation, t);

				elapsed += Time.deltaTime;
				yield return null;
			}

			cam.position = target.position;
			cam.rotation = target.rotation;
		}

		#endregion


		private void HavePriority() {
			if (!IsOwner) return;

			Debug.Log("Have Priority");

			ShowPriorityUI();
		}

		private void LosePriority() {
			if (!IsOwner) return;
			Debug.Log("Lost Priority");

			ShowWaitingUI();
			// TODO: maybe "waiting for opponent" or smth like this
		}

		private void ShowWaitingUI() {
			waitPanel.SetActive(true);
		}

		private void ShowPriorityUI() {
			priorityPanel.SetActive(true);
		}
	}
}