using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] wheels = new WheelCollider[4];
        [SerializeField] private float torque = 200;
        
        void Start()
        {
        
        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.W)) {
                print("Accellerate");
                for (int i = 0; i < wheels.Length; i++) {
                    wheels[i].motorTorque = torque;
                }
            }
            else {
                for (int i = 0; i < wheels.Length; i++) {
                    wheels[i].motorTorque = 0;
                }
            }
            
        }
    }
}
