using System.Collections.Generic;
using UnityEngine;

namespace GeneralScripts.Gameplay {
	public class SlotMachine : MonoBehaviour {
		[SerializeField] private Transform[] slots;

		private void OnEnable() {
			GameEvents.OnAllLeversDown += EvaluateSlots;
		}

		private void OnDisable() {
			GameEvents.OnAllLeversDown -= EvaluateSlots;
		}

		// Function 1: Get slot value
		// x-rotation 80 ~ 260 : Blue (1)
		// else : Red (0)
		private int[] GetSlotValues() {
			var values = new int[slots.Length];

			for (var i = 0; i < slots.Length; ++i) {
				var xRotation = slots[i].eulerAngles.x;

				if (xRotation is >= 80f and <= 260f)
					values[i] = 1; // Blue
				else
					values[i] = 0; // Red
			}

			return values;
		}

		// Function 2: Evaluate slots and trigger event
		private void EvaluateSlots() {
			var values = GetSlotValues();

			GameEvents.OnSlotMachineFinished?.Invoke();
		}
	}
}
