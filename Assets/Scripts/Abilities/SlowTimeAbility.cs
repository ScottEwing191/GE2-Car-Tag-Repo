using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities
{
    public class SlowTimeAbility : Ability
    {
        [SerializeField] private float slowDuration = 2;
        [SerializeField] private float angularVelStrength = 2f;
        [SerializeField] private float liniearVelStrength = 2;

        [SerializeField] private float slowTimeScale = 0.4f;
        private Player thisPlayer;
        private Coroutine slowTimeRoutine;

        protected override void Awake() {
            base.Awake();
            thisPlayer = GetComponentInParent<Player>();
        }

        private void Start() {
            
        }
        //--Can be implemented by each Spawnable ability to determine if they can be activated
        public override bool CanStartAbility() {
            if (slowTimeRoutine!= null) {
                return false;
            }
            if (usesLeft <= 0) {
                return false;
            }
            return true;
        }
        public override bool CanSwitchFrom() {
            return true;
        }

        public override void RoleStartSetup(bool isRunner) {

        }
        public override void OnAbilityButtonPressed<T>(T obj) {
            slowTimeRoutine = StartCoroutine(SlowTimeRoutine());
        }

        private IEnumerator SlowTimeRoutine() {
            Time.timeScale = slowTimeScale;
            float originalSteerHelperAngularVelStrength = thisPlayer.RCC_CarController.steerHelperAngularVelStrength;
            float originalSteerHelperLinearVelStrength = thisPlayer.RCC_CarController.steerHelperLinearVelStrength;

            thisPlayer.RCC_CarController.steerHelperAngularVelStrength = angularVelStrength;
            thisPlayer.RCC_CarController.steerHelperLinearVelStrength = liniearVelStrength;

            yield return new WaitForSeconds(slowDuration);
            Time.timeScale = 1;
            //thisPlayer.RCC_CarController.steerHelperAngularVelStrength = originalSteerHelperAngularVelStrength;
            //thisPlayer.RCC_CarController.steerHelperLinearVelStrength = originalSteerHelperLinearVelStrength;

            AbilityUsed();
        }



        private void AbilityUsed() {
            usesLeft--;          //--Keeps track of how many times the ability can be used
            playerAbilityController.CurrentAbilityUsed(usesLeft);
        }

        public override void Reset() {

        }
    }
}
