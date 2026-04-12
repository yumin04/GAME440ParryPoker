using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectFile {
	[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserDataSO")]
	public class UserDataScriptableObject : ScriptableObject {
		[Header("hand")]
		public List<CardDataScriptableObject> cards;

		// public HandRank myHandRank;
		public void ResetData() {
			cards.Clear();
			// myHandRank = HandRank.HighCard;
			Debug.Log("[UserDataSO] Reset complete");
		}
	}
}
