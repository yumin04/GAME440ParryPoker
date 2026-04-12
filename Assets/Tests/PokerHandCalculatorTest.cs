using System;
using System.Collections;
using GenericHelpers;
using NUnit.Framework;
using PokerSystem;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

// MethodName_StateUnderTest_ExpectedResult
[TestFixture]
public class PokerHandCalculatorTest
{
    public CardData[] allCards;

/* ids: 1 ~ 13 clubs A - K
 * ids: 14 ~ 26 diamonds A - K
 * ids: 27 ~ 39 hearts A - K
 * ids: 40 ~ 52 spades A - K
 */
    private CardData[] GetHandDataByID(int[] cardIDs)
    {
        CardData[] handData = new CardData[cardIDs.Length];
        for (int i = 0; i < cardIDs.Length; i++)
        {
            handData[i] = allCards[cardIDs[i]];
        }

        return handData;
    }

    [SetUp]
    public void Setup()
    {
        allCards = new CardData[53];

        for (int i = 1; i <= 52; i++)
        {
            allCards[i] = new CardData
            {
                cardID = i, cardSymbol = (Suit)((i - 1) / 13), cardNumber = ((i - 1) % 13) + 1
            };
        }
    }

    #region Poker Result Test

    [TestCase(new int[] { 40, 41, 42, 43, 44 }, 0x912345)]
    [TestCase(new int[] { 1, 14, 27, 40, 5 }, 0x811115)]
    [TestCase(new int[] { 1, 14, 27, 2, 15 }, 0x711122)]
    [TestCase(new int[] { 1, 3, 5, 7, 9 }, 0x613579)]
    [TestCase(new int[] { 3, 17, 31, 45, 7 }, 0x534567)]
    [TestCase(new int[] { 1, 14, 27, 5, 9 }, 0x411159)]
    [TestCase(new int[] { 1, 14, 27, 5, 9 }, 0x411159)]
    [TestCase(new int[] { 1, 14, 2, 15, 9 }, 0x311229)]
    [TestCase(new int[] { 1, 14, 3, 5, 9 }, 0x211359)]
    [TestCase(new int[] { 1, 3, 5, 30, 11 }, 0x11354B)]
    public void GetPokerResult_RandomHand_ReturnCorrectHandRank(int[] ids, int expected)
    {
        PokerHandCalculator pokerHandCalculator = new PokerHandCalculator();
        CardData[] handData = GetHandDataByID(ids);
        int result = pokerHandCalculator.GetPokerResult(handData);
        Assert.AreEqual(expected, result);
    }

    #endregion

    #region All Hand Test

    [TestCase(new int[] { 1, 2, 3, 4, 5, 18, 31, 44 }, HandRank.StraightFlush)] // also Four of a kind
    [TestCase(new int[] { 1, 14, 27, 2, 15, 16, 17, 31, 32, 20 }, HandRank.FullHouse)] // also straight and also flush
    public void CalculatePokerHand_RandomHand_ReturnCorrectHandRank(int[] ids, HandRank expected)
    {
        PokerHandCalculator pokerHandCalculator = new PokerHandCalculator();
        CardData[] handData = GetHandDataByID(ids);
        HandRank result = pokerHandCalculator.CalculatePokerHand(handData);
        Assert.AreEqual(expected, result);
    }

// Straight Flush
    [TestCase(new int[] { 1, 5, 3, 4, 2 }, HandRank.StraightFlush)]
    [TestCase(new int[] { 17, 18, 19, 20, 21 }, HandRank.StraightFlush)]
    [TestCase(new int[] { 27, 28, 29, 30, 31 }, HandRank.StraightFlush)]
    [TestCase(new int[] { 40, 41, 42, 43, 44 }, HandRank.StraightFlush)]
    [TestCase(new int[] { 1, 14, 5, 40, 27 }, HandRank.FourOfAKind)]
    [TestCase(new int[] { 1, 14, 27, 2, 15 }, HandRank.FullHouse)]
    [TestCase(new int[] { 1, 3, 5, 7, 9 }, HandRank.Flush)]
    [TestCase(new int[] { 3, 17, 31, 45, 7 }, HandRank.Straight)]
    [TestCase(new int[] { 1, 14, 9, 5, 27 }, HandRank.ThreeOfAKind)]
    [TestCase(new int[] { 1, 14, 2, 15, 9 }, HandRank.TwoPair)]
    [TestCase(new int[] { 1, 14, 3, 5, 9 }, HandRank.OnePair)]
    [TestCase(new int[] { 1, 3, 5, 30, 11 }, HandRank.HighCard)]
    public void CalculatePokerHand_5ExactHand_ReturnCorrectHandRank(int[] ids, HandRank expected)
    {
        PokerHandCalculator pokerHandCalculator = new PokerHandCalculator();
        CardData[] handData = GetHandDataByID(ids);
        HandRank result = pokerHandCalculator.CalculatePokerHand(handData);
        Assert.AreEqual(expected, result);
    }

