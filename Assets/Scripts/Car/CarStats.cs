using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Car
{
    [CreateAssetMenu(fileName ="new Car Stats", menuName = "Card Stats")]
    public class CarStats : ScriptableObject
    {
        [SerializeField] private float m_MaximumSteerAngle = 25;
        [Range(0, 1)] [SerializeField] private float m_SteerHelper = 0.644f; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] [SerializeField] private float m_TractionControl = 1; // 0 is no traction control, 1 is full interference
        [SerializeField] private float m_FullTorqueOverAllWheels = 2500;
        [SerializeField] private float m_ReverseTorque = 500;
        [SerializeField] private float m_MaxHandbrakeTorque = 100000000;
        [SerializeField] private float m_Downforce = 100f;
        [SerializeField] private float m_Topspeed = 150;
        [SerializeField] private float m_SlipLimit = 0.3f;
        [SerializeField] private float m_BrakeTorque = 20000;
    }
}
