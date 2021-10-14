using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Player
{
    public class TestCollision : MonoBehaviour
    {
        public LayerMask layerMask;
        Vector3 center;
        Vector3 size;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            bool collide = Physics.CheckBox(transform.position, size / 2, transform.rotation, layerMask);
            print("Collision: " + collide);
        }
    }
}
