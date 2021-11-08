using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI
{
    [Serializable]
    public struct RunnerUIElements
    {
        [SerializeField] private GameObject runnerUIObject;
        [Tooltip ("The Runner will drop the next checkpoint when the slider is full")]
        [SerializeField] private Slider placeCheckpointTracker;
        [SerializeField] private TextMeshProUGUI checkpointsAhead;



        //--Properties
        public GameObject RunnerUIObject {
            get { return runnerUIObject; }
        }
        public Slider PlaceCheckpointTracker {
            get { return placeCheckpointTracker; }
            set { placeCheckpointTracker = value; }
        }
        public TextMeshProUGUI CheckpointsAhead {
            get { return checkpointsAhead; }
            set { checkpointsAhead = value; }
        }
    }
}
