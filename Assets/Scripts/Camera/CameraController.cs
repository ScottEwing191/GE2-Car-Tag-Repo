using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class CameraController : MonoBehaviour {
        
        private float input;
        private GameObject player;
        [SerializeField] Transform cameraTarget;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            //input = Input.GetAxis("Horizontal");
            //transform.localPosition = new Vector3(transform.localPosition.x + input, transform.localPosition.y, transform.localPosition.z);
            //transform.LookAt(player.transform);
            transform.position = cameraTarget.position;
            transform.rotation = cameraTarget.localRotation;
        }
    }
}
