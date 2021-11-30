using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Input;
using CarTag.UI;
using UnityEngine.InputSystem;

namespace CarTag.Input {
    public class InputManager : MonoBehaviour {

        [SerializeField] private CarInputHandler carInputHandler;
        [SerializeField] private CameraInputHandler cameraInputHandler;
        [SerializeField] private PlayerInputHandler playerInputHandler;

        public UIManager UIManager { get; private set; }

        PlayerInput playerInput;
        private InputActionMap carActionMap, playerActionMap, camerActionMap, UIActionMap;

        public CarInputHandler CarInput {
            get { return carInputHandler; }
        }
        public CameraInputHandler CameraInput {
            get { return cameraInputHandler; }
        }

        private void Awake() {
            playerInput = GetComponent<PlayerInput>();
            carActionMap = playerInput.actions.FindActionMap("Car");
            playerActionMap = playerInput.actions.FindActionMap("Player");
            camerActionMap = playerInput.actions.FindActionMap("Camera");
            UIActionMap = playerInput.actions.FindActionMap("UI");

            carActionMap.Enable();
            playerActionMap.Enable();
            camerActionMap.Enable();
        }

        private void Start() {
            UIManager = GameManager.Instance.UIManager;
        }
    }
}
