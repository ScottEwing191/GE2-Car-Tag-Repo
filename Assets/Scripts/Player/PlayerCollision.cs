using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.PlayerSpace {
    public class PlayerCollision : MonoBehaviour {
        [SerializeField] private LayerMask collisionBackOnCheckMask;

        [Tooltip("The time between the chaser colliding with the runner and the collision being turned off. (Same for all cars)")]
        [SerializeField] private float collisionDisableTime = 0.5f;

        [Tooltip("List of GameObjects who should not collide after the Runner is caught")]
        [SerializeField] private List<GameObject> colliderObjects = new List<GameObject>();


        [Tooltip("Used to difine the collision check box for when collision between cars get turned back on after Chaser catches Runner")]
        [SerializeField] private BoxCollider tempBoxCollider;
        private Vector3 collisionCheckCenter;       // when chaser catches the runner, collision is briefly turned off. These define the box that is used to check ...
        private Vector3 collisionCheckSize;         // ... if collision can be turned back on
        private Player player;
        private static bool rolesSwapped = false;   // there are 3 colliders on the car. When any 1 of them sucessfully collides with the runner then this variable...
                                                    // ... will be set to false and will stop the other two colliders from initiating a second Role Swap. Since this...
                                                    // ... variable is static it will also stop other chase cars from initiating a role swap after the initial Swap. 

        private void Awake() {
            player = GetComponentInParent<Player>();
            collisionCheckCenter = tempBoxCollider.center;
            collisionCheckSize = tempBoxCollider.size;
        }

        private void Start() {
            Destroy(tempBoxCollider);

        }

        private void OnCollisionEnter(Collision collision) {
            if (rolesSwapped) { return; }       // if the roles have already swapped (due to first collider on car) dont swap again (due to second)

            if (player.PlayerRoll != PlayerRoleEnum.Chaser) { return; }       // this car must be a chaser to continue

            if (!collision.transform.CompareTag("Player")) { return; }        // other object must be a car to continue

            if (!IsCollidingWithRunner(collision)) { return; }              // Must be colliding with the runner and not another chaser to continue

            if (GameManager.Instance.CheckpointManager.CheckpointQueues[player.PlayerListIndex].Count != 0) { return; }  // Has this car been through all checkpoints

            //--Chaser Has Sucesfully caught the runner
            //--Turn of the collision on the chaser (soon to be runner)
            rolesSwapped = true;
            StartCoroutine(TurnOffCollisionWithDelay(collisionDisableTime));

            //--Tell the GameManager To manage the Role Swap Behaviour 
            GameManager.Instance.ManageRoleSwap(player, player.PlayerManager.GetPlayerFromGameObject(collision.gameObject));
        }
        /// <summary>
        /// Check if the car being collided with is a Runner or not
        /// </summary>
        private bool IsCollidingWithRunner(Collision collision) {
            foreach (var p in player.PlayerManager.Players) {
                if (p.gameObject == collision.gameObject.transform.parent.gameObject) {         // if this Player is the one we are colliding with
                    if (p.PlayerRoll == PlayerRoleEnum.Runner) {                                // .. and this player is the runner
                        return true;
                    }
                }
            }
            return false;
        }


        private void SetGameObjectListToLayer(List<GameObject> list, string layerName) {
            foreach (var l in list) {
                l.layer = LayerMask.NameToLayer(layerName);
            }
        }
        IEnumerator TurnOffCollisionWithDelay(float delay) {
            if (delay!= 0) {
                yield return new WaitForSeconds(delay);
            }
            SetGameObjectListToLayer(colliderObjects, "Car No Collision");
        }

        private bool CanCollisionBeTurnedOn() {
            if (Physics.CheckBox(transform.position + collisionCheckCenter, collisionCheckSize / 2, transform.rotation, collisionBackOnCheckMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }
            return true;
        }


        // When the chaser collides with the player I wan the original collision to happen.
        // I then want the cars to phase through each other ie not collide.
        // Then when they are not inside of each other the collision can turn back on
        public IEnumerator TurnCarCollisionBackOn(float delay) {
            if (delay != 0) {
                yield return new WaitForSeconds(delay);
            }
            while (!CanCollisionBeTurnedOn()) {
                yield return new WaitForFixedUpdate();
            }
            SetGameObjectListToLayer(colliderObjects, "Car Collision");
            rolesSwapped = false;
        }
    }
}
