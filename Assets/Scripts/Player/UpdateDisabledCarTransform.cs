using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class UpdateDisabledCarTransform : MonoBehaviour
    {
        [SerializeField] private RCC_CarControllerV3 runnerController;
        [SerializeField] private RCC_CarControllerV3 chaserController;


        // Update is called once per frame
        void LateUpdate()
        {
            if (!chaserController.gameObject.activeInHierarchy) {
                chaserController.transform.position = runnerController.transform.position;
                chaserController.transform.rotation = runnerController.transform.rotation;
            }
            else if (!runnerController.gameObject.activeInHierarchy) {
                runnerController.transform.position = chaserController.transform.position;
                runnerController.transform.rotation = chaserController.transform.rotation;
            }
            
        }
    }
}
