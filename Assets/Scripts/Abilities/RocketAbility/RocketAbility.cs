using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class RocketAbility : SpawnableAbility {
        //--Serialized Fields
        //[SerializeField] private int runnerMaxUses = 3;
        //[SerializeField] private int chaserMaxUses = 3;
        //--Private
        Transform spawnTransform;
        Rocket currentRocket;
        private LineRenderer _lineRenderer;

        public Transform SpawnTransform {
            get { return spawnTransform; }
            set { spawnTransform = value; }
        }

        protected override void Awake() {
            base.Awake();
            spawnTransform = runnerSpawnTransform;      //-- the runner and chaser will both spawn the rocket at the same transform 
            _lineRenderer = GetComponentInChildren<LineRenderer>();

            _lineRenderer.gameObject.SetActive(false);
        }
        public override void RoleStartSetup(bool isRunner) {
            /*usesLeft = chaserMaxUses;
            if (isRunner) {
                usesLeft = runnerMaxUses;
            }*/
            // Destroy current rocket if one exists
            if (currentRocket != null) {
                Destroy(currentRocket.gameObject);
            }
            SetLineRendererParent();
            base.RoleStartSetup(isRunner);
        }
        public override void ChangeToAbility() {
            //_lineRenderer.gameObject.SetActive(true);
            base.ChangeFromAbility();
        }

        public override void ChangeFromAbility() {
            //_lineRenderer.gameObject.SetActive(false);
            base.ChangeFromAbility();
        }

        private void SetLineRendererParent() {
            _lineRenderer.transform.parent = playerAbilityController.thisPlayer.RCC_CarController.transform;
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
            currentRocket.PlayerWhoFired = playerAbilityController.thisPlayer;      // For GUR Telemetry
            currentRocket.StartRocket(carSpeed);
            _lineRenderer.gameObject.SetActive(true);        //Turn off line renderer once fired
            AbilityUsed();
        }

        public override void OnAbilityButtonReleased<T>(T obj) {

            _lineRenderer.gameObject.SetActive(false);        //Turn off line renderer once fired

        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }


        
    }
}
