using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class RocketAbility : SpawnableAbility {
        //--Private
        Transform spawnTransform;
        Rocket currentRocket;
        private LineRenderer _lineRenderer;
        private bool isAimingRocket = false;         // true while the player is holding down the fire button (aiming the rocket)

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
            // Destroy current rocket if one exists
            if (currentRocket != null) {
                Destroy(currentRocket.gameObject);
            }
            SetLineRendererParent();
            _lineRenderer.gameObject.SetActive(false);
            isAimingRocket = false;         // stops rocket from firing if button was held before the role swap and released after 
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

        //--Sets the parent of the line rendere either to the Runner or Chaser Car Controller gameobjects
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
            if (!CanStartAbility()) { return; }
            isAimingRocket = true;
            _lineRenderer.gameObject.SetActive(true);        //Turn off line renderer once fired
        }
        //--Set the length of the line renderer
        //public override void OnAbilityButtonHeld<T>(T obj) {
        private void Update() {
            if (!isAimingRocket) { return; }    // Only fire rocket if aiming was succesfully started
            RaycastHit hit;
            Physics.Raycast(spawnTransform.position, spawnTransform.forward, out hit, 100, collisionCheckLayers.value);
            float distance = 100;
            if (hit.transform != null) {
                distance = Vector3.Distance(spawnTransform.position, hit.transform.position);
            }
            _lineRenderer.SetPosition(1, Vector3.forward * distance);
        }
        public override void OnAbilityButtonReleased<T>(T obj) {

            if (!isAimingRocket) { return; }    // Only fire rocket if aiming was succesfully started

            currentRocket = Instantiate(abilityPrefab, spawnTransform.position, spawnTransform.rotation).GetComponent<Rocket>();
            LayerMask collidableLayers = collisionCheckLayers;
            //float carSpeed = playerAbilityController.thisPlayer.CarController.CurrentSpeed;
            float carSpeed = playerAbilityController.thisPlayer.RCC_CarController.GetComponent<Rigidbody>().velocity.magnitude;
            currentRocket.GetComponent<Rocket>().SetSpawner(playerAbilityController.thisPlayer);
            currentRocket.PlayerWhoFired = playerAbilityController.thisPlayer;      // For GUR Telemetry
            currentRocket.StartRocket(carSpeed);
            //_lineRenderer.gameObject.SetActive(true);        //Turn off line renderer once fired
            AbilityUsed();

            _lineRenderer.gameObject.SetActive(false);        //Turn off line renderer once fired
            isAimingRocket = false;
        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }



    }
}
