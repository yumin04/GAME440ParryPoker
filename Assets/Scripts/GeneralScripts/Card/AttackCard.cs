using SOFile;
using Unity.Netcode;
using UnityEngine;

namespace GeneralScripts.Card {
	public class AttackCard : NetworkBehaviour {
		/* TODO:
		 * I think it would be better if the card had the same texture on both the front and back
		 * It's probably just a component or a prefab
		 */

		[SerializeField] private CardDataSO cardData;

		private int cardId;
		private bool clickable;
		public NetworkVariable<Vector3> endPosition = new();

		// TODO: Correct Speed
		private const float MOVEMENT_SPEED = 15f;

		public void Init(Vector3 position) {
			endPosition.Value = position;
			Debug.Log("[DEBUG] endPosition: " + position);
		}

		public void Update() {
			transform.position = Vector3.MoveTowards(
				transform.position, endPosition.Value, MOVEMENT_SPEED * Time.deltaTime);
		}

		// Modifier pattern
		// Wouldn't it work if I used sine or cosine?
		// Attached to the flying position
		private void OnMouseDown() {
			if (!clickable) return;

			HandleCardClick();
			DespawnGameObject();
		}

		private static void HandleCardClick() {
			GameEvents.OnAttackEnd.Invoke();
		}

		public void DespawnGameObject() {
			DespawnGameObjectRPC();
		}

		[Rpc(SendTo.Server)]
		private void DespawnGameObjectRPC() {
			NetworkObject.Despawn(gameObject);
		}

		private void OnTriggerEnter(Collider other) {
			clickable = true;
		}
	}
}
