using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI
{
    [Serializable]
    public struct PlayerUIElements
    {
        [SerializeField] private GameObject playerUIElements;
        [SerializeField] private TextMeshProUGUI countdownTimer;

        //--Properties
        public TextMeshProUGUI CountdownTimer {
            get { return countdownTimer; }
            set { countdownTimer = value; }
        }
    }
}
