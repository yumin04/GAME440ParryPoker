using System.Collections.Generic;
using UnityEngine;


namespace SOFile
{
    [CreateAssetMenu(fileName = "TableData", menuName = "ScriptableObjects/MatchDataSO")]
    public class MatchDataSO : ScriptableObject
    {
        // TODO: Decide if this should go to player data ?
        public int playerHealth = 100;
        public int opponentHealth = 100;

        
        public void ResetDataForMatch()
        {
            playerHealth = 100;
            opponentHealth = 100;
            Debug.Log("["+this.name + "] Reset complete");
            
        }
    }
    
}