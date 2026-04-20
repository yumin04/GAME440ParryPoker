using System.Collections.Generic;
using ScriptableObjectFile;
using UnityEngine;

namespace GeneralScripts.Card {
	public class CardRepository : MonoBehaviour {
		public List<CardDataScriptableObject> allCardData = new ();

		private readonly HashSet<int> usedCardIds = new();
		[SerializeField] private List<int> debugUsedCardIds;

		public void OnEnable() {
			GameEvents.OnRoundEnd += Reset;
		}

		public void OnDisable() {
			GameEvents.OnRoundEnd -= Reset;
		}

		private void LoadAllCardData() {
			allCardData.Clear();

			string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };

			foreach (var suit in suits) {
				var loaded = Resources.LoadAll<CardDataScriptableObject>($"CardData/{suit}");
				allCardData.AddRange(loaded);
			}
		}

		public CardDataScriptableObject GetRandomCard() {
			if (allCardData.Count == 0) LoadAllCardData();

			if (usedCardIds.Count >= allCardData.Count) {
				Debug.LogWarning("All cards have been used.");
				return null;
			}

			CardDataScriptableObject card;

			do {
				card = allCardData[Random.Range(0, allCardData.Count)];
			} while (usedCardIds.Contains(card.cardID));

			usedCardIds.Add(card.cardID);
			debugUsedCardIds = new List<int>(usedCardIds);
			return card;
		}

		public void Reset() {
			usedCardIds.Clear();
		}

		public CardDataScriptableObject GetCardByID(int id) {
			if (allCardData.Count == 0) LoadAllCardData();

			foreach (var card in allCardData) {
				if (card.cardID == id) return card;
			}

			Debug.LogError($"Card with ID {id} not found.");
			return null;
		}
	}
}
