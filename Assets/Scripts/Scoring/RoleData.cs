using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class RoleData {
        public string currentRole_;
        public float roleDuration;                              //set at end of role
        
        public int slowMoUses;
        public int boxUses;
        public int rocketUses;
        public List<float> timeAvailableBeforeActivations = new List<float>();   // the is the time between the cooldown timer ending and the player using an ability

        public RoleData(bool isRunner) {
            currentRole_ = isRunner ? "Runner" : "Chaser";
        }
    }
}
