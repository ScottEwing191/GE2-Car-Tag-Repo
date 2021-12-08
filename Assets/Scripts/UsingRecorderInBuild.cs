using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class UsingRecorderInBuild : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.RoundManager.DistanceToWin = 100000;
            Invoke("KeepValidRecorder", 1);

        }


        void KeepValidRecorder() {
            var components = GetComponents<RCC_Recorder>();
            foreach (var item in components) {
                if (item.carController != GameManager.Instance.PlayerManager.CurrentRunner.RCC_CarController) {
                    Destroy(item);
                }
            }
        }

        
    }
}
