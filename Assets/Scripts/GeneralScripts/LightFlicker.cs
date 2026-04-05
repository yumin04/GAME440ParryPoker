using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    Light _light;
    float _baseIntensity;
    float _phase;

    void Awake()
    {
        _light = GetComponent<Light>();
        _baseIntensity = _light.intensity;
        _phase = Random.Range(0f, 6.28f);
    }

    void Update()
    {
        float t = Time.time + _phase;
        float wobble = Mathf.Sin(t * 5.7f) * 0.06f + Mathf.Sin(t * 11.3f) * 0.042f;
        float noise = (Mathf.PerlinNoise(t * 0.8f, _phase) - 0.5f) * 0.2f;
        float slowDip = Mathf.Sin(t * 2.1f + _phase * 1.3f) * 0.16f;
        float m = 1f + wobble + noise + slowDip;
        _light.intensity = _baseIntensity * Mathf.Clamp(m, 0.52f, 1.12f);
    }
}
