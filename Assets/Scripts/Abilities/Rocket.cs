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

        Rigidbody thisRigidbody;
        private float timeLeftTillExplode;

        private void Start() {
            thisRigidbody = GetComponent<Rigidbody>();
            timeLeftTillExplode = fuseTime;
            Move(rocketSpeed, transform.rotation);

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
            print("Rocket Trigger Hit");
            Explode();

        }

        public void Move(float speed, Quaternion spawnRotation) {
            transform.rotation = spawnRotation;
            this.thisRigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }

        private void Explode() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            //List<Rigidbody> rigidBodies = new List<Rigidbody>();

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
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected() {
                Gizmos.DrawWireSphere(transform.position, explosionRadius);
            }
        }
    }
