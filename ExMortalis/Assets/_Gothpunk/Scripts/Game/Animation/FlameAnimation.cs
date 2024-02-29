using UnityEngine;

[ExecuteInEditMode]
public class FlameAnimation : MonoBehaviour
{
    public float minIntensity = 1f;     // Minimum intensity of the flame
    public float maxIntensity = 2f;     // Maximum intensity of the flame
    public float speed = 1f;            // Speed of flickering
    public float noiseStrength = 1f;    // Strength of the Perlin noise

    private Light flameLight;

    private void Start()
    {
        flameLight = GetComponent<Light>();
    }

    private void Update()
    {
        // Calculate the intensity based on Perlin noise and time
        float noise = Mathf.PerlinNoise(Time.time * speed, 0f) * 2f - 1f; // Generate Perlin noise between -1 and 1
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, noiseStrength * noise);

        // Apply intensity to the light
        flameLight.intensity = intensity;
    }
}
