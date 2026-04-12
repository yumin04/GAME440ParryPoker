using System.Collections;
using ScriptableObjectFile;
using UnityEngine;

namespace GeneralScripts.Card {
	public class PlayerCard : MonoBehaviour {
		[SerializeField] private CardDataScriptableObject cardData;
		private int cardId;
		private static readonly int Slice = Shader.PropertyToID("_Slice");

		public void Init(int id) {
			cardId = id;
			StartCoroutine(InitRoutine());
		}

		// TODO: We don't need this; once we show the match and animation, it's clear that the hand will be shown later
		private IEnumerator InitRoutine() {
			while (!CardManager.Instance) yield return null;

			cardData = CardManager.Instance.GetCardByID(cardId);
			UpdateTexture();
		}

		private void UpdateTexture() {
			var propertyBlock = new MaterialPropertyBlock();
			// hack for managing the suit values, some changes will have to be made I'm sure

			propertyBlock.SetFloat(Slice, (int)cardData.cardSymbol * 13 + (cardData.cardNumber - 1));

			GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
		}
	}
}
