using UnityEngine;

namespace ScriptableObjectFile {
	[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardDataSO")]
	public class CardDataScriptableObject : ScriptableObject {
		[Header("Card Info")]
		public int cardID;
		public Suit cardSymbol;
		public int cardNumber;
		public Texture cardMaterial;
		public int baseDamage;
	}
}
