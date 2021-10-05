using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
//using Sirenix.Serialization;


namespace CarTag.Road {

    public class RoadSpawnData : MonoBehaviour {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 normal;
        public Vector3 Position { get { return position; } }
        public Vector3 Normal { get { return normal; } }
        public bool IsGrounded { get; private set; }

        public bool GroundedThisFrame { get; private set; }     // is true only the frame that a wheel co
        public bool OffGroundThisFrame { get; private set; }


        [SerializeField] private WheelCollider rearLeftWheel;
        [SerializeField] private WheelCollider rearRightWheel;



        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            GroundedThisFrame = false;
            OffGroundThisFrame = false;
            SetIsGrounded();
            SetPositionAndNormal();
        }

        private void SetIsGrounded() {
            // If niether Wheel is on the ground
            if (rearLeftWheel.isGrounded && rearRightWheel.isGrounded) {
                if (!IsGrounded) {
                    GroundedThisFrame = true;   // only set to true if the car was not already on the ground
                    print("Grounded");
                }
                IsGrounded = true;
            }
            else {
                if (IsGrounded) {
                    OffGroundThisFrame = true;  // only set to true if the car was on the ground previously
                    print("Off Grounded");
                }
                IsGrounded = false;
            }
            print("Grounded: " + IsGrounded);
        }

        private void SetPositionAndNormal() {

            // if only left wheel is grounded
            if (rearLeftWheel.isGrounded && !rearRightWheel.isGrounded) {
                SetPositionAndNormal(rearLeftWheel);
            }
            // if only right wheel is grounded
            else if (!rearLeftWheel.isGrounded && rearRightWheel.isGrounded) {
                SetPositionAndNormal(rearRightWheel);
            }
            // if both wheels are grounded
            else if (rearLeftWheel.isGrounded && rearRightWheel.isGrounded) {
                SetPositionAndNormal(rearLeftWheel, rearRightWheel);
            }
        }

        private void SetPositionAndNormal(WheelCollider wheel) {
            WheelHit wheelHit = new WheelHit();
            wheel.GetGroundHit(out wheelHit);
            normal = wheelHit.normal;
            position = wheelHit.point;      // Should come back and and set it to position between the two wheels
        }

        private void SetPositionAndNormal(WheelCollider leftWheel, WheelCollider rightWheel) {
            WheelHit leftWheelHit = new WheelHit();
            leftWheel.GetGroundHit(out leftWheelHit);

            WheelHit rightWheelHit = new WheelHit();
            rightWheel.GetGroundHit(out rightWheelHit);

            normal = GetAveragVector(leftWheelHit.normal, rightWheelHit.normal);
            position = GetMidPoint(leftWheelHit.point, rightWheelHit.point);    // if   wheels are on different angled roads then point will be in the air
        }

        private Vector3 GetAveragVector(Vector3 v1, Vector3 v2) {
            v1 = v1.normalized;
            v2 = v2.normalized;
            Vector3 average = v1 + v2;
            return average.normalized;
        }

        private Vector3 GetMidPoint(Vector3 v1, Vector3 v2) {
            Vector3 midPoint = (v1 + v2) / 2;
            return midPoint;
        }
    }
}
