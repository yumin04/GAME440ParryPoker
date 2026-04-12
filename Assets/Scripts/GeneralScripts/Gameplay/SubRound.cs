using System.Collections;
using GeneralScripts.Card;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GeneralScripts.Gameplay {
	public class SubRound : NetworkBehaviour {
		[SerializeField] private float minDelay = 1f;
		[SerializeField] private float maxDelay = 3f;

		[FormerlySerializedAs("CardID")] public NetworkVariable<int> cardID = new();

		public void OnEnable() {
			GameEvents.OnAttackClicked += OnAttackClicked;
			GameEvents.OnKeepClicked += OnKeepClicked;

			GameEvents.OnAttackEnd += KeepCard;
		}

		public void OnDisable() {
			GameEvents.OnAttackClicked -= OnAttackClicked;
			GameEvents.OnKeepClicked -= OnKeepClicked;
			GameEvents.OnAttackEnd -= KeepCard;
		}

		public override void OnNetworkSpawn() {
			if (IsServer) StartCoroutine(SubRoundRoutine());
		}

		public void Initialize(int id) {
			if (IsServer) cardID.Value = id;
		}

		private IEnumerator SubRoundRoutine() {
			var delay = Random.Range(minDelay, maxDelay);
			yield return new WaitForSeconds(delay);
			CardManager.Instance.InstantiateSubRoundCard(cardID.Value);
		}

		#region Click Method

		private void OnAttackClicked() {
			RequestAttackRpc();
			RequestHideWaitAndAttackPanelRPC();
			// Hide Attack and Wait Panel in both player
			// Move camera to attack position (this is done through Player I believe)
			// Slot machine pop up
			// Wait for player "1" pop up
		}

		// 이거는 click, 패널부분
		private void OnKeepClicked() {
			RequestHideWaitAndAttackPanelRPC();
			KeepCard();
		}

		// 이거는 Keep도, Attack도 결국에는 부르는 function
		private void KeepCard() {
			RequestKeepRpc();
		}

		#endregion

		#region HidePanelRPC

		[Rpc(SendTo.Server)]
		private void RequestHideWaitAndAttackPanelRPC() {
			NotifyHideWaitAndAttackPanelRPC();
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void NotifyHideWaitAndAttackPanelRPC() {
			Debug.Log("[DEBUG] Inside NotifyHideWaitAndAttackPanelRPC");
			GameEvents.HideWaitAndAttackPanel.Invoke();
		}

		#endregion

		#region KeepRPC

		// Keep Card에 넣고
		[Rpc(SendTo.Server)]
		private void RequestKeepRpc(RpcParams rpcParams = default) {
			if (!IsServer) return;

			var senderId = rpcParams.Receive.SenderClientId;

			GameEvents.OnPlayerKeepCard?.Invoke(senderId, cardID.Value);

			NotifySubRoundEndClientRpc();

			NetworkObject.Despawn();
		}

		#endregion

		#region AttackRPC

		[Rpc(SendTo.Server)]
		private void RequestAttackRpc(RpcParams rpcParams = default) {
			Debug.Log("[DEBUG] Starting Attack");

			if (!IsServer) return;

			NotifyAttackStartRpc();
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void NotifyAttackStartRpc() {
			GameEvents.OnAttackStart.Invoke();
		}

		#endregion

		[Rpc(SendTo.ClientsAndHost)]
		private void NotifySubRoundEndClientRpc() {
			GameEvents.OnSubRoundEnd?.Invoke();
		}
	}
}
