using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities {
    public class SlowTimeAbility : Ability {
        [SerializeField] private float slowDuration = 2;
        [SerializeField] private float angularVelStrength = 2f;
        [SerializeField] private float liniearVelStrength = 2;

        [SerializeField] private float slowTimeScale = 0.4f;
        private Player thisPlayer;
        private Coroutine slowTimeRoutine;
        private float originalAngularStrength;
        private float originalLinearStrength;


        protected override void Awake() {
            base.Awake();
            thisPlayer = GetComponentInParent<Player>();
            isRunnerAbility = false;
        }

        private void Start() {
            originalAngularStrength = thisPlayer.RCC_CarController.steerHelperAngularVelStrength;
            originalLinearStrength = thisPlayer.RCC_CarController.steerHelperLinearVelStrength;

        }

        private void OnEnable() {
            thisPlayer.roleSwapEvent += OnRoleSwap;
        }
        private void OnDisable() {
            thisPlayer.roleSwapEvent -= OnRoleSwap;

        }
        //--Can be implemented by each Spawnable ability to determine if they can be activated
        public override bool CanStartAbility() {
            //--Can't use if Runner
            if (playerAbilityController.thisPlayer.IsThisPlayerCurrentRunner()) {
                return false;
            }
            if (usesLeft <= 0) {
                return false;
            }
            return true;
        }
        public override bool CanSwitchFrom() {
            if (slowTimeRoutine != null) {
                return false;
            }
            return true;
        }

        public override void RoleStartSetup(bool isRunner) {

        }
        public override void OnAbilityButtonPressed<T>(T obj) {
            if (!CanStartAbility()) {
                return;
            }
            if (slowTimeRoutine != null) {      // End Ability when the player clicks again
                StopCoroutine(slowTimeRoutine);
                DisableSlowMo();
                return;
            }
            slowTimeRoutine = StartCoroutine(SlowTimeRoutine());
        }

        private IEnumerator SlowTimeRoutine() {
            Time.timeScale = slowTimeScale;
            SetCarController(angularVelStrength, liniearVelStrength);
            yield return new WaitForSeconds(slowDuration);
            DisableSlowMo();
        }

        private void SetCarController(float angular, float linear) {
            thisPlayer.RCC_CarController.steerHelperAngularVelStrength = angular;
            thisPlayer.RCC_CarController.steerHelperLinearVelStrength = linear;
        }

        private void DisableSlowMo() {
            Time.timeScale = 1;
            SetCarController(originalAngularStrength, originalLinearStrength);
            AbilityUsed();
            slowTimeRoutine = null;
        }

        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }

        private void OnRoleSwap() {
            print("Ability Role Swap");
            if (slowTimeRoutine != null) {
                DisableSlowMo();
            }
        }
        public override void Reset() {

        }
    }
}
