using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface {
	public class HealthDisplayForTrailer : HealthDisplay {
		public override void Init(bool isPlayer1) {
			base.Init(isPlayer1);
			StartCoroutine(EmphasizePlayer1());
		}

		private IEnumerator EmphasizePlayer1() {
			StartCoroutine(ScalePunch(player1HealthText.transform));
			StartCoroutine(ScalePunch(player1HealthBar.transform));

			StartCoroutine(ColorFlash(player1HealthBarImage, GetEmphasizeColor(player1HealthBarImage)));

			yield return new WaitForSeconds(0.5f);
			EmphasizePlayer2();
		}

		private void EmphasizePlayer2() {
			StartCoroutine(ScalePunch(player2HealthText.transform));
			StartCoroutine(ScalePunch(player2HealthBar.transform));

			StartCoroutine(ColorFlash(player2HealthBarImage, GetEmphasizeColor(player2HealthBarImage)));
		}

		private static IEnumerator ScalePunch(Transform target) {
			var original = Vector3.one;
			var big = original * 1.2f;

			var t = 0f;

			// 커지기
			while (t < 0.3f) {
				t += Time.deltaTime;
				target.localScale = Vector3.Lerp(original, big, t / 0.15f);
				yield return null;
			}

			t = 0f;

			// 돌아오기
			while (t < 0.3f) {
				t += Time.deltaTime;
				target.localScale = Vector3.Lerp(big, original, t / 0.15f);
				yield return null;
			}
		}

		IEnumerator ColorFlash(Image img, Color targetColor) {
			var original = img.color;
			var t = 0f;

			// 강조
			while (t < 0.3f) {
				t += Time.deltaTime;
				img.color = Color.Lerp(original, targetColor * 1.3f, t / 0.15f);
				yield return null;
			}

			t = 0f;

			// 복귀
			while (t < 0.3f) {
				t += Time.deltaTime;
				img.color = Color.Lerp(targetColor * 1.3f, original, t / 0.3f);
				yield return null;
			}
		}

		private static Color GetEmphasizeColor(Image img) {
			return img.color * 1.3f; // 살짝 밝게
		}
	}
}
