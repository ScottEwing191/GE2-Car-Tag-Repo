using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CarTag.Player {
    public enum PlayerRoleEnum { Runner, Chaser }
    public class Player : MonoBehaviour {
        [SerializeField] private PlayerRoleEnum playerRoll = PlayerRoleEnum.Runner;

        // Auto-implemented properties
        public int PlayerListIndex { get; set; }                            // each player knows its own position in the PlayerManager's player list
        public PlayerManager playerManager { get; set; }
        public PlayerCollision PlayerCollision { get; set; }
        public Road.RoadSpawnData RoadSpawnData { get; set; }

        //Properties
        public PlayerRoleEnum PlayerRoll { 
            get { return playerRoll; } 
            set { playerRoll = value; } }     // keeps track of the current roll of the player

        public void InitialSetup() {
            RoadSpawnData = GetComponentInChildren<Road.RoadSpawnData>();
            PlayerCollision = GetComponentInChildren<PlayerCollision>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

    }
}
