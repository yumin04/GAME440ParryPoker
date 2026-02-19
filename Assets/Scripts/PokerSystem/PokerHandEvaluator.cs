using System;
using System.Collections.Generic;

namespace PokerSystem
{
    /// <summary>Evaluates best poker hand from 2–12 cards. Accepts List&lt;PokerCardData&gt; or PokerCardSetSO.Cards.</summary>
    public static class PokerHandEvaluator
    {
        private const int MinCards = 2;
        private const int MaxCards = 12;
        private const int HandSize = 5;

        /// <summary>Evaluate best hand. Throws ArgumentException if cards is null or count not in [2, 12].</summary>
        public static PokerHandResult EvaluateBest(IReadOnlyList<PokerCardData> cards)
        {
            if (cards == null)
                throw new ArgumentException("cards cannot be null", nameof(cards));
            if (cards.Count < MinCards || cards.Count > MaxCards)
                throw new ArgumentException($"cards count must be {MinCards} to {MaxCards}, got {cards.Count}", nameof(cards));

            if (cards.Count < HandSize)
                return EvaluatePartial(cards);
            return EvaluateBestFiveCardHand(cards);
        }

        // --- Partial hand (2, 3, or 4 cards): duplicates only, no straight/flush ---
        private static PokerHandResult EvaluatePartial(IReadOnlyList<PokerCardData> cards)
        {
            List<PokerCardData> list = CopyAndSortDesc(cards);
            int[] values = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                values[i] = list[i].NumericValue;

            // Count by value (2..14)
            int[] countByValue = new int[15];
            for (int i = 0; i < values.Length; i++)
                countByValue[values[i]]++;

            HandRank rank;
            List<int> tieBreak = new List<int>();

            if (cards.Count == 2)
            {
                if (countByValue[values[0]] == 2) { rank = HandRank.OnePair; tieBreak.Add(values[0]); }
                else { rank = HandRank.HighCard; tieBreak.Add(values[0]); tieBreak.Add(values[1]); }
            }
            else if (cards.Count == 3)
            {
                int trip = -1, pair = -1, high = -1;
                for (int v = 14; v >= 2; v--)
                {
                    if (countByValue[v] == 3) trip = v;
                    else if (countByValue[v] == 2) pair = v;
                    else if (countByValue[v] == 1 && high == -1) high = v;
                }
                if (trip >= 0) { rank = HandRank.ThreeOfAKind; tieBreak.Add(trip); }
                else if (pair >= 0) { rank = HandRank.OnePair; tieBreak.Add(pair); tieBreak.Add(high); }
                else { rank = HandRank.HighCard; for (int v = 14; v >= 2; v--) if (countByValue[v] == 1) tieBreak.Add(v); }
            }
            else // 4 cards
            {
                int quad = -1, trip = -1, pair1 = -1, pair2 = -1, high = -1;
                for (int v = 14; v >= 2; v--)
                {
                    if (countByValue[v] == 4) quad = v;
                    else if (countByValue[v] == 3) trip = v;
                    else if (countByValue[v] == 2) { if (pair1 < 0) pair1 = v; else pair2 = v; }
                    else if (countByValue[v] == 1 && high == -1) high = v;
                }
                if (quad >= 0) { rank = HandRank.FourOfAKind; tieBreak.Add(quad); tieBreak.Add(high); }
                else if (trip >= 0) { rank = HandRank.ThreeOfAKind; tieBreak.Add(trip); tieBreak.Add(high); }
                else if (pair1 >= 0 && pair2 >= 0) { rank = HandRank.TwoPair; tieBreak.Add(pair1); tieBreak.Add(pair2); tieBreak.Add(high); }
                else if (pair1 >= 0) { rank = HandRank.OnePair; tieBreak.Add(pair1); AddKickers(countByValue, pair1, 2, tieBreak); }
                else { rank = HandRank.HighCard; for (int v = 14; v >= 2; v--) if (countByValue[v] == 1) tieBreak.Add(v); }
            }

            return new PokerHandResult(rank, list, tieBreak);
        }

        private static void AddKickers(int[] countByValue, int exclude, int count, List<int> tieBreak)
        {
            int added = 0;
            for (int v = 14; v >= 2 && added < count; v--)
            {
                if (v != exclude && countByValue[v] > 0) { tieBreak.Add(v); added++; }
            }
        }

        // --- 5-card: generate all C(n,5) combos, evaluate each, return best ---
        private static PokerHandResult EvaluateBestFiveCardHand(IReadOnlyList<PokerCardData> cards)
        {
            int n = cards.Count;
            PokerHandResult best = null;
            // Indices for the 5 cards we choose
            int[] idx = new int[HandSize];

            for (int i0 = 0; i0 <= n - 5; i0++)
            {
                idx[0] = i0;
                for (int i1 = i0 + 1; i1 <= n - 4; i1++)
                {
                    idx[1] = i1;
                    for (int i2 = i1 + 1; i2 <= n - 3; i2++)
                    {
                        idx[2] = i2;
                        for (int i3 = i2 + 1; i3 <= n - 2; i3++)
                        {
                            idx[3] = i3;
                            for (int i4 = i3 + 1; i4 <= n - 1; i4++)
                            {
                                idx[4] = i4;
                                PokerCardData[] five = new PokerCardData[HandSize];
                                for (int j = 0; j < HandSize; j++)
                                    five[j] = cards[idx[j]];
                                PokerHandResult r = EvaluateFive(five);
                                if (best == null || r.CompareTo(best) > 0)
                                    best = r;
                            }
                        }
                    }
                }
            }

            return best;
        }

