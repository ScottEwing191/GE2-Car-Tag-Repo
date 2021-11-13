using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;


namespace CarTag.Abilities {
    public enum InputState { STARTED, PERFORMED, CANCELLED}
    public class PlayerAbilityController : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] private float timeBetweenUses = 5;
        //--Private
        private Ability defaultAbility;
        private bool cooldownOver = true;

        //--Auto-Implemented Properties
        public AbilityManager AbilityManager { get; set; }
        public BoxSpawnAbility boxSpawnAbility { get; set; }
        public Ability CurrentAbility { get; set; }


        public void Awake() {
            boxSpawnAbility = GetComponent<BoxSpawnAbility>();
            defaultAbility = boxSpawnAbility;
            CurrentAbility = defaultAbility;
        }

        public void InitialSetup() {
            AbilityManager = GameManager.Instance.AbilityManager;
        }

        public void OnAbilityInput(InputState state) {
            if (!CurrentAbility.CanStartAbility() || !cooldownOver) {
                print("Cant Activate Ability");
                return;
            }
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);

            switch (state) {
                case InputState.STARTED:
                    CurrentAbility.OnAbilityButtonPressed(isRunner);
                    break;
                case InputState.PERFORMED:
                    CurrentAbility.OnAbilityButtonHeld(isRunner);
                    break;
                case InputState.CANCELLED:
                    CurrentAbility.OnAbilityButtonReleased(isRunner);
                    break;
                default:
                    break;
            }
            
        }
        /// <summary>
        /// Started from within Curent Ability Script
        /// </summary>
        public IEnumerator AbilityCooldown() {
            cooldownOver = false;
            yield return new WaitForSeconds(timeBetweenUses);
            cooldownOver = true;
        }
        

        //=== PRIVATE METHODS ===
        
    }
}
