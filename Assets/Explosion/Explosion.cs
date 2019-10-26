using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float radius = 2f;

    [SerializeField]
    float force = 5f;

    [SerializeField]
    LayerMask mask = Physics.DefaultRaycastLayers;

    IEnumerator Start()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, mask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].attachedRigidbody == null) continue;

            colliders[i].attachedRigidbody.AddExplosionForce(force, transform.position, radius, 0f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
