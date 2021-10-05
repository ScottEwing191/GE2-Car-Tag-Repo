using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
//using Sirenix.Serialization;


namespace CarTag.RoadGeneration {
    
    public class RoadSpawnData : MonoBehaviour {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 normal;
        public Vector3 Position { get { return position; }}
         public Vector3 Normal { get { return normal; }}
        public bool IsGrounded { get; private set; }

        [SerializeField] private WheelCollider rearLeftWheel;
        [SerializeField] private WheelCollider rearRightWheel;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void FixedUpdate() {
            if (rearLeftWheel.isGrounded) {
                WheelHit wheelHit = new WheelHit();
                rearLeftWheel.GetGroundHit(out wheelHit);
                
                print(wheelHit.point);
                position = wheelHit.point;
                normal = wheelHit.normal;
            }
        }
    }
}
