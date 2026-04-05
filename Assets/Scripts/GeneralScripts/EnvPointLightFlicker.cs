using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies the same intensity wobble as <see cref="LightFlicker"/> to every
/// point light under this object (for imported ENV geometry where lights live in the FBX).
/// </summary>
public class EnvPointLightFlicker : MonoBehaviour
{
    readonly List<Light> _lights = new();
    readonly List<float> _baseIntensities = new();
    readonly List<float> _phases = new();

    void Awake()
    {
        foreach (var light in GetComponentsInChildren<Light>(true))
        {
            if (light.type != LightType.Point)
                continue;
            _lights.Add(light);
            _baseIntensities.Add(light.intensity);
            _phases.Add(Random.Range(0f, 6.28f));
        }
    }

    void Update()
    {
        for (var i = 0; i < _lights.Count; i++)
        {
            var light = _lights[i];
            if (light == null)
                continue;
            float t = Time.time + _phases[i];
            float wobble = Mathf.Sin(t * 5.7f) * 0.06f + Mathf.Sin(t * 11.3f) * 0.042f;
            float noise = (Mathf.PerlinNoise(t * 0.8f, _phases[i]) - 0.5f) * 0.2f;
            float slowDip = Mathf.Sin(t * 2.1f + _phases[i] * 1.3f) * 0.16f;
            float m = 1f + wobble + noise + slowDip;
            light.intensity = _baseIntensities[i] * Mathf.Clamp(m, 0.52f, 1.12f);
        }
    }
}
