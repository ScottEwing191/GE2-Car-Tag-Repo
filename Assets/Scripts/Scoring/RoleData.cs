using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class RoleData {
        public string Current_Role;
        public float Role_Duration;                              //set at end of role
        
        public int Slow_Mo_Uses;
        public int Box_Uses;
        public int Rocket_Uses;
        //public List<float> timeAvailableBeforeActivations = new List<float>();   // the is the time between the cooldown timer ending and the player using an ability

        public RoleData(bool isRunner) {
            Current_Role = isRunner ? "Runner" : "Chaser";
        }
    }
}
