using SOFile;
using Unity.Netcode;
using UnityEngine;

namespace GeneralScripts.Card {
	public class Card : NetworkBehaviour {
		public NetworkVariable<int> cardId = new();

		[SerializeField] private CardDataSO cardData;
		private static readonly int Slice = Shader.PropertyToID("_Slice");

		public void Init(int id) {
			cardId.Value = id;
			UpdateTexture();
		}


		public void OnEnable() {
			GameEvents.HideAllInstantiatedCards += HideCard;
			GameEvents.DestroyAllInstantiatedCards += DestroyCard;
			cardId.OnValueChanged += OnCardDataChanged;
			GameEvents.OnKeepClicked += TryDespawn;
		}

		public void OnDisable() {
			GameEvents.HideAllInstantiatedCards -= HideCard;
			GameEvents.DestroyAllInstantiatedCards -= DestroyCard;
			cardId.OnValueChanged -= OnCardDataChanged;
			GameEvents.OnKeepClicked -= TryDespawn;
		}

		// TODO: Refactor this so Card does not need to know this maybe?
		private void OnCardDataChanged(int previousValue, int newValue) {
			cardData = CardManager.Instance.GetCardByID(newValue);
			UpdateTexture();
		}

		private void TryDespawn() {
			// if (!IsOwner) return;
			DespawnCardRPC();
		}

		[Rpc(SendTo.Server)]
		private void DespawnCardRPC(RpcParams rpcParams = default) {
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
			propertyBlock.SetFloat(Slice, (int)cardData.cardSymbol * 4 + cardData.cardNumber);

			GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
		}
	}
}