using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    ShakeableTransform target;

    [SerializeField]
    float delay = 1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);

        GetComponent<ParticleSystem>().Play();
    }
}