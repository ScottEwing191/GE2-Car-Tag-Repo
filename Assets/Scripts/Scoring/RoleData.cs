using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem {
    public enum AbilityType {
        SLOWMO, BOXES, ROCKET
    }
    [System.Serializable]
    public class RoleData {
        public string currentRole_;
        public float roleDuration;                              //set at end of role
        public List<float> timeAvailableBeforeActivation = new List<float>();   // the is the time between the cooldown timer ending and the player using an ability
        public List<AbilityData> abilityDatas = new List<AbilityData>();
        public RoleData(bool isRunner) {
            AbilityData slo = new AbilityData();
            slo.abilityType = "Slow Mo";
            AbilityData box = new AbilityData(); 
            box.abilityType = "Boxes";
            AbilityData rocket = new AbilityData(); 
            rocket.abilityType = "Rocket";

            abilityDatas.Add(box);
            abilityDatas.Add(rocket);
            if (isRunner) {
                abilityDatas.Add(slo);
            }
        }
    }

    [System.Serializable]
    public struct AbilityData {
        public string abilityType;
        public int activations;
    }
}
