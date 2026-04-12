using UnityEngine;

namespace GeneralScripts {
	[RequireComponent(typeof(Light))]
	public class LightFlicker : MonoBehaviour {
		private Light lightFlicker;
		private float baseIntensity;
		private float phase;

		private void Awake() {
			lightFlicker = GetComponent<Light>();
			baseIntensity = lightFlicker.intensity;
			phase = Random.Range(0f, 6.28f);
		}

		private void Update() {
			var t = Time.time + phase;
			var wobble = Mathf.Sin(t * 5.7f) * 0.05f + Mathf.Sin(t * 11.3f) * 0.035f;
			var noise = (Mathf.PerlinNoise(t * 0.8f, phase) - 0.5f) * 0.12f;
			var m = 1f + wobble + noise;
			lightFlicker.intensity = baseIntensity * Mathf.Clamp(m, 0.82f, 1.12f);
		}
	}
}
