using UnityEngine;

namespace PokerSystem
{
    /// <summary>Optional: assign a PokerCardSetSO and it will evaluate and log on Start.</summary>
    public class PokerHandTester : MonoBehaviour
    {
        public PokerCardSetSO CardSet;

        void Start()
        {
            if (CardSet == null || CardSet.Cards == null || CardSet.Cards.Count < 2)
            {
                Debug.Log("[PokerHandTester] No CardSet or fewer than 2 cards.");
                return;
            }

            try
            {
                PokerHandResult result = PokerHandEvaluator.EvaluateBest(CardSet.Cards);
                Debug.Log($"[PokerHandTester] Rank: {result.Rank}, TieBreak: [{string.Join(", ", result.TieBreak)}], BestCards: {result.BestCards.Count}");
            }
            catch (System.Exception e)
            {
                Debug.LogError("[PokerHandTester] " + e.Message);
            }
        }
    }
}
