using System;
using System.Collections;
using Trailer.OtherScriptsForTrailer;
using UnityEngine;

namespace Trailer.EnumCameraScripts {
	public class P1GrabThenAttackCameraScript : CameraScript {
		[Header("Other Objects")]
		[SerializeField] private TrailerAttackCard trailerAttackCard;
		[SerializeField] private Transform startPos;
		[SerializeField] private Transform endPos;

		private void Start() {
			StartCoroutine(PlayCameraSequence());
		}

		private IEnumerator PlayCameraSequence() {
			foreach (P1GrabThenAttackCameraPosition pos in
			         System.Enum.GetValues(typeof(P1GrabThenAttackCameraPosition))) {
				Debug.Log("[DEBUG] Position: " + pos);

				var target = EnumPositionStorage<P1GrabThenAttackCameraPosition>.Positions[(int)pos];

				if (!target) continue;
				switch (pos) {
					case P1GrabThenAttackCameraPosition.InitializeCard:
						yield return StartCoroutine(MoveCamera(target, moveDuration));
						// Initialize SubRound Card
						yield return new WaitForSeconds(stayDuration);
						break;
					case P1GrabThenAttackCameraPosition.P1Grab:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// No Need to Move Camera
						// Initialize Animation
						break;
					case P1GrabThenAttackCameraPosition.P1CheckCard:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// follow Card movement, then go to Player 1's perspective
						break;

					case P1GrabThenAttackCameraPosition.P1OptionSelection:
						yield return StartCoroutine(MoveCamera(target, 0.3f));
						// Option Pops Up
						// Mouse starts in between the options
						yield return new WaitForSeconds(0.5f);
						break;
					case P1GrabThenAttackCameraPosition.P1ChoosingAttack:
						yield return StartCoroutine(MoveCamera(target, 0.7f));
						// move the mouse toward Attack, and click
						yield return new WaitForSeconds(0.5f);
						blackScreen.SetActive(false);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			yield return new WaitForSeconds(stayDuration);
		}
	}
}
