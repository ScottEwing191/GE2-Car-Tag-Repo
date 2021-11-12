using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.Abilities.BoxSpawn;


namespace CarTag.Abilities {
    /*public enum Role { CHASER, RUNNER }*/
    public class PlayerAbilityController : MonoBehaviour {
        //--Public 
        //--Private
        private Ability defaultAbility;

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

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I)) {
                OnAbilityButtonPressed();
            }
            if (UnityEngine.Input.GetKey(KeyCode.I)) {
                OnAbilityButtonHeld();
            }
            if (UnityEngine.Input.GetKeyUp(KeyCode.I)) {
                OnAbilityButtonReleased();
            }
        }

        public void OnAbilityButtonPressed() {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            CurrentAbility.OnAbilityButtonPressed(isRunner);
        }

        public void OnAbilityButtonHeld() {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            CurrentAbility.OnAbilityButtonHeld(isRunner);
        }

        public void OnAbilityButtonReleased() {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            CurrentAbility.OnAbilityButtonReleased(isRunner);
        }

        //=== PRIVATE METHODS ===
        /*public bool GetIsRunner() {
            bool isRunner = AbilityManager.IsControllerAttachedToRunner(this);
            Role role = Role.CHASER;
            if (isRunner) {
                role = Role.RUNNER;
            }
            return role;
        }*/
    }
}
