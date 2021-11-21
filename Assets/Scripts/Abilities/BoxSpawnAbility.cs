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
        private bool isCurrentlyRunner;
        private bool inputReleased = false;
        private Transform currentSpawn;

        protected override void Awake() {
            base.Awake();
            runnerUsesLeft = runnerMaxUses;
            chaserUsesLeft = chaserMaxUses;
        }

        public override bool CanStartAbility(bool isRunner) {
            if (moveBoxesRoutine != null) {           // check if box spawning routine has finished before allowing new box to be spawned
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

        //-- Obj is a bool which corresponds to whether this Box is being spawned by a runner or a chaser
        //-- True = Runner | False = Chaser     ||      T obj must be converted to bool? before be cast to regular bool
        public override void OnAbilityButtonPressed<T>(T obj ) {
            isCurrentlyRunner = (bool)(obj as bool?);
            if (!CanStartAbility(isCurrentlyRunner)) {
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

        public override void OnAbilityButtonHeld<T>(T obj) {
        }

        public override void OnAbilityButtonReleased<T>(T obj) {
            if (moveBoxesRoutine == null) {         // Catches error where user presses input key while ability cooldown is active but releases it after the cooldown timer is up
                return;
            }
            inputReleased = true;
            SetUsesLeftCounter();
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
            playerAbilityController.StartCooldown();                                // start the cooldown in the Ability Controller
            moveBoxesRoutine = null;
        }

        //--Keeps track of how many times the ability can be used
        private void SetUsesLeftCounter() {
            if (isCurrentlyRunner) {
                runnerUsesLeft--;
            }
            else {
                chaserUsesLeft--;
            }
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

            //--Destroy Instantiated Boxes
            foreach (var box in boxesObstacles) {
                Destroy(box.gameObject);
            }
            boxesObstacles.Clear();
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
