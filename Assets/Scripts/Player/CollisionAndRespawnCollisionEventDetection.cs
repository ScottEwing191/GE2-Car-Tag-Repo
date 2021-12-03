using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag {
    public class CollisionAndRespawnCollisionEventDetection : MonoBehaviour {
        private Player thisPlayer;

        private void Awake() {
            thisPlayer = GetComponentInParent<Player>();
        }

        private void OnTriggerEnter(Collider other) {
            thisPlayer.PlayerRespawn.TriggerEnter(other);
        }

        private void OnCollisionEnter(Collision collision) {
            thisPlayer.PlayerCollision.CollisionEnter(collision);
        }
    }
}
