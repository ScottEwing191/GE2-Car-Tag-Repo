using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;


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
                                                                    //...can be used to acess other scripts on this player without going through any managers

        public void Awake() {
            BoxSpawnAbility = GetComponent<BoxSpawnAbility>();
            //defaultAbility = BoxSpawnAbility;
            CurrentAbility = abilities[0];
        }

        public void InitialSetup() {
            AbilityManager = GameManager.Instance.AbilityManager;
            thisPlayer = GetComponentInParent<Player>();
            foreach (Ability ability in abilities) {
                ability.RoleStartSetup(thisPlayer.IsThisPlayerCurrentRunner());
            }
        }

        public void OnAbilityInputStarted() {
            if (cooldownOver) {
                CurrentAbility.OnAbilityButtonPressed("");
            }
        }

        public void OnAbilityInputCancelled() {
            CurrentAbility.OnAbilityButtonReleased("");
        }

        public void NextAbility() {
            //--If Can Switch Ability
            if (CurrentAbility.CanSwitchFrom()) {
                int currentIndex = GetCurrentAbilityIndex();
                int newIndex = currentIndex + 1;
                if (newIndex >= abilities.Count) {      // loop the ability selection back around to the start of the list
                    newIndex = 0;
                }
                CurrentAbility = abilities[newIndex];
                thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, newIndex);     // update the UI
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
                thisPlayer.PlayerUIController.AbilityUI.ChangeAbilityUI(CurrentAbility.UsesLeft, newIndex);     // update the UI
            }
            //--If Can't Switch Ability
            else {

            }
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
            this.thisPlayer.PlayerUIController.AbilityUI.ResetAbilityUI(CurrentAbility.UsesLeft);
            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }
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
