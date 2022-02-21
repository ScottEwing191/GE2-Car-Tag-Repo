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
using UnityEngine.InputSystem;

namespace CarTag {
    public enum PlayerRoleEnum { Runner, Chaser }
    public class Player : MonoBehaviour {
        public event Action roleSwapEvent = delegate { };
        public event Action roundEndEvent = delegate { };

        private enum PlayersEnum { PLAYER_1, PLAYER_2, PLAYER_3, PLAYER_4}

        //public static bool areControlsSetup = false;
        [SerializeField] PlayersEnum playerNumber;
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
        public ChangePlayerCars ChangePlayerCars { get; set; }
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
            PlayerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            PlayerManager = FindObjectOfType<PlayerManager>();

           /* if (!areControlsSetup) {
                areControlsSetup = true;
                PlayerManager.SetupPlayersList();
                //PlayerManager.SetPlayersControlScheme();
            }*/
            if (gameObject.name == "Player 1") {
                //PlayerInput.defaultControlScheme = "KeyboardMouse";
                //PlayerInput.SwitchCurrentControlScheme("KeyboardMouse", PlayerInput.devices.ToArray());

                //SetControlScheme(0);
            }

            //print("Player" + gameObject.name);
        }

        public void InitialSetup() {
            PlayerRespawn = GetComponentInChildren<PlayerRespawn>();
            PlayerCollision = GetComponentInChildren<PlayerCollision>();
            RoadSpawnData = GetComponentInChildren<Road.RoadSpawnData>();
            CarController = GetComponentInChildren<CarController>();
            PlayerUIController = GetComponentInChildren<PlayerUIController>();
            PlayerAbilityController = GetComponentInChildren<PlayerAbilityController>();
            PlayerScore = GetComponentInChildren<PlayerScore>();
            //RCC_CarController = GetComponentInChildren<RCC_CarControllerV3>();

        }
        public void SetControlScheme(MainMenu.ControlType type) {
            //PlayerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            //PlayerInput.defaultControlScheme = type.ToString();
            PlayerInput.defaultControlScheme = "KeyboardMouse";

        }

        private void SetControlScheme(int thisPlayerNumber) {
            //PlayerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            MainMenu.MainMenuData data = FindObjectOfType<MainMenu.MainMenuData>();
            if (data ==null) {
                return;
            }
            //PlayerInput.defaultControlScheme = data.PlayerControlTypes[thisPlayerNumber].ToString();
            //print("Test: " + data.PlayerControlTypes[thisPlayerNumber].ToString());

            switch (playerNumber) {
                case PlayersEnum.PLAYER_1:
                    PlayerInput.defaultControlScheme = data.PlayerControlTypes[0].ToString();
                    //PlayerInput.SwitchCurrentControlScheme(data.PlayerControlTypes[0].ToString(), PlayerInput.devices.ToArray());
                    break;
                case PlayersEnum.PLAYER_2:
                    PlayerInput.defaultControlScheme = data.PlayerControlTypes[1].ToString();
                    //PlayerInput.SwitchCurrentControlScheme(data.PlayerControlTypes[1].ToString(), PlayerInput.devices.ToArray());

                    break;
                case PlayersEnum.PLAYER_3:
                    PlayerInput.defaultControlScheme = data.PlayerControlTypes[2].ToString();
                    //PlayerInput.SwitchCurrentControlScheme(data.PlayerControlTypes[2].ToString(), PlayerInput.devices.ToArray());

                    break;
                case PlayersEnum.PLAYER_4:
                    PlayerInput.defaultControlScheme = data.PlayerControlTypes[3].ToString();
                    //PlayerInput.SwitchCurrentControlScheme(data.PlayerControlTypes[3].ToString(), PlayerInput.devices.ToArray());

                    break;
                default:
                    break;
            }
        }

        public void InvokeRoleSwapEvent() {
            roleSwapEvent.Invoke();
        }
        public void InvokeRoundEndEvent() {
            roundEndEvent.Invoke();
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
        }
        public void DisablePlayer() {
            RCC_CarController.canControl = false;
            IsPlayerEnabled = false;
        }
    }
}
