using UnityEngine;

namespace ScriptableObjectFile {
	[CreateAssetMenu(menuName = "Round/RoundData")]
	public class RoundDataScriptableObject : ScriptableObject {
		public CardDataScriptableObject subRoundCard;
	}
}
