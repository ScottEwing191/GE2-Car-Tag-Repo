using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CarTag.Input
{
    public class CarInputHandler : MonoBehaviour
    {
        [SerializeField] private UnityStandardAssets.Vehicles.Car.CarController carController;
        public float Steering { get; private set; }
        public float Accelerate { get; private set; }
        public float Brake { get; private set; }
        public float Handbrake { get; private set; }

        private void FixedUpdate() {
            carController.Move(Steering, Accelerate, Brake, Handbrake);
        }

        public void OnSteer(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<Vector2>();
            Steering = value.x;
        }
        public void OnAccelerate(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Accelerate = value;
        }
        public void OnBrake(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Brake = -value;
        }
        public void OnHandbrake(InputAction.CallbackContext context) {
            var value = context.action.ReadValue<float>();
            Handbrake = value;
        }
        
    }
}
