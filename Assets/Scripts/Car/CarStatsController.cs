using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace CarTag.Car
{
    public class CarStatsController : MonoBehaviour
    {
        [SerializeField] CarStats runnerStats;
        [SerializeField] CarStats chaserStats;

        public CarStats RunnerStats { get { return runnerStats; } }
        public CarStats ChaserStats { get { return chaserStats; } }

        public void AssignStats(CarController carController, CarStats stats) {
            carController.MaximumSteerAngle = stats.MaximumSteerAngle;
            carController.SteerHelper = stats.SteerHelper;
            carController.TractionControl = stats.TractionControl;
            carController.FullTorqueOverAllWheels = stats.FullTorqueOverAllWheels;
            carController.ReverseTorque = stats.ReverseTorque;
            carController.MaxHandbrakeTorque = stats.MaxHandbrakeTorque;
            carController.Downforce = stats.Downforce;
            carController.Topspeed = stats.Topspeed;
            carController.SlipLimit = stats.SlipLimit;
            carController.BrakeTorque = stats.BrakeTorque;
        }

        public void DisableCar(CarController carController) {
            carController.FullTorqueOverAllWheels = 0;
        }
        public void EnableCar(CarController carController, CarStats stats) {
            carController.FullTorqueOverAllWheels = stats.FullTorqueOverAllWheels;
        }
    }
}
