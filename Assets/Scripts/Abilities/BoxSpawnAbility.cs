using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;


namespace CarTag.Abilities.BoxSpawn {
//    public enum BoxSpawnLocation { CHASER, RUNNER }
    public class BoxSpawnAbility : SpawnableAbility {
        //--Private
        private BoxesObstacle currentBoxObstacle;

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.O)) {
                InstatiateAndGetScript(true);
            }
        }

        //=== PUBLIC METHODS ===

        public override void OnAbilityButtonPressed<T>(T obj ) {
            //--Make Role enum is passed in
            if (typeof(T) != typeof(bool)) {
                Debug.LogError("Wrong type is passed into BoxSpawnAbility Button Pressed Method." + typeof(T) + " != " + typeof(int));
                return;
            }
            //--"obj as bool" is not allowed since "bool is not a nullable type"
            //--"(bool) obj" is not alowed since "cant convert T to bool"
            //--Solution was to pass in obj as a nullable bool so that "obj as bool?" would work. Then it could be cast to a regular bool
            bool isRunner = (bool)(obj as bool?);

            //--Instantiate Prefab
            currentBoxObstacle = InstatiateAndGetScript(isRunner);
            //--Turn Off Collision
            currentBoxObstacle.DisablePhysics();
            
        }

        public override void OnAbilityButtonHeld<T>(T obj) {
            //--Convert Nullable Bool to bool
            bool isRunner = (bool)(obj as bool?);
            Transform location = chaserSpawnTransform;
            if (isRunner) {
                location = runnerSpawnTransform;
            }
            currentBoxObstacle.gameObject.transform.position = location.position;
            currentBoxObstacle.gameObject.transform.rotation = location.rotation;

            //--CheckCollision
            //--Green or red affect accordingly
        }

        public override void OnAbilityButtonReleased<T>(T obj) {
            //--Convert Nullable Bool to bool
            bool isRunner = (bool)(obj as bool?);
            Transform location = chaserSpawnTransform;
            if (isRunner) {
                location = runnerSpawnTransform;
            }
            //--Check collision
            if (!WouldCollide(location)) {
                currentBoxObstacle.EnablePhysics();
            }
            //--If ok Turn on collision
            //--    Make prefab appear solid
            //--Start Coroutine to Enable next possible time 
        }
        

        //=== PRIVATE METHODS ===
        /// <summary>
        /// Instantiates a new Boxes Obstacle Gameobject and return the BoxesObstacle script which is attached to it
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private BoxesObstacle InstatiateAndGetScript(bool? isRunner) {
            Transform spawn = chaserSpawnTransform;
            if (isRunner == true) {
                spawn = chaserSpawnTransform;
            }
            BoxesObstacle boxes = Instantiate(abilityPrefab, spawn.position, spawn.rotation).GetComponent<BoxesObstacle>();
            return boxes;        
        }

    }
}
