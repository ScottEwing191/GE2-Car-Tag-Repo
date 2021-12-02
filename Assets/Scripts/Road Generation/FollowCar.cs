using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Road
{
    public class FollowCar : MonoBehaviour
    {
        [SerializeField] private Transform targetCar;

        private Vector3 offset;

        private void Awake() {
            offset = transform.position - targetCar.position;
            if (transform.rotation.eulerAngles.y != 0 || targetCar.rotation.eulerAngles.y != 0) {
                Debug.Log("Either the target Car's or the game object the follow car script is attached to has a y rotation that is not 0. \n " +
                "This has cause problems in the past");
            }
        }

        private void LateUpdate() {
            transform.rotation = targetCar.rotation;
            //transform.position = targetCar.position + offset;
            transform.position =  targetCar.TransformPoint(offset);
        }
    }
}
