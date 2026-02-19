using System;
using System.Collections.Generic;

namespace PokerSystem
{
    /// <summary>Result of evaluating a hand: rank, best cards, and tie-break values (lexicographic compare).</summary>
    public class PokerHandResult
    {
        public HandRank Rank;
        /// <summary>Winning 5 cards (or all cards if &lt;5). Sorted desc by value.</summary>
        public List<PokerCardData> BestCards;
        /// <summary>Tie-break values: compare first differing int (higher wins).</summary>
        public List<int> TieBreak;

        public PokerHandResult(HandRank rank, List<PokerCardData> bestCards, List<int> tieBreak)
        {
            Rank = rank;
            BestCards = bestCards ?? new List<PokerCardData>();
            TieBreak = tieBreak ?? new List<int>();
        }

        /// <summary>Compare two results: better hand returns &gt; 0, worse &lt; 0, same = 0.</summary>
        public int CompareTo(PokerHandResult other)
        {
            if (other == null) return 1;
            // Lower enum value = better (RoyalFlush=1, HighCard=10)
            int rankCompare = ((int)other.Rank).CompareTo((int)Rank);
            if (rankCompare != 0) return rankCompare;

            // Tie-break: first differing int, higher wins
            int maxLen = TieBreak != null && other.TieBreak != null
                ? Math.Min(TieBreak.Count, other.TieBreak.Count)
                : 0;
            for (int i = 0; i < maxLen; i++)
            {
                int c = TieBreak[i].CompareTo(other.TieBreak[i]);
                if (c != 0) return c;
            }
            return (TieBreak?.Count ?? 0).CompareTo(other.TieBreak?.Count ?? 0);
        }
    }
}
