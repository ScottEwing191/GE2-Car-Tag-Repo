using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;


namespace CarTag.Abilities.BoxSpawn {
    public class BoxSpawnAbility : SpawnableAbility {
        //--Serialized Fields
        [SerializeField] private int runnerMaxUses = 4;
        [SerializeField] private int chaserMaxUses = 4;

        //--Private
        private List<BoxesObstacle> boxesObstacles = new List<BoxesObstacle>();
        private BoxesObstacle currentBoxObstacle;
        private Coroutine moveBoxesRoutine;
        private bool isCurrentlyRunner;
        private bool inputReleased = false;
        private Transform currentSpawn;


        
        public override void RoleStartSetup(bool isRunner) {
            isCurrentlyRunner = isRunner;
            usesLeft = chaserMaxUses;
            if (isRunner) {
                usesLeft = runnerMaxUses;
            }

            //--Stop Coroutines
            if (moveBoxesRoutine != null) {
                StopCoroutine(moveBoxesRoutine);
                moveBoxesRoutine = null;
            }

            //--Destroy Instantiated Boxes
            foreach (var boxes in boxesObstacles) {
                Destroy(boxes.gameObject);
            }
            boxesObstacles.Clear();
        }

        public override bool CanStartAbility() {
            if (moveBoxesRoutine != null) {           // check if box spawning routine has finished before allowing new box to be spawned
                return false;
            }
            if (usesLeft <= 0) {
                return false;
            }
            return true;
        }

        //-- the player cannot switch to another ability if the box is still in the process of spawning
        public override bool CanSwitchFrom() {
            if (moveBoxesRoutine != null) {
                return false;
            }
            return true;
        }

        //--Method is generic incase different abilites need to pass in different types to this method. This ability doesn't need to pass in anything
        public override void OnAbilityButtonPressed<T>(T obj){
            if (!CanStartAbility()) {
                return;
            }
            inputReleased = false;
            currentSpawn = GetSpawnLocation();
            currentBoxObstacle = InstatiateAndGetScript(currentSpawn);              // instantiate Prefab
            currentBoxObstacle.DisablePhysics();                                    // turn Off Collision
            currentBoxObstacle.SetMaterialNoCollision();                            // Switch to transparent material
            boxesObstacles.Add(currentBoxObstacle);
            moveBoxesRoutine = StartCoroutine(MoveBoxesRoutine(currentSpawn));

        }

        public override void OnAbilityButtonReleased<T>(T obj) {
            if (moveBoxesRoutine == null) {         // Catches error where user presses input key while ability cooldown is active but releases it after the cooldown timer is up
                return;
            }
            inputReleased = true;
        }

        private IEnumerator MoveBoxesRoutine(Transform spawn) {
            //--Keep moving the boxes to the given transform. This is stop when the player releases the Ability input
            while (!inputReleased) {
                yield return new WaitForFixedUpdate();
                SetCurrentBoxObstacle(spawn, WouldCollide(spawn));
            }

            //--Move the boxes to the given transfoem until the boxes are in a location where they would not collide with anything if their collision was...
            //--turned on
            while (WouldCollide(spawn)) {
                yield return new WaitForFixedUpdate();
                SetCurrentBoxObstacle(spawn, true);
            }

            currentBoxObstacle.EnablePhysics();
            currentBoxObstacle.SetMaterialCollision();
            AbilityUsed();
            moveBoxesRoutine = null;
        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }

        //--Instantiates a new Boxes Obstacle Gameobject and return the BoxesObstacle script which is attached to it
        private BoxesObstacle InstatiateAndGetScript(Transform spawn) {
            BoxesObstacle boxes = Instantiate(abilityPrefab, spawn.position, spawn.rotation).GetComponent<BoxesObstacle>();
            return boxes;        
        }

        private Transform GetSpawnLocation() {
            Transform spawn = runnerSpawnTransform;
            if (!isCurrentlyRunner) {
                //--Get chaser Spawn Transform from Current runner
                spawn = playerAbilityController.AbilityManager.GetRunnerAbilityController().BoxSpawnAbility.chaserSpawnTransform;
            }
            return spawn;
        }

        private void SetCurrentBoxObstacle(Transform spawn, bool wouldCollide) {
            currentBoxObstacle.SetColour(wouldCollide);
            currentBoxObstacle.gameObject.transform.position = spawn.position;
            currentBoxObstacle.gameObject.transform.rotation = spawn.rotation;
        }
    }
}
