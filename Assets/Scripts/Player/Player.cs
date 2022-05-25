using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using CarTag.UI;
using CarTag.Road;
using CarTag.PlayerSpace;
using CarTag.Abilities;
using CarTag.ScoreSystem;
using CarTag.Checkpoints;
using CarTag.Events;


namespace CarTag {
    public enum PlayerRoleEnum { Runner, Chaser }
    public class Player : MonoBehaviour {
        //public event Action roleSwapEvent = delegate { };
        public event Action roundEndEvent = delegate { };
        public event Action playerEnabledEvent = delegate { };
        public event Action playerDisabledEvent = delegate { };

        [SerializeField] private PlayerRoleEnum playerRoll = PlayerRoleEnum.Runner;
        //private PlayerEvents _playerEvents = new PlayerEvents();
        public PlayerEvents PlayerEvents { get; set; } = new PlayerEvents();

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
        public ChangePlayerCars ChangePlayerCars { get; set; }
        //public CheckpointGuide CheckpointGuide { get; set; }
        public PlayerCheckpointsController PlayerCheckpointsController { get; set; }
        public UnityEngine.InputSystem.PlayerInput PlayerInput { get; set; }

        public bool IsPlayerEnabled { get; set; }


        //Properties
        public PlayerRoleEnum PlayerRoll {
            get { return playerRoll; }
            set { playerRoll = value; }
        }     // keeps track of the current roll of the player
        private void Awake() {
            //RCC_CarController = GetComponentInChildren<RCC_CarControllerV3>();
            RCC_CarControllerV3[] rccCarControllers = GetComponentsInChildren<RCC_CarControllerV3>();
            foreach (RCC_CarControllerV3 controller in rccCarControllers) {
                if (controller.gameObject.activeInHierarchy) {
                    RCC_CarController = controller;
                    break;
                }
            }
            ChangePlayerCars = GetComponent<ChangePlayerCars>();
            PlayerManager = FindObjectOfType<PlayerManager>();
            //PlayerEvents = new PlayerEvents();

        }

        public static Player GetThisPlayer(GameObject callingGameObject) {
            Player player = callingGameObject.GetComponentInParent<Player>();
            return player;
        }

        public void InitialSetup() {
            PlayerRespawn = GetComponentInChildren<PlayerRespawn>();
            PlayerCollision = GetComponentInChildren<PlayerCollision>();
            RoadSpawnData = GetComponentInChildren<Road.RoadSpawnData>();
            CarController = GetComponentInChildren<CarController>();
            PlayerUIController = GetComponentInChildren<PlayerUIController>();
            PlayerAbilityController = GetComponentInChildren<PlayerAbilityController>();
            PlayerScore = GetComponentInChildren<PlayerScore>();
            PlayerCheckpointsController = GetComponentInChildren<PlayerCheckpointsController>();
            //CheckpointGuide =GetComponentInChildren<CheckpointGuide>();
            //CheckpointGuide.InitialSetup();
            //RCC_CarController = GetComponentInChildren<RCC_CarControllerV3>();

        }

        
        public void InvokeRoundEndEvent() {
            roundEndEvent?.Invoke();
        }
        public bool IsThisPlayerCurrentRunner() {
            if (this == PlayerManager.CurrentRunner) {
                return true;
            }
            else { return false; }
        }
        public void EnablePlayer() {
            RCC_CarController.canControl = true;
            IsPlayerEnabled = true;
            playerEnabledEvent?.Invoke();    
        }
        public void DisablePlayer() {
            RCC_CarController.canControl = false;
            IsPlayerEnabled = false;
            playerDisabledEvent?.Invoke();
        }
    }
}
