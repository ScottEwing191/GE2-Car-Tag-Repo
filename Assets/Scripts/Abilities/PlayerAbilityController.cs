using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;
using System;
using System.Linq;

namespace CarTag.Abilities {
    public enum InputState { STARTED, PERFORMED, CANCELLED }
    public class PlayerAbilityController : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] private List<Ability> abilities = new List<Ability>();         // only have one ability at the moment but should need this in future
        [SerializeField] private float timeBetweenAbilityUse = 5;

        //--Private
        private Ability defaultAbility;
        private bool cooldownOver = true;
        private Coroutine abilityTimerRoutine;

        //--Auto-Implemented Properties
        public AbilityManager AbilityManager { get; set; }
        public BoxSpawnAbility BoxSpawnAbility { get; set; }
        public Ability CurrentAbility { get; set; }
        public Player thisPlayer { get; private set; }              // The Player script Attached the same player as this PlayerAbilityController script...
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
        public void NextAbility() {
            //--If Can Switch Ability
            if (CurrentAbility.CanSwitchFrom()) {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + 1;
                if (newIndex >= abilities.Count) {      // loop the ability selection back around to the start of the list
                    newIndex = 0;
                }
                CurrentAbility = abilities[newIndex];
                //--Check if the new CUrrent ability will work with the player current role
                if (!IsAbilityCompatibleWithPlayerRole()) {
                    NextAbility();
                    return;                 // makes sure the UI is not set for each ability which is not compatable
                }
                int abilityCompatableIndex = GetAbilityCompatableIndex();
                thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
            }
            //--If Can't Switch Ability
            else {

            }
        }
        public void PreviousAbility() {
            //--If Can Switch Ability
            if (CurrentAbility.CanSwitchFrom()) {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex - 1;
                if (newIndex < 0) {      // loop the ability selection back around to the end of the list
                    newIndex = abilities.Count - 1;
                }
                CurrentAbility = abilities[newIndex];
                if (!IsAbilityCompatibleWithPlayerRole()) {
                    PreviousAbility();
                    return;
                }
                int abilityCompatableIndex = GetAbilityCompatableIndex();

                thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, abilityCompatableIndex);     // update the UI
            }
            //--If Can't Switch Ability
            else {

            }
        }

        //--Returns True if the current ability is compatable with the players current role
        private bool IsAbilityCompatibleWithPlayerRole() {
            if (thisPlayer.IsThisPlayerCurrentRunner() && CurrentAbility.IsRunnerAbility) {     // player = runner | current ability is compatible with runner
                return true;
            }
            else if (!thisPlayer.IsThisPlayerCurrentRunner() && CurrentAbility.IsChaserAbility) {   // player = chaser | current ability is compatible with chaser
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
        public void ResetAbilities() {
            foreach (Ability ability in abilities) {
                ability.RoleStartSetup(thisPlayer.IsThisPlayerCurrentRunner());
            }

            if (!IsAbilityCompatibleWithPlayerRole()) {     // if the player has an ability selected which is not compatable with current role then move onto next ability
                NextAbility();
            }
            int abilityIndex = GetAbilityCompatableIndex();

            thisPlayer.PlayerUIController.AbilityUI.ResetAbilityUI(CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner(), abilityIndex);
            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }
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
            thisPlayer.PlayerUIController.AbilityUI.UpdateAbilityUIOnUse(timeBetweenAbilityUse, usesLeft);
        }

        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenAbilityUse);
            cooldownOver = true;
        }
    }
}
