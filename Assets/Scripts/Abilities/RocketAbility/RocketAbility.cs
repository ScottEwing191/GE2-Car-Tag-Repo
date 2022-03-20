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
        Transform _spawnTransform;
        Rocket _currentRocket;
        private LineRenderer _lineRenderer;

        public Transform SpawnTransform {
            get { return _spawnTransform; }
            set { _spawnTransform = value; }
        }

        protected override void Awake() {
            base.Awake();
            _spawnTransform = runnerSpawnTransform;      //-- the runner and chaser will both spawn the rocket at the same transform 
            _lineRenderer = GetComponentInChildren<LineRenderer>();
        }
        public override void RoleStartSetup(bool isRunner) {
            usesLeft = chaserMaxUses;
            /*if (isRunner) {
                usesLeft = runnerMaxUses;
            }*/
            // Destroy current rocket if one exists
            if (_currentRocket != null) {
                Destroy(_currentRocket.gameObject);
            }
            //--Set the rocket aim Line Renderer's Parent
            SetLineRendererParent();
            base.RoleStartSetup(isRunner);
        }

        private void SetLineRendererParent() {
            //_lineRenderer.transform.parent = playerAbilityController.thisPlayer.RCC_CarController.transform;
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
            _currentRocket = Instantiate(abilityPrefab, _spawnTransform.position, _spawnTransform.rotation).GetComponent<Rocket>();
            LayerMask collidableLayers = collisionCheckLayers;
            //float carSpeed = playerAbilityController.thisPlayer.CarController.CurrentSpeed;
            float carSpeed = playerAbilityController.thisPlayer.RCC_CarController.GetComponent<Rigidbody>().velocity.magnitude;
            _currentRocket.GetComponent<Rocket>().SetSpawner(playerAbilityController.thisPlayer);
            _currentRocket.PlayerWhoFired = playerAbilityController.thisPlayer;      // For GUR Telemetry
            _currentRocket.StartRocket(carSpeed);
            AbilityUsed();
        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }


        
    }
}
