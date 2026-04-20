using System.Collections;
using GenericHelpers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace GeneralScripts.Card {
	public class ClickableCard : Card {
		[FormerlySerializedAs("CurrentPriorityClientId")]
		public NetworkVariable<ulong> currentPriorityClientId = new();

		public NetworkVariable<bool> isClickable = new(true);

		private void OnMouseDown() {
			Debug.Log($"LOCAL CLICK by client {NetworkManager.Singleton.LocalClientId}");
			ClickRpc();
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void ClickRpc(RpcParams rpcParams = default) {
			var senderId = rpcParams.Receive.SenderClientId;

			if (!isClickable.Value) return;

			isClickable.Value = false;

			currentPriorityClientId.Value = senderId;

			BroadcastPriorityClientRpc(senderId);
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void BroadcastPriorityClientRpc(ulong newPriorityId) {
			var localId = NetworkManager.Singleton.LocalClientId;

			if (localId == newPriorityId) {
				GameEvents.OnHavingPriority?.Invoke();
				StartCoroutine(MoveToPriorityPosition());
			}
			else {
				GameEvents.OnLosingPriority?.Invoke();
			}
		}

		private IEnumerator MoveToPriorityPosition() {
			var target = GameParameters.CardPriorityPosition;

			var startPos = transform.position;
			var startRot = transform.rotation;
			var startScale = transform.localScale;

			const float duration = 1f;
			var elapsed = 0f;

			while (elapsed < duration) {
				var t = elapsed / duration;

				transform.position = Vector3.Lerp(startPos, target.position, t);
				transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
				transform.localScale = Vector3.Lerp(startScale, target.localScale, t);

				elapsed += Time.deltaTime;
				yield return null;
			}

			transform.position = target.position;
			transform.rotation = target.rotation;
			transform.localScale = target.localScale;
		}
	}
}
