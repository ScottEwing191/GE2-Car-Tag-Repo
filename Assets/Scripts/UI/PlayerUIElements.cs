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
        [SerializeField] private GameObject playerUIObject;
        [SerializeField] private TextMeshProUGUI countdownTimer;
        [SerializeField] private AbilityUIElements abilityUIElements;

        //--Properties
        public GameObject PlayerUIObject {
            get { return playerUIObject; }
            set { playerUIObject = value; }
        }
        public TextMeshProUGUI CountdownTimer {
            get { return countdownTimer; }
            set { countdownTimer = value; }
        }
        public AbilityUIElements AbilityUIElements {
            get { return abilityUIElements; }
            set { abilityUIElements = value; }
        }
    }
}
