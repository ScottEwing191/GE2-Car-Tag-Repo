using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class Rocket : MonoBehaviour {
        [SerializeField] private float explosionForce = 80000;
        [SerializeField] private float explosionRadius = 3;
        [SerializeField] private float fuseTime = 3;
        [SerializeField] private bool useFuse = false;
        [SerializeField] float rocketSpeed = 5;
        [SerializeField] private ParticleSystem rocketEffect;
        [SerializeField] private ParticleSystem explosionEffect;

        Rigidbody thisRigidbody;
        private float timeLeftTillExplode;
        private bool hasExploded = false;

        private void Start() {
            thisRigidbody = GetComponent<Rigidbody>();
            timeLeftTillExplode = fuseTime;
            StartRocket(rocketSpeed, transform.rotation);

        }
        public void StartRocket(float speed, Quaternion spawnRotation) {
            rocketEffect.Play();
            transform.rotation = spawnRotation;
            thisRigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
        private void Update() {
            if (useFuse) {
                timeLeftTillExplode -= Time.deltaTime;
                if (timeLeftTillExplode <= 0) {
                    Explode();
                }
            }
        }
        private void OnTriggerEnter(Collider other) {
            if (!hasExploded) {
                print("Rocket Trigger Hit:" + other.gameObject.name);
                Explode();
            }
        }

        private void Explode() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var c in colliders) {
                if (c.CompareTag("CollidesWithRocket")) {
                    Rigidbody rb = c.GetComponent<Rigidbody>();
                    if (rb == null) {
                        rb = c.GetComponentInParent<Rigidbody>();
                    }
                    if (rb != null) {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    else {
                        Debug.Log("Rocket could not find a Rigidbody to explode on an object tagged with 'Collides With Rocket' ");
                    }
                }
            }
            hasExploded = true;
            thisRigidbody.AddForce(-thisRigidbody.velocity, ForceMode.VelocityChange);              // Dont know why this needs be divided by two
            rocketEffect.Stop();
            explosionEffect.Play();
            Destroy(gameObject, explosionEffect.main.duration);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
