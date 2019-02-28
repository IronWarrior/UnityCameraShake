using UnityEngine;

public class ShakeableTransform : MonoBehaviour 
{
    /// <summary>
    /// Maximum distance in each direction the transform
    /// with translate during shaking.
    /// </summary>
    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one;

    /// <summary>
    /// Maximum angle, in degrees, the transform will rotate
    /// during shaking.
    /// </summary>
    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one * 15;

    /// <summary>
    /// Frequency of the Perlin noise function. Higher values
    /// will result in faster shaking.
    /// </summary>
    [SerializeField]
    float frequency = 25;

    /// <summary>
    /// <see cref="trauma"/> is taken to this power before
    /// shaking is applied. Higher values will result in a smoother
    /// falloff as trauma reduces.
    /// </summary>
    [SerializeField]
    float traumaExponent = 1;

    /// <summary>
    /// Amount of trauma per second that is recovered.
    /// </summary>
    [SerializeField]
    float recoverySpeed = 1;

    /// <summary>
    /// Value between 0 and 1 defining the current amount
    /// of stress this transform is enduring.
    /// </summary>
    private float trauma;

    private float seed;

    private void Awake()
    {
        seed = Random.value;
    }

    private void Update()
    {
        // Taking trauma to an exponent allows the ability to smoothen
        // out the transition from shaking to being static.
        float shake = Mathf.Pow(trauma, traumaExponent);

        // This x value of each Perlin noise sample is fixed,
        // allowing a vertical strip of noise to be sampled by each dimension
        // of the translational and rotational shake.
        // PerlinNoise returns a value in the 0...1 range; this is transformed to
        // be in the -1...1 range to ensure the shake travels in all directions.
        transform.localPosition = new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
        ) * shake;

        transform.localRotation = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        ) * shake);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }

    public void InduceStress(float stress)
    {
        trauma = Mathf.Clamp01(trauma + stress);
    }
}
