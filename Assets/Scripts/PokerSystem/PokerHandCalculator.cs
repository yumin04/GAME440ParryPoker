using System;
using System.Collections.Generic;
using System.Linq;
using GenericHelpers;
using SOFile;

namespace PokerSystem {
	public class PokerHandCalculator {
		private List<CardData> currentHandData = new();
		// Array.Sort(handData, (a, b) => a.cardNumber.CompareTo(b.cardNumber));
		// How to sort things

		// TODO: USED IN UNITY
		public int GetPokerResult(CardDataSO[] handData) {
			var converted = new CardData[handData.Length];
			for (var i = 0; i < handData.Length; i++) {
				converted[i].cardID = handData[i].cardID;
				converted[i].cardNumber = handData[i].cardNumber;
				converted[i].cardSymbol = handData[i].cardSymbol;
			}

			return GetPokerResult(converted);
		}

		public int GetPokerResult(CardData[] handData) {
			var rank = CalculatePokerHand(handData);
			FindBestHand(rank, handData);
			return 1;
		}

		private static void FindBestHand(HandRank rank, CardData[] handData) {
			switch (rank) {
				case HandRank.StraightFlush:
				case HandRank.FourOfAKind:
				case HandRank.FullHouse:
				case HandRank.Flush:
				case HandRank.Straight:
				case HandRank.ThreeOfAKind:
				case HandRank.TwoPair:
				case HandRank.OnePair:
				case HandRank.HighCard:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(rank), rank, null);
			}
		}

		public HandRank CalculatePokerHand(CardData[] handData) {
			if (CheckStraightFlush(handData)) return HandRank.StraightFlush;

			if (CheckFourOfAKind(handData)) return HandRank.FourOfAKind;

			if (CheckFullHouse(handData)) return HandRank.FullHouse;

			if (CheckFlush(handData)) return HandRank.Flush;

			if (CheckStraight(handData)) return HandRank.Straight;

			if (CheckThreeOfAKind(handData)) return HandRank.ThreeOfAKind;

			if (CheckTwoPair(handData)) return HandRank.TwoPair;

			if (CheckOnePair(handData)) return HandRank.OnePair;

			return HandRank.HighCard;
		}

		public bool CheckStraightFlush(CardData[] handData) {
			var suitGroups = new Dictionary<Suit, List<CardData>>();

			foreach (var card in handData) {
				if (!suitGroups.ContainsKey(card.cardSymbol)) suitGroups[card.cardSymbol] = new List<CardData>();

				suitGroups[card.cardSymbol].Add(card);
			}

			foreach (var group in suitGroups.Values) {
				if (group.Count >= 5 && CheckStraight(group.ToArray())) return true;
			}

			return false;
		}

		public bool CheckFourOfAKind(CardData[] handData) {
			var count = new Dictionary<int, int>();

			foreach (var card in handData) {
				count.TryAdd(card.cardNumber, 0);

				count[card.cardNumber]++;

				if (count[card.cardNumber] >= 4) return true;
			}

			return false;
		}

		public bool CheckFullHouse(CardData[] handData) {
			var count = new Dictionary<int, int>();

			foreach (var card in handData) {
				count.TryAdd(card.cardNumber, 0);

				count[card.cardNumber]++;
			}

			var hasThree = false;
			var hasTwo = false;

			foreach (var kvp in count) {
				switch (kvp.Value) {
					case >= 3:
						hasThree = true;
						break;
					case >= 2:
						hasTwo = true;
						break;
				}
			}

			return hasThree && hasTwo;
		}

		public bool CheckFlush(CardData[] handData) {
			var suitCount = new Dictionary<Suit, int>();

			foreach (var card in handData) {
				suitCount.TryAdd(card.cardSymbol, 0);

				suitCount[card.cardSymbol]++;

				if (suitCount[card.cardSymbol] >= 5) return true;
			}

			return false;
		}

		public bool CheckStraight(CardData[] handData) {
			var numbers = new HashSet<int>();

			foreach (var card in handData) {
				numbers.Add(card.cardNumber);
			}

			var sorted = numbers.OrderByDescending(n => n).ToList();

			for (var i = 0; i <= sorted.Count - 5; i++) {
				if (sorted[i] - sorted[i + 4] == 4) return true;
			}

			return false;
		}

		public bool CheckThreeOfAKind(CardData[] handData) {
			var count = new Dictionary<int, int>();

			foreach (var card in handData) {
				count.TryAdd(card.cardNumber, 0);

				count[card.cardNumber]++;

				if (count[card.cardNumber] >= 3) return true;
			}

			return false;
		}

		public bool CheckTwoPair(CardData[] handData) {
			var count = new Dictionary<int, int>();
			var pairCount = 0;

			foreach (var card in handData) {
				count.TryAdd(card.cardNumber, 0);

				count[card.cardNumber]++;
			}

			foreach (var kvp in count) {
				if (kvp.Value >= 2) pairCount++;
			}

			return pairCount >= 2;
		}

		public bool CheckOnePair(CardData[] handData) {
			var count = new Dictionary<int, int>();

			foreach (var card in handData) {
				count.TryAdd(card.cardNumber, 0);

				count[card.cardNumber]++;

				if (count[card.cardNumber] >= 2) return true;
			}

			return false;
		}

		public bool CheckHighCard(CardData[] handData) {
			return !CheckOnePair(handData) && !CheckTwoPair(handData) && !CheckThreeOfAKind(handData);
		}
	}
}
