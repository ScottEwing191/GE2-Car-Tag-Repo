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
        private Player thisPlayer;                               // The Player script Attached the same player as this PlayerAbilityController script...
                                                                //...can be used to acess other scripts on this player without going through any managers
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
            thisPlayer = GetComponentInParent<Player>();
            BoxSpawnAbility.RoleStartSetup(thisPlayer.IsThisPlayerCurrentRunner());
        }

        public void OnAbilityInputStarted() {
            if (cooldownOver) {
                CurrentAbility.OnAbilityButtonPressed("");
            }
        }

        public void OnAbilityInputCancelled() {
            CurrentAbility.OnAbilityButtonReleased("");
        }

        public void ResetAbilities() {
            BoxSpawnAbility.RoleStartSetup(this.thisPlayer.IsThisPlayerCurrentRunner());

            //-Reset use ability timer
            if (abilityTimerRoutine != null) {
                StopCoroutine(abilityTimerRoutine);
                abilityTimerRoutine = null;
                cooldownOver = true;
            }
        }

        public void CurrentAbilityUsed(int usesLeft) {
            abilityTimerRoutine = StartCoroutine(AbilityCooldown());
            thisPlayer.PlayerUIController.UpdateAbilityUIOnUse(timeBetweenAbilityUse, usesLeft);
        }

        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenAbilityUse);
            cooldownOver = true;
        }
    }
}
