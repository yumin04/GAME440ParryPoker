using System.Collections.Generic;
using UnityEngine;

namespace PokerSystem
{
    [CreateAssetMenu(menuName = "Poker/Poker Card Set", fileName = "PokerCardSetSO")]
    public class PokerCardSetSO : ScriptableObject
    {
        public List<PokerCardData> Cards = new List<PokerCardData>();
    }
}