    #endregion

    #region Straight Flush

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    public void CheckStraightFlush_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckStraightFlush(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 6 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })]
    public void CheckStraightFlush_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckStraightFlush(hand));
    }

    #endregion

    #region Straight

    [TestCase(new int[] { 3, 17, 31, 45, 7 })]
    [TestCase(new int[] { 32, 4, 18, 46, 8, 13, 12, 26 })] // 4,5,6,7,8
    [TestCase(new int[] { 5, 19, 33, 47, 9, 13, 12, 26 })] // 5,6,7,8,9
    [TestCase(new int[] { 6, 20, 34, 48, 10, 27, 11, 26 })] // 6,7,8,9,10
    [TestCase(new int[] { 7, 21, 35, 49, 11, 15, 26 })] // 7,8,9,10,11
    public void CheckStraight_StraightHand_ReturnTrue(int[] ids)
    {
        PokerHandCalculator pokerHandCalculator = new PokerHandCalculator();
        CardData[] handData = GetHandDataByID(ids);
        bool result = pokerHandCalculator.CheckStraight(handData);
        Assert.IsTrue(result);
    }

    [TestCase(new int[] { 9, 22, 35, 48, 11 })]
    [TestCase(new int[] { 3, 16, 29, 42, 7 })]
    [TestCase(new int[] { 11, 24, 37, 50, 13 })]
    [TestCase(new int[] { 6, 19, 32, 45, 9 })]
    [TestCase(new int[] { 8, 21, 34, 47, 12 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckStraight_NotStraightHand_ReturnFalse(int[] ids)
    {
        PokerHandCalculator pokerHandCalculator = new PokerHandCalculator();
        CardData[] handData = GetHandDataByID(ids);
        bool result = pokerHandCalculator.CheckStraight(handData);
        Assert.IsFalse(result);
    }

    #endregion

    #region Flush

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 1, 7, 8, 13, 2 })]
    public void CheckFlush_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckFlush(hand));
    }

    [TestCase(new int[] { 1, 14, 2, 15, 3 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckFlush_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckFlush(hand));
    }

    #endregion

    #region Four Of A Kind

    [TestCase(new int[] { 1, 14, 27, 40, 5 })]
    public void CheckFourOfAKind_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckFourOfAKind(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckFourOfAKind_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckFourOfAKind(hand));
    }

    #endregion

    #region Full House

    [TestCase(new int[] { 1, 14, 27, 2, 15 })]
    public void CheckFullHouse_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckFullHouse(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckFullHouse_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckFullHouse(hand));
    }

    #endregion

    #region Three of a Kind

    [TestCase(new int[] { 1, 14, 27, 2, 3 })]
    public void CheckThreeOfAKind_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckThreeOfAKind(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckThreeOfAKind_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckThreeOfAKind(hand));
    }

    #endregion

    #region Two Pair

    [TestCase(new int[] { 1, 14, 2, 15, 3 })]
    public void CheckTwoPair_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckTwoPair(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckTwoPair_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckTwoPair(hand));
    }

    #endregion

    #region One Pair

    [TestCase(new int[] { 1, 14, 2, 3, 4 })]
    public void CheckOnePair_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckOnePair(hand));
    }

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckOnePair_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckOnePair(hand));
    }

    #endregion

    #region High Card

    [TestCase(new int[] { 1, 2, 3, 4, 5 })]
    [TestCase(new int[] { 8, 2 })]
    [TestCase(new int[] { 8, 2, 25, 46 })] // 8, 2, 12, 7
    public void CheckHighCard_ReturnTrue(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsTrue(calc.CheckHighCard(hand));
    }

    [TestCase(new int[] { 1, 14, 2, 15, 3 })]
    [TestCase(new int[] { 8, 2, 21 })]
    [TestCase(new int[] { 1, 14 })]
    [TestCase(new int[] { 8, 2, 25, 47 })]
    public void CheckHighCard_ReturnFalse(int[] ids)
    {
        var calc = new PokerHandCalculator();
        var hand = GetHandDataByID(ids);

        Assert.IsFalse(calc.CheckHighCard(hand));
    }

    #endregion
}