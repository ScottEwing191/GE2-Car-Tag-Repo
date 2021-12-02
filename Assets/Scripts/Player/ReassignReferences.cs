using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;

namespace CarTag
{
    public class ReassignReferences : MonoBehaviour
    {
        [Header("Car Game Objects")]
        [SerializeField] private GameObject runnerCar;
        [SerializeField] private GameObject chaserCar;


        [Header ("Player Collision")]
        [SerializeField] private PlayerCollision runnerCollisionScript;
        [SerializeField] private PlayerCollision chaserCollisionScript;

        [Header("Player Respawn")]
        [SerializeField] private PlayerRespawn runnerRespawnScript;
        [SerializeField] private PlayerRespawn chaserRespawnScript;

        [Header("Road Spawn Transform")]
        [SerializeField] private List<WheelCollider> runnerRearWheels = new List<WheelCollider>();
        [SerializeField] private List<WheelCollider> chaserRearWheels = new List<WheelCollider>();

        [Header("Rocket Ability")]
        [SerializeField] private Transform runnerSpawnTransform;
        [SerializeField] private Transform chaserSpawnTransform;

        [Header("Box Spawn Ability")]
        [SerializeField] private Transform runnerFollowTransform;
        [SerializeField] private Transform chaserFollowTransform;

        [Header("Camera")]
        [SerializeField] private RCC_CarControllerV3 runnerCarController;
        [SerializeField] private RCC_CarControllerV3 chaserCarController;







        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
