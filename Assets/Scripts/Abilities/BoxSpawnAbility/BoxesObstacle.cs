using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities.BoxSpawn
{
    public class BoxesObstacle : MonoBehaviour
    {
        [SerializeField] Material collisionMaterial;
        [SerializeField] Material noCollisionMaterial;
        [SerializeField] private Color spawningCollisionColor;              // the colour the boxes will appear while they are UNABLE to be placed
        [SerializeField] private Color spawningNoCollisionColor;            // the colour the boxes will appear while they are ABLE to be placed

        Rigidbody[] boxesArray;                                             // the rigidbody components attached to each of the boxes
        Renderer[] boxRendererArray;                                        // the renderer components atached to each of the boxes

        private void Awake() {
            boxesArray = GetComponentsInChildren<Rigidbody>();
            boxRendererArray = GetComponentsInChildren<Renderer>();
        }

        //--Turn off the physics and collision for the boxes 
        public void DisablePhysics() {
            foreach (var b in boxesArray) {
                b.isKinematic = true;
                b.gameObject.layer = LayerMask.NameToLayer("No Collision");
            }
        }
        //--Turn on the physics and collision for the boxes 
        public void EnablePhysics() {
            foreach (var b in boxesArray) {
                b.isKinematic = false;
                b.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        //--Switch the material of each of the boxes to the one used for boxes which CAN be collided with
        public void SetMaterialCollision() {
            for (int i = 0; i < boxRendererArray.Length; i++) {
                boxRendererArray[i].material = collisionMaterial;
            }
        }
        //--Switch the material of each of the boxes to the one used for boxes which CANNOT be collided with
        public void SetMaterialNoCollision() {
            for (int i = 0; i < boxRendererArray.Length; i++) {
                boxRendererArray[i].material = noCollisionMaterial;
            }
        }

        //--Sets the colour of the boxes based on whether they are able to have their collision turned on (i.e if collision was turned on now would they
        //--collide with anything)
        public void SetColour(bool wouldCollide) {
            if (wouldCollide) {
                noCollisionMaterial.color = spawningCollisionColor;     // would collide = turn red
            }
            else {
                noCollisionMaterial.color = spawningNoCollisionColor;   // would collide = turn green
            }
        }
    }
}
