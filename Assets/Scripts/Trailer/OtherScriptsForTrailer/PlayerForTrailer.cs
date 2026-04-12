using UnityEngine;

namespace Trailer.OtherScriptsForTrailer {
	public class PlayerForTrailer : MonoBehaviour {
		private Animator anim;

		private void Start() {
			anim = GetComponent<Animator>();
			// anim.SetTrigger("Hit");
		}
	}
}
