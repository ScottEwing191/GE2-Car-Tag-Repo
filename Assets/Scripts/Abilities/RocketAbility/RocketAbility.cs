using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class RocketAbility : SpawnableAbility {
        //--Serialized Fields
        [SerializeField] private int runnerMaxUses = 3;
        [SerializeField] private int chaserMaxUses = 3;
        //--Private
        Transform spawnTransform;
        Rocket currentRocket;

        protected override void Awake() {
            base.Awake();
            spawnTransform = runnerSpawnTransform;      //-- the runner and chaser will both spawn the rocket at the same transform 
        }
        public override void RoleStartSetup(bool isRunner) {
            usesLeft = chaserMaxUses;
            if (isRunner) {
                usesLeft = runnerMaxUses;
            }
            // Destroy current rocket if one exists
            if (currentRocket != null) {
                Destroy(currentRocket.gameObject);
            }
        }
        public override bool CanStartAbility() {
            if (usesLeft <= 0) {
                return false;
            }
            return true;
        }

        public override void OnAbilityButtonPressed<T>(T obj) {
            if (!CanStartAbility()) {
                return;
            }
            currentRocket = Instantiate(abilityPrefab, spawnTransform.position, spawnTransform.rotation).GetComponent<Rocket>();
            LayerMask collidableLayers = collisionCheckLayers;
            //float carSpeed = playerAbilityController.thisPlayer.CarController.CurrentSpeed;
            float carSpeed = playerAbilityController.thisPlayer.RCC_CarController.GetComponent<Rigidbody>().velocity.magnitude;
            currentRocket.GetComponent<Rocket>().SetSpawner(playerAbilityController.thisPlayer);
            currentRocket.StartRocket(carSpeed);
            AbilityUsed();
        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }


        
    }
}
