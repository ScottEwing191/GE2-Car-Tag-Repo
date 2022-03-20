using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class Rocket : MonoBehaviour {
        [SerializeField] private float explosionForce = 50000;
        [SerializeField] private float explosionRadius = 2.5f;
        [SerializeField] private float fuseTime = 3;
        [SerializeField] private bool useFuse = false;
        [SerializeField] private bool addRocketSpeedToCarSpeed = false;
        [SerializeField] private float rocketSpeed = 5;
        [SerializeField] private ParticleSystem rocketEffect;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private LayerMask collidableLayers;
        [SerializeField] private Light rocketLight;

        Rigidbody thisRigidbody;
        private float timeLeftTillExplode;
        private bool hasExploded = false;
        private Player spawner;                 // The player which spawned the rocket

        public Player PlayerWhoFired { get; set; }      // Just using this for telemetry

        public void StartRocket(float carSpeed) {
            thisRigidbody = GetComponent<Rigidbody>();
            timeLeftTillExplode = fuseTime;
            float finalSpeed = rocketSpeed;
            if (addRocketSpeedToCarSpeed) {
                finalSpeed += carSpeed;
            }
            rocketEffect.Play();
            thisRigidbody.AddForce(transform.forward * finalSpeed, ForceMode.VelocityChange);
            PlayerWhoFired.PlayerScore.RocketsFired++;      //For Telemetry
        }


        private void Update() {
            if (useFuse) {
                timeLeftTillExplode -= Time.deltaTime;
                if (!hasExploded && timeLeftTillExplode <= 0) {
                    Explode(transform.position);
                }
            }
        }

        internal void SetSpawner(Player thisPlayer) {
            spawner = thisPlayer;
        }

        private void OnCollisionEnter(Collision collision) {
            //--                    Is colliding with an object which is already on the collidable layer
            if (!hasExploded && collidableLayers == (collidableLayers | 1 << collision.gameObject.layer)) {
                if (!IsCollidingWithSpawner(collision.gameObject)) {                                        // dont explode when hitting car that fired rocket
                    Vector3 contactPosition = collision.GetContact(0).point;
                    Explode(contactPosition);
                    CheckSuccessfullHit(collision);
                }
            }
        }

        private void CheckSuccessfullHit(Collision collision) {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("Box")) {
                print("Succesful Hit");
                PlayerWhoFired.PlayerScore.RocketHits++;            //For Telemetry
            }
        }

        //--Check if the object that the rocket collided with has a Player script attached to its parents. And checks if the that player is the same as the player who 
        //--fired the rocket.
        private bool IsCollidingWithSpawner(GameObject collidedWith) {
            Player playerCollidedWith = GameManager.Instance.PlayerManager.GetPlayerFromGameObject(collidedWith);
            if (playerCollidedWith != null) {
                if (playerCollidedWith == spawner) {
                    return true;
                }
            }
            return false;
        }

        private void Explode(Vector3 explodePosition) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, collidableLayers);
            foreach (var c in colliders) {
                if (c.CompareTag("CollidesWithRocket")) {
                    
                    Rigidbody rb = c.GetComponent<Rigidbody>();
                    if (rb == null) {
                        rb = c.GetComponentInParent<Rigidbody>();
                    }
                    if (rb != null && !IsCollidingWithSpawner(c.transform.gameObject)) {
                        rb.AddExplosionForce(explosionForce, explodePosition, explosionRadius);
                    }
                    else {
                        //Debug.Log("Rocket could not find a Rigidbody to explode on an object tagged with 'Collides With Rocket' ");
                    }
                }
            }
            hasExploded = true;
            thisRigidbody.AddForce(-thisRigidbody.velocity, ForceMode.VelocityChange);      // stop rocket    
            rocketEffect.Stop();
            rocketEffect.gameObject.SetActive(false);                                       
            explosionEffect.Play();
            rocketLight.enabled = false;
            Destroy(gameObject, explosionEffect.main.duration);                             // destroy the Gameobject after the explosion effect has played
        }
    }
}
