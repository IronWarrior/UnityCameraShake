using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
    [SerializeField]
    ShakeableTransform target;

    [SerializeField]
    float delay = 1f;

    /// <summary>
    /// Maximum possible stress that will be inflicted
    /// on the target when it is immediately beside the
    /// explosion.
    /// </summary>
    [SerializeField]
    float maximumStress = 0.6f;

    [SerializeField]
    float range = 45;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);

        GetComponent<ParticleSystem>().Play();

        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Retrieve a value in the 0...1 range, where it is 0 when
        // the target is immediately beside us, and 1 when at (or beyond) maximum range.
        float distance01 = Mathf.Clamp01(distance / range);

        // Square the value to create a parabolic falloff curve,
        // then invert it such that closer distances cause greater stress.
        // https://en.wikipedia.org/wiki/Parabola
        float stress = (1 - Mathf.Pow(distance01, 2)) * maximumStress;

        target.InduceStress(stress);
    }
}
