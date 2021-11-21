using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;


namespace CarTag.Abilities {
    public enum InputState { STARTED, PERFORMED, CANCELLED }
    public class PlayerAbilityController : MonoBehaviour {
        //--Serialized Fields
        //[SerializeField] private List<Ability> abilities = new List<Ability>();         // only have one ability at the moment but should need this in future
        [SerializeField] private float timeBetweenAbilityUse = 5;
        //--Private
        private Ability defaultAbility;
        private bool cooldownOver = true;
        private Coroutine abilityTimerRoutine;

        //--Auto-Implemented Properties
        public AbilityManager AbilityManager { get; set; }
        public BoxSpawnAbility BoxSpawnAbility { get; set; }
        public Ability CurrentAbility { get; set; }


        public void Awake() {
            BoxSpawnAbility = GetComponent<BoxSpawnAbility>();
            defaultAbility = BoxSpawnAbility;
            CurrentAbility = defaultAbility;
        }

        public void InitialSetup() {
            AbilityManager = GameManager.Instance.AbilityManager;
        }

        public void OnAbilityInputStarted() {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            if (CanDoAbility(isRunner)) {
                CurrentAbility.OnAbilityButtonPressed(isRunner);
            }
        }

        public void OnAbilityInputCancelled() {
            CurrentAbility.OnAbilityButtonReleased("");
        }

        public void OnAbilityInput(InputState state) {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            
            if (!CanDoAbility(isRunner)) { 
                return; 
            }

            switch (state) {
                case InputState.STARTED:
                    CurrentAbility.OnAbilityButtonPressed(isRunner);
                    break;
                case InputState.PERFORMED:
                    //CurrentAbility.OnAbilityButtonHeld(isRunner);
                    break;
                case InputState.CANCELLED:
                    CurrentAbility.OnAbilityButtonReleased(isRunner);
                    break;
                default:
                    break;
            }

        }

        private bool CanDoAbility(bool isRunner) {
            if (!CurrentAbility.CanStartAbility(isRunner)) { return false; }
            if (!cooldownOver) { return false; }
            //--if isRunner and Runner cant be targeted
            return true;
        }

        public void ResetAbilities() {
            BoxSpawnAbility.Reset();

            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }
        }

        public void StartCooldown() {
            abilityTimerRoutine = StartCoroutine(AbilityCooldown());
        }

        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenAbilityUse);
            cooldownOver = true;
        }

    }
}
