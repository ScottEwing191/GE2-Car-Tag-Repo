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
        [Header("Checkpoint Placer Tracker")]
        [Tooltip ("The Runner will drop the next checkpoint when the slider is full")]
        [SerializeField] private Slider placeCheckpointTracker;
        [SerializeField] private TextMeshProUGUI checkpointsAhead;
        //[Header("Distance Tracker")]
        //[SerializeField] private Slider distanceTrackerSider;
        //[SerializeField] private TextMeshProUGUI distanceTrackerText;



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
        /*public Slider DistanceTrackerSlider {
            get { return distanceTrackerSider; }
            set { distanceTrackerSider = value; }
        }
        public TextMeshProUGUI DistanceTrackerText {
            get { return distanceTrackerText; }
            set { distanceTrackerText = value; }
        }*/
    }
}
