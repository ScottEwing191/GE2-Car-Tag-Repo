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
        //--Called from input handler. Will set the the current ability ti the next ability in the list. If next ability is incompatable with player role it will move onto the...
        //--one after that and so on. Will also loop back to the start of the list.
        /*public void NextAbility(bool recursiveCall = false) {
            //--If Can Switch Ability
            if (CurrentAbility.CanSwitchFrom()) {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + 1;
                if (newIndex >= abilities.Count) {      // loop the ability selection back around to the start of the list
                    newIndex = 0;
                }
                //--Method is called recursively when skipping through abilities which are incompatable with the player role. We dont want to 
                //--to call ChangeFromAbility on those abilities.
                if (!recursiveCall) {
                    CurrentAbility.ChangeFromAbility(); 
                }
                CurrentAbility = abilities[newIndex];
                //--Check if the new CUrrent ability will work with the player current role
                if (!IsAbilityCompatibleWithPlayerRole()) {
                    NextAbility(true);
                    return;                 // makes sure the UI is not set for each ability which is not compatable
                }
                CurrentAbility.ChangeToAbility();
                int abilityCompatableIndex = GetAbilityCompatableIndex();
                //thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
                thisPlayer.PlayerEvents.AbilityEvents.CurrentAbilityChanged(CurrentAbility.UsesLeft, abilityCompatableIndex);
            }
            //--If Can't Switch Ability
            else {

            }
        }*/

        public void NextAbility(bool recursiveCall = false) {
            //ChangeAbility(1, 0);
            //--If Can Switch Ability
            if (!CurrentAbility.CanSwitchFrom()) {
                return;
            }
            do {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + 1;
                if (newIndex >= abilities.Count) {
                    // loop the ability selection back around to the start of the list
                    newIndex = 0;
                }
                //--Method is called recursively when skipping through abilities which are incompatable with the player role. We dont want to 
                //--to call ChangeFromAbility on those abilities.
                if (!recursiveCall) {
                    CurrentAbility.ChangeFromAbility();
                    recursiveCall = true;
                }
                CurrentAbility = abilities[newIndex];
                //--Check if the new CUrrent ability will work with the player current role
            } while (!IsAbilityCompatibleWithPlayerRole());

            CurrentAbility.ChangeToAbility();
            int abilityCompatableIndex = GetAbilityCompatableIndex();
            //thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
            thisPlayer.PlayerEvents.AbilityEvents.CurrentAbilityChanged(CurrentAbility.UsesLeft, abilityCompatableIndex);
        }
        public void ChangeAbility(int iterateValue,int indexToLoopTo) {
            //--If Can Switch Ability
            if (!CurrentAbility.CanSwitchFrom()) {
                return;
            }

            bool alreadyChangedFrom = false;                    // set to true once ChangeFromAbility method called. stops the method from running while cycling through abilities which are not compatible with the player's role 
            do {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + iterateValue;
                if (newIndex >= abilities.Count) {
                    // loop the ability selection back around to the start of the list
                    newIndex = indexToLoopTo;
                }
                //--Method is called recursively when skipping through abilities which are incompatable with the player role. We dont want to 
                //--to call ChangeFromAbility on those abilities.
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


        public void PreviousAbility(bool recursiveCall = false) {
            //ChangeAbility(-1, abilities.Count - 1);
            //--If Can Switch Ability
            if (CurrentAbility.CanSwitchFrom()) {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex - 1;
                if (newIndex < 0) {
                    // loop the ability selection back around to the end of the list
                    newIndex = abilities.Count - 1;
                }

                //--Method is called recursively when skipping through abilities which are incompatable with the player role. We dont want to 
                //--to call ChangeFromAbility on those abilities.
                if (!recursiveCall) {
                    CurrentAbility.ChangeFromAbility();
                }

                CurrentAbility = abilities[newIndex];
                if (!IsAbilityCompatibleWithPlayerRole()) {
                    PreviousAbility();
                    return;
                }

                CurrentAbility.ChangeToAbility();
                int abilityCompatableIndex = GetAbilityCompatableIndex();
                //thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
                thisPlayer.PlayerEvents.AbilityEvents.CurrentAbilityChanged(CurrentAbility.UsesLeft, abilityCompatableIndex);
            }
            //--If Can't Switch Ability
            else {
            }
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
                // if the player has an ability selected which is not compatable with current role then move onto next ability
                NextAbility();
            }

            int abilityIndex = GetAbilityCompatableIndex();

            //thisPlayer.PlayerUIController.AbilityUI.ResetAbilityUI(CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner(), abilityIndex);
            thisPlayer.PlayerEvents.AbilityEvents.ResetAbilities(CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner(),
                abilityIndex);
            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }

            timeElapsedSinceCooldownEnd = 0; //Reset 
        }

        //--Get the index of the current ability but only out of the Abilities which are compatable with the current runner
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
            thisPlayer.PlayerScore.UpdateAbilityUsedTelemetry(CurrentAbility, timeElapsedSinceCooldownEnd);
        }

        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenAbilityUse);
            cooldownOver = true;
            timeElapsedSinceCooldownEnd = 0;
        }

        private void Update() {
            //AbilityManager.RoundManager.IsRoundRunning;
            if (thisPlayer.IsPlayerEnabled) {
                timeElapsedSinceCooldownEnd += Time.deltaTime;
            }
        }
    }
}