using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CarTag.UI
{
    public class CheckpointGuideUI :MonoBehaviour
    {
        private GameObject _checkpointGuide;
        [SerializeField] Transform _target;
        [SerializeField] private float rotateSpeed = 50.0f;

        public Transform Target { get; set; }

        public GameObject CheckpointGuide
        {
            get { return _checkpointGuide; }
        }

        private void Update()
        {
            //transform.LookAt(Target);
            if (Target == null) {
                return;
            }
            // Determine which direction to rotate towards
            Vector3 targetDirection = Target.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = rotateSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            //Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            //transform.rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f));


        }

    }
}
