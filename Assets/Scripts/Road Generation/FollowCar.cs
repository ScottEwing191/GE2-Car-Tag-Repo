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
        }

        private void LateUpdate() {
            transform.rotation = targetCar.rotation;
            //transform.position = targetCar.position + offset;
            transform.position =  targetCar.TransformPoint(offset);
        }
    }
}
