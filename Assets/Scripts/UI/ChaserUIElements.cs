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
        [SerializeField] private GameObject chaserUIObject;
        [SerializeField] private ChaserCheckpointTrackerUIElements checkpointTracker;
        [SerializeField] private Slider runnerDistanceTrackerSlider;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;


        //--Properties
        public GameObject ChaserUIObject { 
            get { return chaserUIObject; }
        }
        public ChaserCheckpointTrackerUIElements CheckpointTracker {
            get { return checkpointTracker; }
            set { checkpointTracker = value; }
        }
        public Slider RunnerDistanceTrackerSlider {
            get { return runnerDistanceTrackerSlider; }
            set { runnerDistanceTrackerSlider = value; }
        }
        public TextMeshProUGUI RunnerDistanceTrackerText {
            get { return textMeshProUGUI; }
            set { textMeshProUGUI = value; }
        }
    }

    [Serializable]
    public struct ChaserCheckpointTrackerUIElements {
        [SerializeField] private Slider chaserCpTrackerSlider;
        [SerializeField] private Image startCheckpointIcon;
        [SerializeField] private Image endCheckpointIcon;
        [SerializeField] private Image numberCheckpointIcon;
        [SerializeField] private List<Image> defualtCheckpointIcons;
        [SerializeField] private TextMeshProUGUI checkpointsLeftText;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;


        //--Properties
        public Slider ChaserCpTrackerSlider {
            get { return chaserCpTrackerSlider; }
        }
        public Image StartCheckpointIcon {
            get { return startCheckpointIcon; }
        }
        public Image EndCheckpointIcon {
            get { return endCheckpointIcon; }
        }
        public Image NumberCheckpointIcon {
            get { return numberCheckpointIcon; }
        }
        public List<Image> DefualtCheckpointIcons {
            get { return defualtCheckpointIcons; }
        }
        public TextMeshProUGUI CheckpointsLeftText {
            get { return checkpointsLeftText; }
            set {checkpointsLeftText = value; }
        }
        public HorizontalLayoutGroup LayoutGroup {
            get { return layoutGroup; }
        }

    }
}
