using System;
using System.Collections;
using UnityEngine;

namespace Trailer.EnumCameraScripts {
	public class IntroCameraScript : CameraScript {
		[Header("Other Objects")]
		[SerializeField] private TrailerObjectInstantiator trailerObjectInstantiator;

		private readonly int[] roundCards = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

		private void Start() {
			UserInterface.UserInterface.Instance.Init(true);
			StartCoroutine(PlayCameraSequence());
		}

		private IEnumerator PlayCameraSequence() {
			foreach (IntroCameraPosition pos in Enum.GetValues(typeof(IntroCameraPosition))) {
				Debug.Log("[DEBUG] Position: " + pos);
				var target = EnumPositionStorage<IntroCameraPosition>.Positions[(int)pos];
				if (!target) continue;
				switch (pos) {
					case IntroCameraPosition.VsScreenPopUp:
						yield return StartCoroutine(MoveCamera(target, moveDuration));
						UserInterface.UserInterface.Instance.DisplayVS();
						// Can this be done during the Move?
						yield return new WaitForSeconds(2f);
						UserInterface.UserInterface.Instance.DisableDisplayVS();
						break;
					case IntroCameraPosition.HealthBarPopUp:
						UserInterface.UserInterface.Instance.DisplayHealth();
						// yield return StartCoroutine(MoveCamera(target, moveDuration));
						yield return new WaitForSeconds(1.5f);
						break;
					case IntroCameraPosition.CardsPopUp:
						yield return StartCoroutine(MoveCamera(target, 0.5f));
						trailerObjectInstantiator.InstantiateRoundCardsByID(roundCards);
						yield return new WaitForSeconds(5f);
						break;
					case IntroCameraPosition.Memorize:
						yield return new WaitForSeconds(stayDuration);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			yield return new WaitForSeconds(stayDuration);
		}
	}
}
