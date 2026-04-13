using System.Collections.Generic;
using UnityEngine;

namespace UserInterface {
	public class SubRoundIndicator : MonoBehaviour {
		[SerializeField] private List<GameObject> subRounds;

		public void DisableSubRound(int subroundNumber) {
			for (var i = 10; i > subroundNumber; i--) {
				subRounds[i - 1].SetActive(false);
				Debug.Log("[DEBUG] IS THIS RUN?");
			}
		}

		public void EnableAllSubRound() {
			gameObject.SetActive(true);
			foreach (var subRound in subRounds) {
				subRound.SetActive(true);
			}
		}

		public void DisableSubRoundNumber() {
			gameObject.SetActive(false);
		}
	}
}
