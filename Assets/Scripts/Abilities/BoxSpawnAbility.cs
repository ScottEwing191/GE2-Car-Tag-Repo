using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;


namespace CarTag.Abilities.BoxSpawn {
    public class BoxSpawnAbility : SpawnableAbility {
        //--Serialized Fields
        //[SerializeField] private float coolDownTimer = 10;
        [SerializeField] private int runnerMaxUses = 3;
        [SerializeField] private int chaserMaxUses = 3;


        //--Private
        private List<BoxesObstacle> boxesObstacles = new List<BoxesObstacle>();
        private int runnerUsesLeft;
        private int chaserUsesLeft;
        private BoxesObstacle currentBoxObstacle;
        private Coroutine moveBoxesRoutine;
        private Coroutine moveBoxesUntilValidRoutine;
        private bool isCurrentlyRunner;
        private Transform currentSpawn;


        //=== UNITY METHODS ==
        protected override void Awake() {
            base.Awake();
            runnerUsesLeft = runnerMaxUses;
            chaserUsesLeft = chaserMaxUses;
        }

        //=== PUBLIC METHODS ===

        public override bool CanStartAbility(bool isRunner) {
            if (moveBoxesUntilValidRoutine != null) {           // check if box spawning routine has finished before allowing new box to be spawned
                return false;
            }
            if (isRunner && runnerUsesLeft <= 0) {
                return false;
            }
            else if (!isRunner && chaserUsesLeft <= 0) {
                return false;
            }
            return true;
        }

        //-- ***
        //-- Obj is a bool which corresponds to whether this Box is being spawned by a runner or a chaser
        //-- True = Runner | False = Chaser     ||      T obj must be converted to bool? in order to be passed into another method 
        public override void OnAbilityButtonPressed<T>(T obj ) {
            //isCurrentlyRunner = (bool)(obj as bool?);
            //currentSpawn = GetSpawnLocationFromNullableBool(obj as bool?);
            Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            currentBoxObstacle = InstatiateAndGetScript(spawn);                     // instantiate Prefab
            currentBoxObstacle.DisablePhysics();                                    // turn Off Collision
            currentBoxObstacle.SetMaterialNoCollision();                            // Switch to transparent material
            boxesObstacles.Add(currentBoxObstacle);
            moveBoxesRoutine = StartCoroutine(MoveBoxesRoutine(spawn));

        }

        //-- See Comment Above ***
        public override void OnAbilityButtonHeld<T>(T obj) {
            //Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            //moveBoxesRoutine = StartCoroutine(MoveBoxesRoutine(spawn));
            
        }

        //-- See Comment Above ***
        public override void OnAbilityButtonReleased<T>(T obj) {
            if (moveBoxesRoutine == null) {         // Catches error where user presses input key while ability cooldown is active but releases it after the cooldown timer is up
                return;
            }
            StopCoroutine(moveBoxesRoutine);                                        // stop routine from moving boxes every frame
            moveBoxesRoutine = null;
            Transform spawn = GetSpawnLocationFromNullableBool(obj as bool?);       // get spawn Transform corresponding to bool?
            if (!WouldCollide(spawn)) {                 
                currentBoxObstacle.EnablePhysics();
                currentBoxObstacle.SetMaterialCollision();
                playerAbilityController.StartCooldown();                                // start the cooldown in the Ability Controller
            }
            else {
                moveBoxesUntilValidRoutine =  StartCoroutine(MoveBoxesUntilInValidLocation(spawn));   // move Boxes until in valid position
            }

            SetUseCounter(obj as bool?);        
        }

        public override void Reset() {
            //--Reset Uses Counters
            runnerUsesLeft = runnerMaxUses;
            chaserUsesLeft = chaserMaxUses;
            
            //--Stop Coroutines
            if (moveBoxesRoutine != null) {
                StopCoroutine(moveBoxesRoutine);
                moveBoxesRoutine = null;
            }
            if (moveBoxesUntilValidRoutine != null) {
                StopCoroutine(moveBoxesUntilValidRoutine);
                moveBoxesUntilValidRoutine = null;
            }

            //--Destroy Instantiated Boxes
            foreach (var box in boxesObstacles) {
                Destroy(box.gameObject);
            }
            boxesObstacles.Clear();
        }

        //=== PRIVATE METHODS ===

        //--Keeps track of how many times the ability can be used
        private void SetUseCounter(bool? nullableIsRunner) {
            bool isRunner = (bool)(nullableIsRunner as bool?);
            if (isRunner) {
                runnerUsesLeft--;
            }
            else {
                chaserUsesLeft--;
            }
        }

        //--Instantiates a new Boxes Obstacle Gameobject and return the BoxesObstacle script which is attached to it
        private BoxesObstacle InstatiateAndGetScript(Transform spawn) {
            BoxesObstacle boxes = Instantiate(abilityPrefab, spawn.position, spawn.rotation).GetComponent<BoxesObstacle>();
            return boxes;        
        }

        //--"obj as bool" is not allowed since "bool is not a nullable type"
        //--"(bool) obj" is not alowed since "cant convert T to bool"
        //--Solution was to pass in obj as a nullable bool so that "obj as bool?" would work. Then it could be cast to a regular bool
        private Transform GetSpawnLocationFromNullableBool(bool? nullableIsRunner) {
            bool isRunner = (bool)(nullableIsRunner as bool?);
            Transform spawn = runnerSpawnTransform;
            if (!isRunner) {
                //--Get chaser Spawn Transform from Current runner
                spawn = playerAbilityController.AbilityManager.GetRunnerAbilityController().BoxSpawnAbility.chaserSpawnTransform;
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
            playerAbilityController.StartCooldown();                                // start the cooldown in the Ability Controller
        }

    }
}
