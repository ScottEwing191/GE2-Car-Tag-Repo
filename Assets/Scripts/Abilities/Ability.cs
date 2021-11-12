using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        protected PlayerAbilityController playerAbilityController;

        protected virtual void Awake() {
            playerAbilityController = GetComponentInParent<PlayerAbilityController>();
        }
        public virtual void OnAbilityButtonPressed<T>(T obj) {

        }

        //public

        public virtual void OnAbilityButtonHeld<T>(T obj) {

        }

        public virtual void OnAbilityButtonReleased<T>(T obj) {

        }
    }
}
