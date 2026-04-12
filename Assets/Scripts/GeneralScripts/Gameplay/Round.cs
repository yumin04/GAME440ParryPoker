using System.Collections;
using GeneralScripts.Card;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GeneralScripts.Gameplay {
	public class Round : NetworkBehaviour {
		private readonly NetworkList<int> roundCardIDs = new();

		// TODO: make sure we are having correct seconds
		private const float MEMORIZATION_SECONDS = 12f;

		public void OnEnable() {
			GameEvents.OnSubRoundEnd += CheckSubRoundRemaining;
		}

		public void OnDisable() {
			GameEvents.OnSubRoundEnd -= CheckSubRoundRemaining;
		}

		private void CheckSubRoundRemaining() {
			if (!IsServer) return;

			if (roundCardIDs.Count == 0) {
				Debug.Log("No more cards. Ending round.");

				var loserID = CalculateRoundWinner();

				// TODO:
				// instead of 10, have a round damage
				// or damage by hand rank?

				Game.Instance.DecreasePlayerHealth(loserID, 10);

				EndRound();
			}
			else {
				Debug.Log("Cards remaining. Starting next subround.");
				StartSubRound();
			}
		}

		private ulong CalculateRoundWinner() {
			// Stub 
			Debug.LogWarning("[STUB] No Winner Calculation Implemented, player 2 lose");
			// TODO: 
			// 서버가 각각의 player에게 hand calculation 요청
			// 그다음 hand 반환
			return 1;
		}

		private static void EndRound() {
			Debug.Log("[DEBUG] EndRound");
			GameEvents.OnRoundEnd?.Invoke();
		}

		public override void OnNetworkSpawn() {
			base.OnNetworkSpawn();
			roundCardIDs.OnListChanged += OnRoundCardsChanged;

			if (IsServer) {
				StartRound();
			}
		}

		private void OnRoundCardsChanged(NetworkListEvent<int> changeEvent) {
			UserInterface.Instance.ChangeSubRoundNumber(roundCardIDs.Count);
		}

		private void StartRound() {
			// Reset();
			Debug.Log("[DEBUG] Choosing 10 Cards");

			// Give 2 cards to each player
			Give2CardsToEachPlayer();

			Choose10RoundCards();
			Debug.Log("[DEBUG] Chose 10 Cards: " + roundCardIDs.Count);

			// Wait 10 seconds
			WaitForMemorization();
		}

		private void WaitForMemorization() {
			StartCoroutine(MemorizationCoroutine());
		}

		private IEnumerator MemorizationCoroutine() {
			if (!IsServer) yield break;

			yield return new WaitForSeconds(MEMORIZATION_SECONDS);
			RunAfterMemorizationRPC();
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void RunAfterMemorizationRPC() {
			CardManager.HideAllRoundCards();
			UserInterface.Instance.EnableSubRoundNumber();
			StartSubRound();
		}

		private void StartSubRound() {
			if (!IsServer) return;

			if (roundCardIDs.Count == 0) return;

			// TODO: UserInterface.Instance.
			var randomIndex = Random.Range(0, roundCardIDs.Count);
			var chosenCardID = roundCardIDs[randomIndex];

			roundCardIDs.RemoveAt(randomIndex);
			var subRoundObj = GameInitializer.Instance.SpawnSubRound();

			var subRound = subRoundObj.GetComponent<SubRound>();
			subRound.Initialize(chosenCardID);
		}

		private void Give2CardsToEachPlayer() {
			if (!IsServer) return;

			// Refactor?
			foreach (var client in NetworkManager.Singleton.ConnectedClientsList) {
				var clientId = client.ClientId;

				var cards = CardManager.Instance.GetCards(2);

				foreach (var card in cards) {
					GameEvents.OnPlayerKeepCard?.Invoke(clientId, card.cardID);
				}
			}
		}

		private void Choose10RoundCards() {
			if (!IsServer) return;

			roundCardIDs.Clear();

			var roundCardData = CardManager.Instance.GetCards(10);

			foreach (var card in roundCardData) {
				roundCardIDs.Add(card.cardID);
			}

			CardManager.Instance.InstantiateRoundCardsByID(roundCardIDs);
		}

		private int[] ToArray(NetworkList<int> list) {
			return list.AsNativeArray().ToArray();
		}
	}
}
