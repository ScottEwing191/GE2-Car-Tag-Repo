using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI
{
    [Serializable]
    public struct ChaserUIElements
    {
        [SerializeField] private GameObject chaserUIElements;
        [SerializeField] private Slider chaserCheckpointsProgress;

        //--Properties
        public Slider ChaserCheckpointsProgress {
            get { return chaserCheckpointsProgress; }
            set { chaserCheckpointsProgress = value; }
        }
    }
}
