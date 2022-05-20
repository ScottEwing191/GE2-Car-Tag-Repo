using CarTag.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem
{
    //--There is one PlayerScore script attached to each player
    //--In future this could be used to track all sorts of information about the player eg., Abilities used, times hit distance/time spent as runner, time drifting
    [System.Serializable]
    public class PlayerScore : MonoBehaviour
    {
        Player thisPlayer;
        public string Test_Type;
        public string Player_Name;
        public int Round_Wins;
        public int Rocket_Hits;
        public int Rockets_Fired;
        public int Total_Slow_Mo_Uses;
        public int Total_Box_Uses;
        public int Total_Rocket_Uses;
        [SerializeField] public List<RoundData> Round_Data = new List<RoundData>();

        private float roleTime;     // how long has the player been in the current role
        private PlayerScoreStats playerScoreStats = new PlayerScoreStats();
        public PlayerScoreStats PlayerScoreStats {
            get { return playerScoreStats; }
        }
        public Player ThisPlayer {
            get { return thisPlayer; }
        }

        //--If I want to replace this if I let the player enter their own names
        private void Awake() {
            thisPlayer = GetComponentInParent<Player>();
            //PlayerScoreStats.PlayerName = thisPlayer.gameObject.name;
            Player_Name = thisPlayer.gameObject.name;

        }
        private void Update() {
            if (thisPlayer.IsPlayerEnabled) {
                roleTime += Time.deltaTime;
            }
        }
        //--Called when an ability is used
        internal void UpdateAbilityUsedTelemetry(Ability currentAbility, float timeElapsedSinceCooldownEnd) {
            RoleData currentRole = GetCurrentRoundData().GetCurrentRoleData();
            if (currentAbility.GetType() == typeof(SlowTimeAbility)) {
                currentRole.Slow_Mo_Uses++;
            }else if(currentAbility.GetType() == typeof(Abilities.BoxSpawn.BoxSpawnAbility)) {
                currentRole.Box_Uses++;
            }
            else if (currentAbility.GetType() == typeof(RocketAbility)) {
                currentRole.Rocket_Uses++;
            }
            //currentRole.timeAvailableBeforeActivations.Add(timeElapsedSinceCooldownEnd);
        }

        public RoundData GetCurrentRoundData() {
            if (Round_Data.Count > 0) {
                return Round_Data[Round_Data.Count - 1];
            }
            else {
                Debug.LogError("Trying to access RoundData before an instance has been added to the list");
                return null;
            }
        }

        internal void SetRoleDuration() {
            GetCurrentRoundData().GetCurrentRoleData().Role_Duration = roleTime;
            roleTime = 0;
        }

        public void CalculateTotalAbilityUses() {
            foreach (var round in Round_Data) {
                foreach (var role in round.Role_Data) {
                    Total_Slow_Mo_Uses += role.Slow_Mo_Uses;
                    Total_Box_Uses += role.Box_Uses;
                    Total_Rocket_Uses += role.Rocket_Uses;
                }
            }
        }
    }
}
