using GeneralScripts;
using GeneralScripts.Card;
using Unity.Netcode;
using UnityEngine;

// initialize when defend is out.

// This is the damage calculation
namespace Player {
	public class CardCollider : MonoBehaviour {
		public void EnableCollider() {
			gameObject.SetActive(true);
		}

		private void DisableCollider() {
			gameObject.SetActive(false);
		}

		public void OnTriggerEnter(Collider other) {
			// TODO: Currently Decreasing health by 10 for every hit

			Game.Instance.DecreasePlayerHealth(NetworkManager.Singleton.LocalClientId, 10);

			GameEvents.OnAttackEnd.Invoke();
			var netObj = other.GetComponent<AttackCard>();
			if (netObj && netObj.IsSpawned) {
				netObj.DespawnGameObject();
			}

			DisableCollider();
		}
	}
}