        /// <summary>Evaluate exactly 5 cards: flush, straight, duplicate groups. Ace low for A-2-3-4-5 (wheel).</summary>
        private static PokerHandResult EvaluateFive(PokerCardData[] five)
        {
            int[] values = new int[5];  // numeric 2..14 (Ace=14)
            int[] suits = new int[5];
            for (int i = 0; i < 5; i++)
            {
                values[i] = five[i].NumericValue;
                suits[i] = five[i].Suit;
            }

            bool flush = suits[0] == suits[1] && suits[0] == suits[2] && suits[0] == suits[3] && suits[0] == suits[4];

            // Straight: sort and check consecutive. Ace can be low (1) for A-2-3-4-5 (wheel, high=5).
            int[] sorted = (int[])values.Clone();
            Array.Sort(sorted);
            int straightHigh = GetStraightHigh(sorted);
            bool straight = straightHigh > 0;

            bool royal = straight && flush && straightHigh == 14;
            if (royal)
                return BuildResult(HandRank.RoyalFlush, five, new List<int> { 14 });

            if (flush && straight)
                return BuildResult(HandRank.StraightFlush, five, new List<int> { straightHigh });

            // Duplicate groups: count each value
            int[] countByValue = new int[15];
            for (int i = 0; i < 5; i++)
                countByValue[values[i]]++;

            int quadVal = -1, tripVal = -1, pairVal1 = -1, pairVal2 = -1;
            for (int v = 14; v >= 2; v--)
            {
                if (countByValue[v] == 4) quadVal = v;
                else if (countByValue[v] == 3) tripVal = v;
                else if (countByValue[v] == 2) { if (pairVal1 < 0) pairVal1 = v; else pairVal2 = v; }
            }

            if (quadVal >= 0)
            {
                int kicker = 0;
                for (int v = 14; v >= 2; v--) if (countByValue[v] == 1) { kicker = v; break; }
                return BuildResult(HandRank.FourOfAKind, five, new List<int> { quadVal, kicker });
            }
            if (tripVal >= 0 && pairVal1 >= 0)
                return BuildResult(HandRank.FullHouse, five, new List<int> { tripVal, pairVal1 });
            if (flush)
            {
                var desc = new List<int>();
                for (int v = 14; v >= 2; v--) for (int c = 0; c < countByValue[v]; c++) desc.Add(v);
                return BuildResult(HandRank.Flush, five, desc);
            }
            if (straight)
                return BuildResult(HandRank.Straight, five, new List<int> { straightHigh });
            if (tripVal >= 0)
            {
                var tie = new List<int> { tripVal };
                AddKickers(countByValue, tripVal, 2, tie);
                return BuildResult(HandRank.ThreeOfAKind, five, tie);
            }
            if (pairVal1 >= 0 && pairVal2 >= 0)
            {
                int kicker = 0;
                for (int v = 14; v >= 2; v--) if (countByValue[v] == 1) { kicker = v; break; }
                return BuildResult(HandRank.TwoPair, five, new List<int> { pairVal1, pairVal2, kicker });
            }
            if (pairVal1 >= 0)
            {
                var tie = new List<int> { pairVal1 };
                AddKickers(countByValue, pairVal1, 3, tie);
                return BuildResult(HandRank.OnePair, five, tie);
            }
            {
                var desc = new List<int>();
                for (int v = 14; v >= 2; v--) for (int c = 0; c < countByValue[v]; c++) desc.Add(v);
                return BuildResult(HandRank.HighCard, five, desc);
            }
        }

        /// <summary>If sorted (asc) forms a straight, return high card value (5 for wheel). Else 0.</summary>
        private static int GetStraightHigh(int[] sorted)
        {
            // Normal: 5 consecutive (sorted is ascending)
            bool ok = true;
            for (int i = 1; i < 5; i++)
                if (sorted[i] != sorted[i - 1] + 1) { ok = false; break; }
            if (ok) return sorted[4];

            // Wheel: A-2-3-4-5 (Ace as 1)
            int[] wheel = new int[5];
            for (int i = 0; i < 5; i++)
                wheel[i] = sorted[i] == 14 ? 1 : sorted[i];
            Array.Sort(wheel);
            for (int i = 1; i < 5; i++)
                if (wheel[i] != wheel[i - 1] + 1) return 0;
            return 5; // high card of wheel is 5
        }

        private static PokerHandResult BuildResult(HandRank rank, PokerCardData[] five, List<int> tieBreak)
        {
            List<PokerCardData> best = CopyAndSortDesc(five);
            return new PokerHandResult(rank, best, tieBreak);
        }

        private static List<PokerCardData> CopyAndSortDesc(IReadOnlyList<PokerCardData> cards)
        {
            List<PokerCardData> list = new List<PokerCardData>(cards.Count);
            for (int i = 0; i < cards.Count; i++)
                list.Add(cards[i]);
            list.Sort((a, b) => b.NumericValue.CompareTo(a.NumericValue));
            return list;
        }
    }
}
