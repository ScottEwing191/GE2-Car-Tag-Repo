using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private float explosionForce = 100;
        [SerializeField] private float explosionRadius = 5;
        
        private void OnTriggerEnter(Collider other) {
                print("Rocket Trigger Hit");

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            List<Rigidbody> rigidBodies = new List<Rigidbody>();

            foreach (var c in colliders) {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                if (rb != null) {
                    rigidBodies.Add(rb);
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }



            Rigidbody body = other.GetComponentInParent<Rigidbody>();
            body.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        
    }
}
