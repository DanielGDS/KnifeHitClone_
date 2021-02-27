using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour
{
    public float radius;
    public float force;

    private void Start()
    {
        Explode();
    }
    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] overlappedColliders = Physics.OverlapSphere(explosionPos, radius);

        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force, explosionPos, radius, 3.0f, ForceMode.Impulse);
                Explosions explosions = rigidbody.GetComponent<Explosions>();
                if (explosions)
                {
                    if (Vector3.Distance(transform.position, rigidbody.position) < radius / 2)
                        explosions.Explode();
                }
            }

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius / 2f);
    }
}
