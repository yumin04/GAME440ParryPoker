using ScriptableObjectFile;
using Unity.Netcode;
using UnityEngine;

namespace GeneralScripts.Card {
	public class CardInstantiator : MonoBehaviour {
		[SerializeField] private GameObject cardPrefab;
		[SerializeField] private GameObject clickableCardPrefab;
		[SerializeField] private GameObject attackCardPrefab;

		// TODO: Decide whether to get CardID instead of Card Data maybe?
		public void SpawnCard(CardDataScriptableObject cardData, Vector3 position, Quaternion rotation) {
			var obj = Instantiate(cardPrefab, position, rotation);

			var netObj = obj.GetComponent<NetworkObject>();
			netObj.Spawn(true);

			var card = obj.GetComponent<global::Card>();
			card.Init(cardData.cardID);
		}

		public void InstantiateCard(CardDataScriptableObject cardData, Vector3 position, Quaternion rotation) {
			var obj = Instantiate(cardPrefab, position, rotation);

			var card = obj.GetComponent<global::Card>();
			card.Init(cardData.cardID);
		}

		public void SpawnClickableCard(CardDataScriptableObject cardData, Vector3 position, Quaternion rotation) {
			var obj = Instantiate(clickableCardPrefab, position, rotation);

			var netObj = obj.GetComponent<NetworkObject>();
			netObj.Spawn();

			var card = obj.GetComponent<ClickableCard>();
			card.Init(cardData.cardID);
		}

		public void SpawnAttackCard(Vector3 startPosition, Vector3 endPosition) {
			// I think I'll just bring this
			var defaultRotation = Quaternion.LookRotation(endPosition - startPosition);

			var obj = Instantiate(attackCardPrefab, startPosition, defaultRotation);

			var netObj = obj.GetComponent<NetworkObject>();
			netObj.Spawn();

			var card = obj.GetComponent<AttackCard>();
			card.Init(endPosition);

			// Would a motion trigger work here?
		}
	}
}
