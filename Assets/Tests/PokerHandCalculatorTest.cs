using GenericHelpers;
using NUnit.Framework;
using PokerSystem;

// MethodName_StateUnderTest_ExpectedResult
namespace Tests {
	[TestFixture]
	public class PokerHandCalculatorTest {
		private CardData[] allCards;

/* ids: 1 ~ 13 clubs A - K
 * ids: 14 ~ 26 diamonds A - K
 * ids: 27 ~ 39 hearts A - K
 * ids: 40 ~ 52 spades A - K
 */
		private CardData[] GetHandDataByID(int[] cardIDs) {
			CardData[] handData = new CardData[cardIDs.Length];
			for (var i = 0; i < cardIDs.Length; i++) {
				handData[i] = allCards[cardIDs[i]];
			}

			return handData;
		}

		[SetUp]
		public void Setup() {
			allCards = new CardData[53];

			for (var i = 1; i <= 52; i++) {
				allCards[i] = new CardData {
					cardID = i, cardSymbol = (Suit)((i - 1) / 13), cardNumber = ((i - 1) % 13) + 1
				};
			}
		}

		#region Poker Result Test

		[TestCase(new [] { 40, 41, 42, 43, 44 }, 0x912345)]
		[TestCase(new [] { 1, 14, 27, 40, 5 }, 0x811115)]
		[TestCase(new [] { 1, 14, 27, 2, 15 }, 0x711122)]
		[TestCase(new [] { 1, 3, 5, 7, 9 }, 0x613579)]
		[TestCase(new [] { 3, 17, 31, 45, 7 }, 0x534567)]
		[TestCase(new [] { 1, 14, 27, 5, 9 }, 0x411159)]
		[TestCase(new [] { 1, 14, 27, 5, 9 }, 0x411159)]
		[TestCase(new [] { 1, 14, 2, 15, 9 }, 0x311229)]
		[TestCase(new [] { 1, 14, 3, 5, 9 }, 0x211359)]
		[TestCase(new [] { 1, 3, 5, 30, 11 }, 0x11354B)]
		public void GetPokerResult_RandomHand_ReturnCorrectHandRank(int[] ids, int expected) {
			var handData = GetHandDataByID(ids);
			var result = PokerHandCalculator.GetPokerResult(handData);
			Assert.AreEqual(expected, result);
		}

		#endregion

		#region All Hand Test

		[TestCase(new [] { 1, 2, 3, 4, 5, 18, 31, 44 }, HandRank.StraightFlush)] // also Four of a kind
		[TestCase(new [] { 1, 14, 27, 2, 15, 16, 17, 31, 32, 20 },
			HandRank.FullHouse)] // also straight and also flush
		public void CalculatePokerHand_RandomHand_ReturnCorrectHandRank(int[] ids, HandRank expected) {
			CardData[] handData = GetHandDataByID(ids);
			HandRank result = PokerHandCalculator.CalculatePokerHand(handData);
			Assert.AreEqual(expected, result);
		}

// Straight Flush
		[TestCase(new [] { 1, 5, 3, 4, 2 }, HandRank.StraightFlush)]
		[TestCase(new [] { 17, 18, 19, 20, 21 }, HandRank.StraightFlush)]
		[TestCase(new [] { 27, 28, 29, 30, 31 }, HandRank.StraightFlush)]
		[TestCase(new [] { 40, 41, 42, 43, 44 }, HandRank.StraightFlush)]
		[TestCase(new [] { 1, 14, 5, 40, 27 }, HandRank.FourOfAKind)]
		[TestCase(new [] { 1, 14, 27, 2, 15 }, HandRank.FullHouse)]
		[TestCase(new [] { 1, 3, 5, 7, 9 }, HandRank.Flush)]
		[TestCase(new [] { 3, 17, 31, 45, 7 }, HandRank.Straight)]
		[TestCase(new [] { 1, 14, 9, 5, 27 }, HandRank.ThreeOfAKind)]
		[TestCase(new [] { 1, 14, 2, 15, 9 }, HandRank.TwoPair)]
		[TestCase(new [] { 1, 14, 3, 5, 9 }, HandRank.OnePair)]
		[TestCase(new [] { 1, 3, 5, 30, 11 }, HandRank.HighCard)]
		public void CalculatePokerHand_5ExactHand_ReturnCorrectHandRank(int[] ids, HandRank expected) {
			var handData = GetHandDataByID(ids);
			var result = PokerHandCalculator.CalculatePokerHand(handData);
			Assert.AreEqual(expected, result);
		}

