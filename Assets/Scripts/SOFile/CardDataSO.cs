using UnityEngine;

namespace SOFile
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardDataSO")]
    public class CardDataSO : ScriptableObject
    {
        [Header("Card Info")] 
        public int cardID;
        public Suit cardSymbol;
        public int cardNumber;
        public Texture cardMaterial;
        public int baseDamage;
    }
    
}