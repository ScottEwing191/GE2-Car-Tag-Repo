using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.PlayerSpace {
    public class PlayerCollision : MonoBehaviour {
        [Tooltip("Make sure the car is not going to collide with this layer before collision is turned on")]
        private LayerMask collisionOnCheckMask;

        [Tooltip("The time between the chaser colliding with the runner and the collision being turned off. (Same for all cars)")]
        [SerializeField] private float collisionDisableTime = 0.5f;

        [Tooltip("List of GameObjects who should not collide after the Runner is caught")]
        [SerializeField] private List<GameObject> colliderObjects = new List<GameObject>();


        [Tooltip("Used to difine the collision check box for when collision between cars get turned back on after Chaser catches Runner")]
        [SerializeField] private BoxCollider tempBoxCollider;
        private Vector3 collisionCheckCenter;       // when chaser catches the runner, collision is briefly turned off. These define the box that is used to check ...
        private Vector3 collisionCheckHalfSize;         // ... if collision can be turned back on
        private Player thisPlayer;
        private static bool rolesSwapped = false;   // there are 3 colliders on the car. When any 1 of them sucessfully collides with the runner then this variable...
                                                    // ... will be set to false and will stop the other two colliders from initiating a second Role Swap. Since this...
                                                    // ... variable is static it will also stop other chase cars from initiating a role swap after the initial Swap. 

        private void Awake() {
            thisPlayer = GetComponentInParent<Player>();
            collisionCheckCenter = tempBoxCollider.center;
            collisionCheckHalfSize = tempBoxCollider.size / 2;
            //collisionOnCheckMask = LayerMask.GetMask("Car Collision");
            collisionOnCheckMask = LayerMask.GetMask("RCC");

        }

        private void Start() {
            Destroy(tempBoxCollider);
        }

        

        public void CollisionEnter(Collision collision) {
            if (rolesSwapped) { return; }       // if the roles have already swapped (due to first collider on car) dont swap again (due to second)

            if (!thisPlayer.IsPlayerEnabled) { return; }                            //TEST

            if (thisPlayer.PlayerRoll != PlayerRoleEnum.Chaser) { return; }         // this car must be a chaser to continue (This is the car which is turning into the runner)

            if (!collision.transform.CompareTag("Player")) { return; }        // other object must be a car to continue

            if (!IsCollidingWithRunner(collision)) { return; }              // Must be colliding with the runner and not another chaser to continue

            if (GameManager.Instance.CheckpointManager.CheckpointQueues[thisPlayer.PlayerListIndex].Count != 0) { return; }  // Has this car been through all checkpoints

            //--Chaser Has Sucesfully caught the runner
            //--Turn of the collision on the chaser (soon to be runner)
            rolesSwapped = true;
            StartCoroutine(TurnOffCollisionWithDelay(collisionDisableTime));
            //--Tell the GameManager To manage the Role Swap Behaviour 
            GameManager.Instance.ManageRoleSwap(thisPlayer, thisPlayer.PlayerManager.GetPlayerFromGameObject(collision.gameObject));
        }
        /// <summary>
        /// Check if the car being collided with is a Runner or not
        /// </summary>
        private bool IsCollidingWithRunner(Collision collision) {
            foreach (var p in thisPlayer.PlayerManager.Players) {
                if (p.gameObject == collision.gameObject.transform.parent.gameObject) {         // if this Player is the one we are colliding with
                    if (p.PlayerRoll == PlayerRoleEnum.Runner) {                                // .. and this player is the runner
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Set the car colliders to the given layer
        /// </summary>
        /// <param name="layerName"></param>
        public void SetGameObjectListToLayer(string layerName) {
            foreach (var c in colliderObjects) {
                c.layer = LayerMask.NameToLayer(layerName);
            }
        }

        public IEnumerator TurnOffCollisionWithDelay(float delay) {
            if (delay != 0) {
                yield return new WaitForSeconds(delay);
            }
            SetGameObjectListToLayer("Car No Collision");
        }


        // When the chaser collides with the player I wan the original collision to happen.
        // I then want the cars to phase through each other ie not collide.
        // Then when they are not inside of each other the collision can turn back on
        /// <summary>
        /// Waits until the car will not be colliding with another car if the collision were to be turned on. Before turning the collision on.
        /// </summary>
        /// <param name="delay">The seconds before the method will try to turn the collision on</param>
        /// <param name="collisionOnMask">If the car will collide with anything on this layermask the collision will not turn on</param>
        public IEnumerator TurnOnCarCollision(float delay, LayerMask collisionOnMask) {
            if (delay != 0) {
                yield return new WaitForSeconds(delay);
            }
            while (CarCollisionCheck(thisPlayer.RCC_CarController.transform.position, thisPlayer.RCC_CarController.transform.rotation)) {

                yield return new WaitForFixedUpdate();
            }
            //SetGameObjectListToLayer("Car Collision");
            SetGameObjectListToLayer("RCC");
            rolesSwapped = false;       //this should happen when the new nunners collision is turned on NOT just any old car
        }
        public IEnumerator TurnOnCarCollision(float delay) {
            StartCoroutine(TurnOnCarCollision(delay, collisionOnCheckMask));
            yield return null;
        }


        /// <summary>
        /// Two uses:
        ///     1.Checks if the position the car has been moved to already has car on the "Car Collider Layer" so that the game knows to put
        ///         this car on the "Car No Collision Layer"
        ///     2.Called each frame while this car is on the "Car No Collision" to check if this car is still inside the collider of a car on
        ///         on the "Car Collision" layer. When this method return false the game will know that the colliders are no longer overlapping
        ///         and this car can Safely be set the ("Car Collision) layer
        /// </summary>
        public bool CarCollisionCheck() {
            if (Physics.CheckBox(transform.position + collisionCheckCenter, collisionCheckHalfSize, transform.rotation, collisionOnCheckMask, QueryTriggerInteraction.Ignore)) {
                return true;
            }
            return false;
        }
        public bool CarCollisionCheck(Vector3 position, Quaternion rotation) {
            if (Physics.CheckBox(position + collisionCheckCenter, collisionCheckHalfSize, rotation, collisionOnCheckMask, QueryTriggerInteraction.Ignore)) {
                return true;
            }
            return false;
        }

        /*private void OnDrawGizmos() {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position + collisionCheckCenter, collisionCheckHalfSize * 2);
        }*/
    }
}
