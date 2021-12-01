using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using CarTag.UI;
using CarTag.Road;
using CarTag.PlayerSpace;
using CarTag.Abilities;
using CarTag.ScoreSystem;

namespace CarTag {
    public enum PlayerRoleEnum { Runner, Chaser }
    public class Player : MonoBehaviour {
        [SerializeField] private PlayerRoleEnum playerRoll = PlayerRoleEnum.Runner;

        // Auto-implemented properties
        public int PlayerListIndex { get; set; }                            // each player knows its own position in the PlayerManager's player list
        public PlayerManager PlayerManager { get; set; }
        public PlayerRespawn PlayerRespawn { get; set; }
        public PlayerCollision PlayerCollision { get; set; }
        public RoadSpawnData RoadSpawnData { get; set; }
        public CarController CarController { get; set; }
        public PlayerUIController PlayerUIController { get; set; }
        public PlayerAbilityController PlayerAbilityController { get; set; }
        public PlayerScore PlayerScore { get; set; }
        public RCC_CarControllerV3 RCC_CarController { get; set; }


        //Properties
        public PlayerRoleEnum PlayerRoll { 
            get { return playerRoll; } 
            set { playerRoll = value; } }     // keeps track of the current roll of the player

        public void InitialSetup() {
            PlayerManager = FindObjectOfType<PlayerManager>();
            PlayerRespawn = GetComponentInChildren<PlayerRespawn>();
            PlayerCollision = GetComponentInChildren<PlayerCollision>();
            RoadSpawnData = GetComponentInChildren<Road.RoadSpawnData>();
            CarController = GetComponentInChildren<CarController>();
            PlayerUIController = GetComponentInChildren<PlayerUIController>();
            PlayerAbilityController = GetComponentInChildren<PlayerAbilityController>();
            PlayerScore = GetComponentInChildren<PlayerScore>();
            RCC_CarController = GetComponentInChildren<RCC_CarControllerV3>();
        }

        public bool IsThisPlayerCurrentRunner() {
            if (this == PlayerManager.CurrentRunner) {
                return true;
            }
            else { return false; }
        }
    }
}
