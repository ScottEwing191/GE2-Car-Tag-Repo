using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace CarTag.Input {
    public class CameraInputHandler : MonoBehaviour {
        public float CameraX { get; private set; }
        public float CameraY { get; private set; }

        private PlayerInput playerInput;
        private InputActionMap cameraActionMap;

        private void Awake() {
            playerInput = GetComponentInParent<PlayerInput>();
            cameraActionMap = playerInput.actions.FindActionMap("Camera");
            cameraActionMap.Enable();
        }
        public void OnLook(InputAction.CallbackContext context) {
            Vector2 value = context.action.ReadValue<Vector2>();
            CameraX = value.x;
            CameraY = value.y;
            
        }
    }
}