		#endregion

		#region Straight Flush

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		public void CheckStraightFlush_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckStraightFlush(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 6 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })]
		public void CheckStraightFlush_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckStraightFlush(hand));
		}

		#endregion

		#region Straight

		[TestCase(new [] { 3, 17, 31, 45, 7 })]
		[TestCase(new [] { 32, 4, 18, 46, 8, 13, 12, 26 })] // 4,5,6,7,8
		[TestCase(new [] { 5, 19, 33, 47, 9, 13, 12, 26 })] // 5,6,7,8,9
		[TestCase(new [] { 6, 20, 34, 48, 10, 27, 11, 26 })] // 6,7,8,9,10
		[TestCase(new [] { 7, 21, 35, 49, 11, 15, 26 })] // 7,8,9,10,11
		public void CheckStraight_StraightHand_ReturnTrue(int[] ids) {
			CardData[] handData = GetHandDataByID(ids);
			bool result = PokerHandCalculator.CheckStraight(handData);
			Assert.IsTrue(result);
		}

		[TestCase(new [] { 9, 22, 35, 48, 11 })]
		[TestCase(new [] { 3, 16, 29, 42, 7 })]
		[TestCase(new [] { 11, 24, 37, 50, 13 })]
		[TestCase(new [] { 6, 19, 32, 45, 9 })]
		[TestCase(new [] { 8, 21, 34, 47, 12 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckStraight_NotStraightHand_ReturnFalse(int[] ids) {
			CardData[] handData = GetHandDataByID(ids);
			bool result = PokerHandCalculator.CheckStraight(handData);
			Assert.IsFalse(result);
		}

		#endregion

		#region Flush

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 1, 7, 8, 13, 2 })]
		public void CheckFlush_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckFlush(hand));
		}

		[TestCase(new [] { 1, 14, 2, 15, 3 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckFlush_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckFlush(hand));
		}

		#endregion

		#region Four Of A Kind

		[TestCase(new [] { 1, 14, 27, 40, 5 })]
		public void CheckFourOfAKind_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckFourOfAKind(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckFourOfAKind_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckFourOfAKind(hand));
		}

		#endregion

		#region Full House

		[TestCase(new [] { 1, 14, 27, 2, 15 })]
		public void CheckFullHouse_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckFullHouse(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckFullHouse_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckFullHouse(hand));
		}

		#endregion

		#region Three of a Kind

		[TestCase(new [] { 1, 14, 27, 2, 3 })]
		public void CheckThreeOfAKind_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckThreeOfAKind(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckThreeOfAKind_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckThreeOfAKind(hand));
		}

		#endregion

		#region Two Pair

		[TestCase(new [] { 1, 14, 2, 15, 3 })]
		public void CheckTwoPair_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckTwoPair(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckTwoPair_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckTwoPair(hand));
		}

		#endregion

		#region One Pair

		[TestCase(new [] { 1, 14, 2, 3, 4 })]
		public void CheckOnePair_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckOnePair(hand));
		}

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new int[] { })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckOnePair_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckOnePair(hand));
		}

		#endregion

		#region High Card

		[TestCase(new [] { 1, 2, 3, 4, 5 })]
		[TestCase(new [] { 8, 2 })]
		[TestCase(new [] { 8, 2, 25, 46 })] // 8, 2, 12, 7
		public void CheckHighCard_ReturnTrue(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsTrue(PokerHandCalculator.CheckHighCard(hand));
		}

		[TestCase(new [] { 1, 14, 2, 15, 3 })]
		[TestCase(new [] { 8, 2, 21 })]
		[TestCase(new [] { 1, 14 })]
		[TestCase(new [] { 8, 2, 25, 47 })]
		public void CheckHighCard_ReturnFalse(int[] ids) {
			var hand = GetHandDataByID(ids);

			Assert.IsFalse(PokerHandCalculator.CheckHighCard(hand));
		}

		#endregion
	}
}
