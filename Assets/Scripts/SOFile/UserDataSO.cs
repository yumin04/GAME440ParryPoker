using System.Collections.Generic;
using UnityEngine;


namespace SOFile
{
    [CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserDataSO")]
    public class UserDataSO : ScriptableObject
    {
        [Header("hand")]
        public List<CardDataSO> cards;

        // public HandRank myHandRank;
        public void ResetData()
        {
            cards.Clear();
            // myHandRank = HandRank.HighCard;
            Debug.Log("[UserDataSO] Reset complete");
        }
    }
    
}