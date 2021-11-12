using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Abilities
{
    public abstract class SpawnableAbility : Ability
    {
        [SerializeField] protected GameObject abilityPrefab;
        [SerializeField] protected Transform runnerSpawnTransform;
        [SerializeField] protected Transform chaserSpawnTransform;
        [Tooltip("Dont spawn boxes if any of these layers would be are collided with")]
        [SerializeField] protected LayerMask collisionCheckLayer;
        
        //-The box collider center must have XZ values of 0
        protected BoxCollider abilityCollisionCheckTrigger;
        

        protected override void Awake() {
            base.Awake();
            abilityCollisionCheckTrigger = abilityPrefab.GetComponent<BoxCollider>();
            if (abilityCollisionCheckTrigger.center.x != 0 || abilityCollisionCheckTrigger.center.z != 0) {
                Debug.LogError("The prefab collision check box collider must have a center with XZ values of 0");
            }
        }

        protected bool WouldCollide(Transform location) {
            Vector3 center = location.position + (location.up * abilityCollisionCheckTrigger.center.y);   
            if (Physics.CheckBox(center,
                                    //location.position + abilityCollisionCheckTrigger.center,
                                    abilityCollisionCheckTrigger.size / 2,
                                    location.rotation,
                                    collisionCheckLayer,
                                    QueryTriggerInteraction.Ignore)) {
                return true;
            }
            return false;
        }
    }
}
