using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Input;

namespace CarTag.Input {
    public class InputManager : MonoBehaviour {

        [SerializeField] private CarInputHandler carInput;
        [SerializeField] private CameraInputHandler cameraInput;

        public CarInputHandler CarInput {
            get { return carInput; }
        }
        public CameraInputHandler CameraInput {
            get { return cameraInput; }
        }
        

    }
}
