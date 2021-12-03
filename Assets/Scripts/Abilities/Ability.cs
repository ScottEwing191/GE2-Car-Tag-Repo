using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        protected PlayerAbilityController playerAbilityController;
        protected int maxUses = 4;
        protected int usesLeft = 4;
        protected bool isRunnerAbility = true;
        protected bool isChaserAbility = true;


        //--Properties
        public int MaxUses { get { return maxUses; } }
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

        }
        public virtual void OnAbilityButtonPressed<T>(T obj) {

        }

        public virtual void OnAbilityButtonHeld<T>(T obj) {

        }

        public virtual void OnAbilityButtonReleased<T>(T obj) {

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
