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
        public string PlayerName;
        public int RoundWins;
        public int RocketHits;
        public int RocketsFired;
        [SerializeField] public List<RoundData> roundData = new List<RoundData>();

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
            PlayerName = thisPlayer.gameObject.name;

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
                currentRole.slowMoUses++;
            }else if(currentAbility.GetType() == typeof(Abilities.BoxSpawn.BoxSpawnAbility)) {
                currentRole.boxUses++;
            }
            else if (currentAbility.GetType() == typeof(RocketAbility)) {
                currentRole.rocketUses++;
            }
            currentRole.timeAvailableBeforeActivations.Add(timeElapsedSinceCooldownEnd);
        }

        public RoundData GetCurrentRoundData() {
            if (roundData.Count > 0) {
                return roundData[roundData.Count - 1];
            }
            else {
                Debug.LogError("Trying to access RoundData before an instance has been added to the list");
                return null;
            }
        }

        internal void SetRoleDuration() {
            GetCurrentRoundData().GetCurrentRoleData().roleDuration = roleTime;
            roleTime = 0;
        }
    }
}
