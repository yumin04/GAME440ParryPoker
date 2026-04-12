using GenericHelpers;
using SOFile;
using Unity.Netcode;
using UnityEngine;

namespace GeneralScripts.Card {
	public class CardManager : Singleton<CardManager> {
		private readonly float[] xPositions = { -3f, -1.5f, 0f, 1.5f, 3f };

		private readonly float[] zPositions = { 0f, 2f };

		private const float Y_POSITION = 2.4f;

		private const float X_MIN_BOUNDARY = -4.5f;
		private const float X_MAX_BOUNDARY = 4.5f;

		private const float Z_MIN_BOUNDARY = -0.5f;
		private const float Z_MAX_BOUNDARY = 2.5f;
		private readonly Quaternion cardShowInTableRotation = Quaternion.Euler(180f, 180f, 0f);
		private readonly Quaternion cardHideInTableRotation = Quaternion.Euler(0f, 0f, 0f);

		[SerializeField] private CardInstantiator cardInstantiator;
		[SerializeField] private CardRepository cardRepository;

		// public void Start()
		// {
		//     // DebuggingCardTextures();
		// }
		private void DebuggingCardTextures() {
			var position = new Vector3();
			for (var i = 1; i <= 52; i++) {
				var cardData = cardRepository.GetCardByID(i);
				cardInstantiator.InstantiateCard(cardData, position, Quaternion.identity);
				position.x += 1f;
			}
		}

		public CardDataSO[] GetCards(int numCards) {
			var roundCards = new CardDataSO[numCards];

			for (var i = 0; i < numCards; i++) {
				roundCards[i] = cardRepository.GetRandomCard();
			}

			return roundCards;
		}

		public CardDataSO GetCardByID(int newValue) {
			return cardRepository.GetCardByID(newValue);
		}

		public void InstantiateAttackCard(Vector3 startPosition, Vector3 endPosition) {
			cardInstantiator.SpawnAttackCard(startPosition, endPosition);
		}

		// DONE

		#region RoundCards

		public static void HideAllRoundCards() {
			// GameEvents.HideAllInstantiatedCards.Invoke();
			GameEvents.DestroyAllInstantiatedCards.Invoke();
		}

		public void InstantiateRoundCardsByID(NetworkList<int> ids) {
			var cards = new CardDataSO[ids.Count];

			for (var i = 0; i < ids.Count; i++) {
				cards[i] = cardRepository.GetCardByID(ids[i]);
			}

			InstantiateRoundCards(cards);
		}

		public void InstantiateRoundCardsByID(int[] ids) {
			var cards = new CardDataSO[ids.Length];

			for (var i = 0; i < ids.Length; i++) {
				cards[i] = cardRepository.GetCardByID(ids[i]);
			}

			InstantiateRoundCards(cards);
		}

		private void InstantiateRoundCards(CardDataSO[] roundCards) {
			var index = 0;
			foreach (var x in xPositions) {
				foreach (var z in zPositions) {
					var spawnPos = new Vector3(x, Y_POSITION, z);
					cardInstantiator.SpawnCard(roundCards[index], spawnPos, cardShowInTableRotation);
					index++;
				}
			}
		}

		#endregion

		#region SubRoundCardOnTable

		public void InstantiateSubRoundCard(int cardId) {
			Vector3 randomPosition = GenerateRandomXZPosition();
			CardDataSO subRoundCard = cardRepository.GetCardByID(cardId);
			// Generate Random Position
			cardInstantiator.SpawnClickableCard(subRoundCard, randomPosition, cardHideInTableRotation);
		}

		private static Vector3 GenerateRandomXZPosition() {
			var x = Random.Range(X_MIN_BOUNDARY, X_MAX_BOUNDARY);
			var z = Random.Range(Z_MIN_BOUNDARY, Z_MAX_BOUNDARY);

			return new Vector3(x, Y_POSITION, z);
		}

		#endregion
	}
}
