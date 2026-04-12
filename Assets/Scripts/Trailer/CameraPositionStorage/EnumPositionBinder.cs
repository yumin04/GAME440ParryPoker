using UnityEngine;

namespace Trailer.CameraPositionStorage {
	public class EnumPositionBinder<T> : MonoBehaviour where T : System.Enum {
		[SerializeField] private T position;

		private void Awake() {
			var index = System.Convert.ToInt32(position);
			EnumPositionStorage<T>.Positions[index] = transform;
		}
	}
}
