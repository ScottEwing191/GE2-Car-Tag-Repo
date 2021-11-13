using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;


namespace CarTag.Abilities.BoxSpawn {
    public class BoxSpawnAbility : SpawnableAbility {
        //--Serialized Fields
        [SerializeField] private float coolDownTimer = 10;

        //--Private
        private BoxesObstacle currentBoxObstacle;
        private Coroutine moveBoxesRoutine;
        private Coroutine moveBoxesUntilValidRoutine;
        
        //=== PUBLIC METHODS ===

        public override bool CanStartAbility() {
            if (moveBoxesUntilValidRoutine == null) {
                return true;
            }
            return false;
        }

        //-- Obj is a bool which corresponds to whether this Box is being spawned by a runner or a chaser
        //-- True = Runner | False = Chaser     ||      T obj must be converted to bool? in order to be passed into another method 
        public override void OnAbilityButtonPressed<T>(T obj ) {
            Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            currentBoxObstacle = InstatiateAndGetScript(spawn);                     // instantiate Prefab
            currentBoxObstacle.DisablePhysics();                                    // turn Off Collision
            currentBoxObstacle.SetMaterialNoCollision();                            // Switch to transparent material

        }

        //-- Obj is a bool which corresponds to whether this Box is being spawned by a runner or a chaser
        //-- True = Runner | False = Chaser     ||      T obj must be converted to bool? in order to be passed into another method 
        public override void OnAbilityButtonHeld<T>(T obj) {
            Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            moveBoxesRoutine = StartCoroutine(MoveBoxesRoutine(spawn));
        }

        //-- Obj is a bool which corresponds to whether this Box is being spawned by a runner or a chaser
        //-- True = Runner | False = Chaser     ||      T obj must be converted to bool? in order to be passed into another method 
        public override void OnAbilityButtonReleased<T>(T obj) {
            if (moveBoxesRoutine == null) {         // Catches error where user presses input key while ability colldown is active but releases it after the timer is up
                return;
            }
            StopCoroutine(moveBoxesRoutine);                                        // stop routine from moving boxes every frame
            moveBoxesRoutine = null;
            Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            if (!WouldCollide(spawn)) {                 
                currentBoxObstacle.EnablePhysics();
                currentBoxObstacle.SetMaterialCollision();
                StartCoroutine(playerAbilityController.AbilityCooldown());          // start the cooldown in the Ability Controller
            }
            else {
                moveBoxesUntilValidRoutine =  StartCoroutine(MoveBoxesUntilInValidLocation(spawn));   // move Boxes until in valid position
            }
        }

        //=== PRIVATE METHODS ===

        //--Instantiates a new Boxes Obstacle Gameobject and return the BoxesObstacle script which is attached to it
        private BoxesObstacle InstatiateAndGetScript(Transform spawn) {
            BoxesObstacle boxes = Instantiate(abilityPrefab, spawn.position, spawn.rotation).GetComponent<BoxesObstacle>();
            return boxes;        
        }

        /// <summary>
        /// "obj as bool" is not allowed since "bool is not a nullable type"
        //  "(bool) obj" is not alowed since "cant convert T to bool"
        //  Solution was to pass in obj as a nullable bool so that "obj as bool?" would work. Then it could be cast to a regular bool
        /// </summary>
        private Transform GetSpawnLocationFromNullableBool(bool? nullableIsRunner) {
            bool isRunner = (bool)(nullableIsRunner as bool?);
            Transform spawn = runnerSpawnTransform;
            if (!isRunner) {
                //--Get chaser Spawn Transform from Current runner
                spawn = playerAbilityController.AbilityManager.GetRunnerAbilityController().boxSpawnAbility.chaserSpawnTransform;
            }
            return spawn;
        }

        //--Keep moving the boxes to the given transform. This is stop when the player releases the Ability input
        private IEnumerator MoveBoxesRoutine(Transform spawn) {
            while (true) {
                yield return new WaitForFixedUpdate();
                currentBoxObstacle.SetColour(WouldCollide(spawn));
                currentBoxObstacle.gameObject.transform.position = spawn.position;
                currentBoxObstacle.gameObject.transform.rotation = spawn.rotation;
            }
        }

        //--Move the boxes to the given transfoem until the boxes are in a location where they would not collide with anything if their collision was...
        //--turned on
        private IEnumerator MoveBoxesUntilInValidLocation(Transform spawn) {
            while (WouldCollide(spawn)) {
                yield return new WaitForFixedUpdate();
                currentBoxObstacle.SetColour(true);
                currentBoxObstacle.gameObject.transform.position = spawn.position;
                currentBoxObstacle.gameObject.transform.rotation = spawn.rotation;
            }
            currentBoxObstacle.EnablePhysics();
            moveBoxesUntilValidRoutine = null;
            currentBoxObstacle.SetMaterialCollision();
            StartCoroutine(playerAbilityController.AbilityCooldown());              // start the cooldown in the Ability Controller
        }

    }
}
