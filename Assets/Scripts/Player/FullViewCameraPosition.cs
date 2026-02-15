using GenericHelpers;
using UnityEngine;

namespace Player {
	public class FullViewCameraPosition : MonoBehaviour {
		private void Awake() {
			GameParameters.FullViewCameraPoint = transform;
		}

		private void OnDestroy() {
			if (GameParameters.FullViewCameraPoint == transform)
				GameParameters.FullViewCameraPoint = null;
		}
	}
}
