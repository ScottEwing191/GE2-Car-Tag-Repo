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
        [SerializeField] protected LayerMask collisionCheckLayers;
        
        //-The box collider center must have XZ values of 0
        protected BoxCollider abilityCollisionCheckTrigger;



        protected override void Awake() {
            base.Awake();
            abilityCollisionCheckTrigger = abilityPrefab.GetComponent<BoxCollider>();

            if (abilityCollisionCheckTrigger != null) {
                if (abilityCollisionCheckTrigger.center.x != 0 || abilityCollisionCheckTrigger.center.z != 0) {
                    Debug.LogError("The prefab collision check box collider must have a center with XZ values of 0");
                } 
            }
        }

        //--Called while the GameObjects collision is turned off and will check if the object would collide if its collision was turned on
        protected bool WouldCollide(Transform location) {
            if (abilityCollisionCheckTrigger == null) {
                Debug.LogError("This ability does not have a Box Collider set up to check if the object would collide");
                return false;
            }
            Vector3 center = location.position + (location.up * abilityCollisionCheckTrigger.center.y);   
            if (Physics.CheckBox(center,
                                    //location.position + abilityCollisionCheckTrigger.center,
                                    abilityCollisionCheckTrigger.size / 2,
                                    location.rotation,
                                    collisionCheckLayers,
                                    QueryTriggerInteraction.Ignore)) {
                return true;
            }
            return false;
        }

        
    }
}
