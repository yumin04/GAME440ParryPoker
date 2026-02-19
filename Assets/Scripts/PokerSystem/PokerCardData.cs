using System;
using UnityEngine;

namespace PokerSystem
{
    /// <summary>Single card: value + suit as int. Suit: 0=Spades, 1=Hearts, 2=Diamonds, 3=Clubs.</summary>
    [Serializable]
    public struct PokerCardData
    {
        public CardValue Value;
        public int Suit;

        /// <summary>Numeric value for comparison (Ace=14 high). For A-2-3-4-5 wheel use 1.</summary>
        public int NumericValue => Value == CardValue.Ace ? 14 : (int)Value;

        /// <summary>Numeric value when Ace is low (for wheel straight).</summary>
        public int NumericValueAceLow => Value == CardValue.Ace ? 1 : (int)Value;
    }
}
