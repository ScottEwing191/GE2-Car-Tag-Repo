using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag {
    public class PlayerCollisionEventDetection : MonoBehaviour {
        private Player thisPlayer;

        private void Awake() {
            thisPlayer = GetComponentInParent<Player>();
        }

        private void OnCollisionEnter(Collision collision) {
            thisPlayer.PlayerCollision.CollisionEnter(collision);
        }
    }
}
