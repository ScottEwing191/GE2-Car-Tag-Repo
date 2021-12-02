using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CarTag
{
    public class TestScript : MonoBehaviour
    {
        public GameObject runner;
        public GameObject chaser;
        public Rigidbody runnerRB;
        public Rigidbody chaserRB;

        Vector3 currentVelocity;
        Vector3 currentAngularVelocity;


        void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) {
                if (runner.activeInHierarchy) {
                    chaser.transform.position = runner.transform.position;
                    chaser.transform.rotation = runner.transform.rotation;

                    currentVelocity = runnerRB.velocity;
                    currentAngularVelocity = runnerRB.angularVelocity;

                    runner.SetActive(false);
                    chaser.SetActive(true);
                    
                    chaserRB.AddForce(currentVelocity, ForceMode.VelocityChange);
                    chaserRB.AddTorque(currentAngularVelocity, ForceMode.VelocityChange);

                }
                else if (!runner.activeInHierarchy) {
                    runner.transform.position = chaser.transform.position;
                    runner.transform.rotation = chaser.transform.rotation;

                    currentVelocity = chaserRB.velocity;
                    currentAngularVelocity = chaserRB.angularVelocity;

                    runner.SetActive(true);
                    chaser.SetActive(false);

                    runnerRB.AddForce(currentVelocity, ForceMode.VelocityChange);
                    runnerRB.AddTorque(currentAngularVelocity, ForceMode.VelocityChange);
                }  
            }
        }
    }
}
