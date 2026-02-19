using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SOFile;

// -----------------------------------------------------------------------------
// Minimal poker hand helper — one script, no new assets.
// Works with your existing CardDataSO (cardNumber 1–13, cardSymbol Suit).
// Use from anywhere: PokerHandEvaluator.GetHandRank(someListOfCards).
// -----------------------------------------------------------------------------

/// <summary>Standard poker hand ranks. Use with UserDataSO.myHandRank when you wire it up.</summary>
public enum HandRank
{
    HighCard,
    Pair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush
}

/// <summary>Evaluates a 5-card hand from CardDataSO list. No GameObjects or prefabs needed.</summary>
public static class PokerHandEvaluator
{
    // Card numbers: 1 = Ace, 2–10, 11 = J, 12 = Q, 13 = K. Ace can be high (14) for straights.

    /// <summary>Returns the best hand rank for the given cards. Expects 5 cards; if not 5, returns HighCard.</summary>
    public static HandRank GetHandRank(List<CardDataSO> cards)
    {
        if (cards == null || cards.Count != 5)
            return HandRank.HighCard;

        // Get numeric values (Ace = 14 for comparison, 1 for wheel)
        int[] values = cards.Select(c => c.cardNumber == 1 ? 14 : c.cardNumber).ToArray();
        int[] suits = cards.Select(c => (int)c.cardSymbol).ToArray();

        bool flush = suits.All(s => s == suits[0]);
        System.Array.Sort(values);

        bool straight = IsStraight(values);
        bool straightFlush = flush && straight;

        if (straightFlush) return HandRank.StraightFlush;

        // Count each rank (e.g. how many 2s, 3s, ...)
        var counts = new Dictionary<int, int>();
        foreach (int v in values)
        {
            if (!counts.ContainsKey(v)) counts[v] = 0;
            counts[v]++;
        }
        int[] countList = counts.Values.OrderByDescending(x => x).ToArray();

        if (countList[0] == 4) return HandRank.FourOfAKind;
        if (countList[0] == 3 && countList.Length >= 2 && countList[1] >= 2) return HandRank.FullHouse;
        if (flush) return HandRank.Flush;
        if (straight) return HandRank.Straight;
        if (countList[0] == 3) return HandRank.ThreeOfAKind;
        if (countList[0] == 2 && countList.Length >= 2 && countList[1] == 2) return HandRank.TwoPair;
        if (countList[0] == 2) return HandRank.Pair;

        return HandRank.HighCard;
    }

    /// <summary>Checks if sorted values form a straight (including A-2-3-4-5 wheel).</summary>
    private static bool IsStraight(int[] sortedValues)
    {
        // Normal straight (e.g. 10-J-Q-K-A)
        bool normal = true;
        for (int i = 1; i < sortedValues.Length; i++)
        {
            if (sortedValues[i] != sortedValues[i - 1] + 1)
            {
                normal = false;
                break;
            }
        }
        if (normal) return true;

        // Wheel: A-2-3-4-5 (Ace as 1)
        bool hasAce = sortedValues.Any(v => v == 14);
        bool hasTwo = sortedValues.Any(v => v == 2);
        if (!hasAce || !hasTwo) return false;
        int[] wheel = sortedValues.Select(v => v == 14 ? 1 : v).ToArray();
        System.Array.Sort(wheel);
        for (int i = 1; i < wheel.Length; i++)
            if (wheel[i] != wheel[i - 1] + 1)
                return false;
        return true;
    }
}
