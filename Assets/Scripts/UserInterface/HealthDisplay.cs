using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UserInterface {
	public class HealthDisplay : MonoBehaviour {
		[FormerlySerializedAs("Player1HealthText")]
		[SerializeField] protected TextMeshProUGUI player1HealthText;
		[FormerlySerializedAs("Player2HealthText")]
		[SerializeField] protected TextMeshProUGUI player2HealthText;

		[FormerlySerializedAs("Player1HealthBar")]
		[SerializeField] protected GameObject player1HealthBar;
		[FormerlySerializedAs("Player2HealthBar")]
		[SerializeField] protected GameObject player2HealthBar;

		[SerializeField] protected Sprite redHealthBar;
		[SerializeField] protected Sprite greenHealthBar;
		protected Image player1HealthBarImage;
		protected Image player2HealthBarImage;

		public virtual void Init(bool isPlayer1) {
			player1HealthBarImage = player1HealthBar.GetComponent<Image>();
			player2HealthBarImage = player2HealthBar.GetComponent<Image>();
			gameObject.SetActive(true);
			if (isPlayer1) {
				player1HealthText.color = Color.black;
				player1HealthBarImage.sprite = greenHealthBar;
				player2HealthText.color = new Color32(0xFF, 0x31, 0x31, 255);
				player2HealthBarImage.sprite = redHealthBar;
			}
			else {
				player1HealthText.color = new Color32(0xFF, 0x31, 0x31, 255);
				player1HealthBarImage.sprite = redHealthBar;
				player2HealthText.color = Color.black;
				player2HealthBarImage.sprite = greenHealthBar;
			}
		}

		public void HideHealthDisplay() {
			gameObject.SetActive(false);
		}

		public void SetPlayer1Health(int player1Health) {
			player1HealthText.text = player1Health.ToString() + " / 100 HP";
			player1HealthBar.transform.localScale = new Vector3(player1Health / 100f, 1, 1);
		}

		public void SetPlayer2Health(int player2Health) {
			player2HealthText.text = player2Health.ToString() + " / 100 HP";
			player2HealthBar.transform.localScale = new Vector3(player2Health / 100f, 1, 1);
		}
	}
}
