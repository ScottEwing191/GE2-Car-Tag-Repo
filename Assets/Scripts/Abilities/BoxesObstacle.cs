using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities.BoxSpawn
{
    public class BoxesObstacle : MonoBehaviour
    {
        Rigidbody[] boxesArray;

        private void Awake() {
            boxesArray = GetComponentsInChildren<Rigidbody>();
        }

        public void DisablePhysics() {
            foreach (var b in boxesArray) {
                b.isKinematic = true;
                b.gameObject.layer = LayerMask.NameToLayer("No Collision");
            }
        }

        public void EnablePhysics() {
            foreach (var b in boxesArray) {
                b.isKinematic = false;
                b.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
