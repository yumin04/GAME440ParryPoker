using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UserInterface {
	public class RoundIndicator : MonoBehaviour {
		[FormerlySerializedAs("RoundIndicatorText")]
		[SerializeField] private TextMeshProUGUI roundIndicatorText;

		public void EnableRoundNumber() {
			gameObject.SetActive(true);
		}

		public void DisableRoundNumber() {
			gameObject.SetActive(false);
		}

		public void ChangeRoundText(int roundNumber) {
			roundIndicatorText.text = "ROUND " + roundNumber;
		}
	}
}
