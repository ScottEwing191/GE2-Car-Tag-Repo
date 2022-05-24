using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CarTag.UI.ProgressTracker
{
    [Serializable]
    public struct RunnerProgressTrackerUIElements 
    {
        [field:SerializeField] public Slider ProgressTrackerSliderForRunner { get; set; }
        [field:SerializeField] public Slider ProgressTrackerSliderForChaser{ get; set; }
        [field:SerializeField] public TextMeshProUGUI ProgressTrackerTextForRunner{ get; set; }
        [field:SerializeField] public TextMeshProUGUI ProgressTrackerTextForChaser{ get; set; }
    }
}
