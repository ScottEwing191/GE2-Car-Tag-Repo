using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CarTag.Input
{
    public class RCCInputHandler : MonoBehaviour
    {
        public float Steering { get; private set; }
        public float Accelerate { get; private set; }
        public float Brake { get; private set; }
        public float Handbrake { get; private set; }
        public float Boost { get; private set; }
        public float CameraX { get; private set; }
        public float CameraY { get; private set; }

        private RCC_Camera camera;

        private void Start() {
            camera = FindObjectOfType<RCC_Camera>();
        }



        public void OnSteer(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<Vector2>();
            Steering = value.x;
        }
        public void OnLook(InputAction.CallbackContext context) {
            Vector2 value = context.action.ReadValue<Vector2>();
            CameraX = value.x;
            CameraY = value.y;

        }
        public void OnAccelerate(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Accelerate = value;
        }
        public void OnBrake(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Brake = value;
            print("Might have to make value positive");
        }
        public void OnHandbrake(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Handbrake = value;
        }

        public void OnBoost(InputAction.CallbackContext context) {
            if (context.started) {
                Boost = 1;
            }
            else if (context.canceled) {
                Boost = 0;
            }
        }
        public void OnChangeCanera(InputAction.CallbackContext context) {
            if (context.started) {
                camera.ChangeCamera();
            }
        }


        internal RCC_Inputs GetInputs() {
            RCC_Inputs inputs = new RCC_Inputs();
            inputs.SetInput(Accelerate, Brake, Steering, Handbrake);
            return inputs;
        }
    }
}
