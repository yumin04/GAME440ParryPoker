using System.Collections.Generic;
using UnityEngine;


namespace SOFile
{
    [CreateAssetMenu(fileName = "TableData", menuName = "ScriptableObjects/GameDataSO")]
    public class GameDataSO : ScriptableObject
    {
        [Header("hand")]
        public List<CardDataSO> cards;

        public Sprite cardBackImage;

        [Header("game data")] 
        public int roundRemaining;
        public int maxRound = 10;
        public int cardVisibleDuration = 15;



        
        public void ResetDataForGame()
        {
            cards.Clear();
            roundRemaining = maxRound;
            Debug.Log("["+this.name + "] Reset complete");
            
        }
    }
    
}