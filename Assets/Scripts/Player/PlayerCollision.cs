using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Player {
    public class PlayerCollision : MonoBehaviour {
        [Tooltip("List of GameObjects who should not collide after the Runner is caught")]
        [SerializeField] private List<GameObject> colliderObjects = new List<GameObject>();

        [SerializeField] private LayerMask collisionBackOnCheckMask;

        [Tooltip("Used to difine the collision check box for when collision between cars get turned back on after Chaser catches Runner")]
        [SerializeField] BoxCollider tempBoxCollider;
        private Vector3 collisionCheckCenter;       // when chaser catches the runner, collision is briefly turned off. These define the box that is used to check ...
        private Vector3 collisionCheckSize;         // ... if collision can be turned back on
        private Player player;
        private static bool rolesSwapped = false;    // Since there are three colliders on the car it would be possible for the role swap to occur multiple...
                                                            // ... times, which we dont want. This bool is used to make sure that the roles are swapped only once...
                                                            // ... for each collision between cars

        private void Awake() {
            player = GetComponentInParent<Player>();
            //collisionCheckCenter = tempBoxCollider.center;
            //collisionCheckSize = tempBoxCollider.size;
        }

        private void Start() {
            Destroy(tempBoxCollider);
            
        }
               
        private void FixedUpdate() {    
        }
        private void OnCollisionEnter(Collision collision) {
            //--if the roles have already swapped (due to first collider on car) dont swap again (due to second)
            if (rolesSwapped) { return; }
                        
            if (player.PlayerRoll != PlayerRoleEnum.Chaser) {
                print(transform.parent.name + " : This car is not a Chaser");
                return;
            }    //this car must be a chaser to continue

            if (!collision.transform.CompareTag("Player")) {
                //print(transform.parent.name + " : Other Object is not a Player");
                return;
            }      // other object must be a car to continue

            if (!IsCollidingWithRunner(collision)) {
                //print(transform.parent.name + " : Colliding with other Chaser");
                return;
            }              // Must be colliding with the runner and not another chaser to continue

            if (GameManager.Instance.CheckpointManager.CheckpointQueues[player.PlayerListIndex].Count != 0) {
                //print(transform.parent.name+ " : Checkpoint Missed");
                return;
            }  // Has this car been through the required checkpoints


            //Initiate roll Swap
            print(transform.parent.name + "Swap Rolls");
            print("This: " + transform.parent.name + " Collision: " + collision.gameObject.name);
            //StartCoroutine(DoRollSwapCollisionBehaviour(gameObject, collision.gameObject));

            rolesSwapped = true;
            // Tell the player manager to swap the roles of this player and the player which was collided with 
            player.playerManager.SwapRoles(player, player.playerManager.GetPlayerFromGameObject(collision.gameObject));
        }

        private bool IsCollidingWithRunner(Collision collision) {
            foreach (var p in player.playerManager.Players) {
                if (p.gameObject == collision.gameObject.transform.parent.gameObject) {         // if this Player is the one we are colliding with
                    if (p.PlayerRoll == PlayerRoleEnum.Runner) {                                // .. and this player is the runner
                        //print("Correct Car");
                        return true;
                    }
                }
            }
            return false;
        }

        // When the chaser collides with the player I wan the original collision to happen.
        // I then want the cars to phase through each other ie not collide.
        // Then when they are not inside of each other the collision can turn back on
        private IEnumerator DoRollSwapCollisionBehaviour(GameObject chaserCar, GameObject runnerCar) {

            yield return new WaitForSeconds(1);
            SetGameObjectListToLayer(colliderObjects, "Car No Collision");
            yield return new WaitForSeconds(3);
            while (!CanCollisionBeTurnedOn()) {
                yield return new WaitForFixedUpdate();
            }
            SetGameObjectListToLayer(colliderObjects, "Car Collision");
        }

        private void SetGameObjectListToLayer(List<GameObject> list, string layerName) {
            foreach (var l in list) {
                l.layer = LayerMask.NameToLayer(layerName);
            }
        }

        private bool CanCollisionBeTurnedOn() {
            if (Physics.CheckBox(transform.position+collisionCheckCenter, collisionCheckSize / 2, Quaternion.identity, collisionBackOnCheckMask)) {
                return false;
            }
            
            return true;
        }

        private void OnDrawGizmos() {
            Gizmos.DrawCube(transform.position + collisionCheckCenter, collisionCheckSize / 2);
        }
    }
}
