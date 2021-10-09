using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CarTag.Player {
    public enum PlayerRoleEnum { Runner, Chaser }
    public class Player : MonoBehaviour {
        [SerializeField] private PlayerRoleEnum playerRoll = PlayerRoleEnum.Runner;
        public PlayerRoleEnum PlayerRoll { get { return playerRoll; } }     // keeps track of the current roll of the player
        public int PlayerListIndex { get; set; }                            // each player knows its own position in the PlayerManager's player list
        public PlayerManager playerManager { get; set; }
        public PlayerCollision PlayerCollision { get; set; }
        public Road.RoadSpawnData RoadSpawnData { get; set; }

        public void InitialSetup() {
            RoadSpawnData = GetComponentInChildren<Road.RoadSpawnData>();
            PlayerCollision = GetComponentInChildren<PlayerCollision>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

    }
}
