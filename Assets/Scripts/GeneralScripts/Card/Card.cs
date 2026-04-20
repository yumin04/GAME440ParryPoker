using ScriptableObjectFile;
using Unity.Netcode;
using UnityEngine;

namespace GeneralScripts.Card {
	public class Card : NetworkBehaviour {
		public NetworkVariable<int> cardId = new();

		[SerializeField] private CardDataScriptableObject cardData;
		private static readonly int Slice = Shader.PropertyToID("_Slice");

		public void Init(int id) {
			cardId.Value = id;
		}

		public void OnEnable() {
			GameEvents.HideAllInstantiatedCards += HideCard;
			GameEvents.DestroyAllInstantiatedCards += DestroyCard;
			cardId.OnValueChanged += OnCardDataChanged;

			// Can be handled by despawn since it is a networking object.
			GameEvents.OnKeepClicked += TryDespawn;
			GameEvents.OnAttackClicked += TryDespawn;
		}

		public void OnDisable() {
			GameEvents.HideAllInstantiatedCards -= HideCard;
			GameEvents.DestroyAllInstantiatedCards -= DestroyCard;
			cardId.OnValueChanged -= OnCardDataChanged;

			// Can be handled by despawn since it is a networking object.
			GameEvents.OnKeepClicked -= TryDespawn;
			GameEvents.OnAttackClicked -= TryDespawn;
		}

		// TODO: Refactor this so Card does not need to know this maybe?
		private void OnCardDataChanged(int previousValue, int newValue) {
			cardData = CardManager.Instance.GetCardByID(newValue);
			UpdateTexture();
		}

		private void TryDespawn() {
			DespawnCardRPC();
		}

		[Rpc(SendTo.Server)]
		private void DespawnCardRPC() {
			NetworkObject.Despawn();
		}

		private void HideCard() {
			// Or move to a different place
			gameObject.SetActive(false);
		}

		private void DestroyCard() {
			Destroy(gameObject);
		}

		private void ShowCard() {
			gameObject.SetActive(true);
		}

		private void UpdateTexture() {
			var propertyBlock = new MaterialPropertyBlock();
			// hack for managing the suit values, some changes will have to be made I'm sure
			propertyBlock.SetFloat(Slice, (int)cardData.cardSymbol * 13 + (cardData.cardNumber - 1));

			GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
		}
	}
}
