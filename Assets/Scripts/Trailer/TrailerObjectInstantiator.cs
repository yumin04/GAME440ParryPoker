using System.Collections;
using GeneralScripts.Card;
using ScriptableObjectFile;
using Trailer.OtherScriptsForTrailer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Trailer {
	public class TrailerObjectInstantiator : MonoBehaviour {
		[SerializeField] private GameObject slotMachine;
		[SerializeField] private GameObject crossHair;

		private readonly float[] xPositions = { -4.5f, -3f, -1.5f, 0f, 1.5f };

		private readonly float[] zPositions = { 0f, 2f };

		private const float Y_POSITION = -0.3f;

		private const float X_MIN_BOUNDARY = -5.5f;
		private const float X_MAX_BOUNDARY = 2f;

		private const float Z_MIN_BOUNDARY = -0.5f;
		private const float Z_MAX_BOUNDARY = 2.5f;

		[FormerlySerializedAs("Card")]
		[SerializeField] private GameObject cardObject;
		[SerializeField] private CardRepository cardRepository;

		public void InstantiateRoundCardsByID(int[] ids) {
			var cards = new CardDataScriptableObject[ids.Length];

			for (var i = 0; i < ids.Length; i++) {
				cards[i] = cardRepository.GetCardByID(ids[i]);
			}

			Debug.Log("[DEBUG] Cards: " + cards.Length);
			StartCoroutine(InstantiateRoundCards(cards));
		}

		private IEnumerator InstantiateRoundCards(CardDataScriptableObject[] roundCards) {
			var index = 0;
			foreach (var x in xPositions) {
				foreach (var z in zPositions) {
					var spawnPos = new Vector3(x, Y_POSITION + 1f, z + 0.5f);
					// TODO: 이거 카드 뒷면이고
					// 여기서 Animate하는걸로
					TrailerCard card = Instantiate(this.cardObject, spawnPos, Quaternion.Euler(0f, 0f, 180f))
						.GetComponent<TrailerCard>();
					Debug.Log("[DEBUG] Card: " + roundCards[index].name);
					card.Init(roundCards[index]);
					index++;
					StartCoroutine(AnimateTrailerCard(card, z));
					yield return new WaitForSeconds(0.1f);
				}
			}
		}

		private static IEnumerator AnimateTrailerCard(TrailerCard card, float z) {
			var t = card.transform;

			var startPos = t.position; // 이미 y+1 상태
			var targetPos = new Vector3(startPos.x, Y_POSITION, z);

			var duration = 0.5f;
			var time = 0f;

			while (time < duration) {
				time += Time.deltaTime;
				var t01 = time / duration;

				// 부드럽게 (ease out 느낌)
				var ease = 1f - Mathf.Pow(1f - t01, 3f);

				t.position = Vector3.Lerp(startPos, targetPos, ease);
				yield return null;
			}

			t.position = targetPos;
		}

		public void InstantiateSubRoundCard(int cardId) {
			var randomPosition = GenerateRandomXZPosition();
			var subRoundCard = cardRepository.GetCardByID(cardId);
			// Generate Random Position
			Instantiate(cardObject, randomPosition, Quaternion.identity);
		}

		private static Vector3 GenerateRandomXZPosition() {
			var x = Random.Range(X_MIN_BOUNDARY, X_MAX_BOUNDARY);
			var z = Random.Range(Z_MIN_BOUNDARY, Z_MAX_BOUNDARY);

			return new Vector3(x, Y_POSITION, z);
		}
	}
}
