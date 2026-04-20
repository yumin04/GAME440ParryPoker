using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectFile {
	[CreateAssetMenu(fileName = "TableData", menuName = "ScriptableObjects/GameDataSO")]
	public class GameDataScriptableObject : ScriptableObject {
		[Header("Hand")]
		public List<CardDataScriptableObject> cards;
		public Sprite cardBackImage;

		[Header("Game Data")]
		public int roundRemaining;
		public int maxRound = 10;
		public int cardVisibleDuration = 15;

		public void ResetDataForGame() {
			cards.Clear();
			roundRemaining = maxRound;
			Debug.Log("[" + name + "] Reset complete");
		}
	}
}
