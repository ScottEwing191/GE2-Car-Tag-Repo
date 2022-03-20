using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        //[SerializeField] protected bool isRunnerAbility = true;
        public bool isRunnerAbility = false;
        public bool isChaserAbility = false;

        [ShowIf("isRunnerAbility")] public int runnerMaxUses = 3;                   // The number of times the runner can use this ability

        [ShowIf("isChaserAbility")] public int chaserMaxUses = 3;                   // The number of times the chaser can use this ability
        protected PlayerAbilityController playerAbilityController;
        //protected int maxUses = 4;                                          // This is current in Use
        protected int usesLeft = 4;                                         


        //--Properties
        //public int MaxUses { get { return maxUses; } }
        public int UsesLeft { 
            get { return usesLeft; }
            set { usesLeft = value; }
        }
        public bool IsRunnerAbility { get { return isRunnerAbility; } }
        public bool IsChaserAbility { get { return isChaserAbility; } }


        protected virtual void Awake() {
            playerAbilityController = GetComponentInParent<PlayerAbilityController>();
        }

        public virtual void RoleStartSetup(bool isRunner) {
            usesLeft = chaserMaxUses;
            if (isRunner) {
                usesLeft = runnerMaxUses;
            }
        }
        public virtual void OnAbilityButtonPressed<T>(T obj) {

        }

        public virtual void OnAbilityButtonHeld<T>(T obj) {

        }

        public virtual void OnAbilityButtonReleased<T>(T obj) {

        }

        public virtual void ChangeToAbility() {

        }
        public virtual void ChangeFromAbility() {

        }

        //--Can be implemented by each Spawnable ability to determine if they can be activated
        public virtual bool CanStartAbility() {
            return true;
        }
        public virtual bool CanSwitchFrom() {
            return true;
        }

        

        public virtual void Reset() {

        }
    }
}
