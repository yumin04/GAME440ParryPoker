using System.Collections.Generic;
using GeneralScripts.Card;
using UnityEngine;

namespace Player {
	public class PlayerHand : MonoBehaviour {
		[SerializeField] private GameObject playerDisplayCardPrefab;

		[SerializeField] private float radius = 5f;
		[SerializeField] private float maxAngle = 60f;

		private GameObject[] cards;

		private void AddCards(int[] cardIds) {
			cards = new GameObject[cardIds.Length];

			for (var i = 0; i < cardIds.Length; i++) {
				var card = Instantiate(playerDisplayCardPrefab, transform);

				card.GetComponent<PlayerCard>().Init(cardIds[i]);

				cards[i] = card;
			}

			Rearrange();
		}

		public void DisplayCards(int[] cardIds) {
			Clear();

			AddCards(cardIds);
		}

		private void Rearrange() {
			var count = cards.Length;
			if (count == 0) return;

			var angleStep = count == 1 ? 0 : maxAngle / (count - 1);
			var startAngle = -maxAngle / 2f;
			float y = 0;
			for (var i = 0; i < count; i++) {
				var angle = startAngle + angleStep * i;
				var rad = angle * Mathf.Deg2Rad;

				var x = Mathf.Sin(rad) * radius;
				var z = -Mathf.Cos(rad) * radius + radius;

				cards[i].transform.localPosition = new Vector3(x, y, z);

				cards[i].transform.localRotation = Quaternion.Euler(0, -angle, 0);
				y += 0.001f;
			}
		}

		public void Clear() {
			if (cards != null)
				foreach (var card in cards) Destroy(card);

			cards = null;
		}
	}
}
