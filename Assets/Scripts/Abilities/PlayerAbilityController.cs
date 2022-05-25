using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;
using System;
using System.Linq;

namespace CarTag.Abilities{
    public enum InputState{
        STARTED,
        PERFORMED,
        CANCELLED
    }

    public class PlayerAbilityController : MonoBehaviour{
        //--Serialized Fields
        [SerializeField]
        private List<Ability> abilities = new List<Ability>(); // only have one ability at the moment but should need this in future

        [SerializeField] private float timeBetweenAbilityUse = 5;

        //--Private
        private Ability defaultAbility;
        private bool cooldownOver = true;
        private Coroutine abilityTimerRoutine;
        public float timeElapsedSinceCooldownEnd = 0; // this is used to gather telemetry data for GUR Module

        //--Auto-Implemented Properties
        public AbilityManager AbilityManager { get; set; }
        public BoxSpawnAbility BoxSpawnAbility { get; set; }
        public Ability CurrentAbility { get; set; }

        public Player
            thisPlayer { get; private set; } // The Player script Attached the same player as this PlayerAbilityController script...
        //private Player thisPlayer;                                    // The Player script Attached the same player as this PlayerAbilityController script...
        //...can be used to acess other scripts on this player without going through any managers

        public void Awake() {
            BoxSpawnAbility = GetComponent<BoxSpawnAbility>();
            //defaultAbility = BoxSpawnAbility;
            CurrentAbility = abilities[0];
        }

        public void InitialSetup() {
            AbilityManager = GameManager.Instance.AbilityManager;
            thisPlayer = GetComponentInParent<Player>();
            //-- if the initial ability is not compatable with the players current role then move onto the next one.
            if (!IsAbilityCompatibleWithPlayerRole()) {
                NextAbility();
            }

            foreach (Ability ability in abilities) {
                ability.RoleStartSetup(thisPlayer.IsThisPlayerCurrentRunner());
            }
        }

        public void OnAbilityInputStarted() {
            if (cooldownOver && thisPlayer.IsPlayerEnabled) {
                CurrentAbility.OnAbilityButtonPressed("");
            }
        }

        public void OnAbilityInputCancelled() {
            CurrentAbility.OnAbilityButtonReleased("");
        }
        //--Called from input handler. Will set the the current ability to the next/previous ability in the list. If next/previous ability is incompatible with player role it will move onto the...
        //--one after that and so on. Will also loop back to the start/end of the list.
        public void NextAbility() {
            ChangeAbility(true);
        }

        public void PreviousAbility() {
            ChangeAbility(false);
        }

        private void ChangeAbility(bool goToNext) {
            //--If Can Switch Ability
            if (!CurrentAbility.CanSwitchFrom()) {
                return;
            }
            // set to true once ChangeFromAbility method called. stops the method from running while cycling through abilities which are not compatible with the player's role
            bool alreadyChangedFrom = false;
            int iterateValue = (goToNext) ? 1 : -1;         // controls whether the method increments/decrements to the next or previous index in the list
            do {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + iterateValue;
                
                //--Controls whether the index loops back to the start or end of the list
                if (goToNext && newIndex >= abilities.Count) {
                    newIndex = 0;                       // loop the ability selection back around to the start of the list
                }
                if (!goToNext && newIndex < 0) {
                    newIndex = abilities.Count - 1;     // loop the ability selection back around to the end of the list
                }

                //--We only need to run this method for the original ability. If the new ability is not compatable with the player role we
                //-- don't need to run the method when going round the loop again. Since the new ability cannot have been activated
                if (!alreadyChangedFrom) {
                    CurrentAbility.ChangeFromAbility();
                    alreadyChangedFrom = true;
                }

                CurrentAbility = abilities[newIndex];
                //--Check if the new CUrrent ability will work with the player current role
            } while (!IsAbilityCompatibleWithPlayerRole());

            CurrentAbility.ChangeToAbility();
            int abilityCompatableIndex = GetAbilityCompatableIndex();
            //thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
            thisPlayer.PlayerEvents.AbilityEvents.CurrentAbilityChanged(CurrentAbility.UsesLeft, abilityCompatableIndex);
        }


        //--Returns True if the current ability is compatable with the players current role
        private bool IsAbilityCompatibleWithPlayerRole() {
            if (thisPlayer.IsThisPlayerCurrentRunner() && CurrentAbility.IsRunnerAbility) {
                // player = runner | current ability is compatible with runner
                return true;
            }
            else if (!thisPlayer.IsThisPlayerCurrentRunner() && CurrentAbility.IsChaserAbility) {
                // player = chaser | current ability is compatible with chaser
                return true;
            }

            return false;
        }

        private int GetCurrentAbilityIndex() {
            for (int i = 0; i < abilities.Count; i++) {
                if (abilities[i] == CurrentAbility) {
                    return i;
                }
            }

            Debug.Log("Couldn't find current ability in ability list");
            return -1;
        }

        public void ResetAbilities(bool isNewRunner) {
            foreach (Ability ability in abilities) {
                //ability.RoleStartSetup(thisPlayer.IsThisPlayerCurrentRunner());
                ability.RoleStartSetup(isNewRunner);
            }

            if (!IsAbilityCompatibleWithPlayerRole()) {
                // if the player has an ability selected which is not compatible with current role then move onto next ability
                NextAbility();
            }

            int abilityIndex = GetAbilityCompatableIndex();

            //thisPlayer.PlayerUIController.AbilityUI.ResetAbilityUI(CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner(), abilityIndex);
            thisPlayer.PlayerEvents.AbilityEvents.ResetAbilities(CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner(), abilityIndex);
            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }

            timeElapsedSinceCooldownEnd = 0; //Reset 
        }

        //--Get the index of the current ability but only out of the Abilities which are compatible with the current runner
        private int GetAbilityCompatableIndex() {
            int index = -1;
            if (thisPlayer.IsThisPlayerCurrentRunner()) {
                index = abilities.Where(i => i.isRunnerAbility).ToList().FindIndex(a => a == CurrentAbility);
            }
            else {
                index = abilities.Where(i => i.isChaserAbility).ToList().FindIndex(a => a == CurrentAbility);
            }
            return index;
        }

        public void CurrentAbilityUsed(int usesLeft) {
            abilityTimerRoutine = StartCoroutine(AbilityCooldown());
            //thisPlayer.PlayerUIController.AbilityUI.UpdateAbilityUIOnUse(timeBetweenAbilityUse, usesLeft);
            thisPlayer.PlayerEvents.AbilityEvents.AbilityUsed(timeBetweenAbilityUse, usesLeft);
            //--Update the player Ability telemetry for Games User Research module
            //thisPlayer.PlayerScore.UpdateAbilityUsedTelemetry(CurrentAbility, timeElapsedSinceCooldownEnd);
        }

        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenAbilityUse);
            cooldownOver = true;
            timeElapsedSinceCooldownEnd = 0;
        }

        private void Update() {
            //AbilityManager.RoundManager.IsRoundRunning;
            /*if (thisPlayer.IsPlayerEnabled) {
                timeElapsedSinceCooldownEnd += Time.deltaTime;
            }*/
        }
    }
}