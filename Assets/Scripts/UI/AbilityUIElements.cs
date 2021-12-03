using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace CarTag.UI

{
    [Serializable]
    public struct AbilityUIElements 
    {
        //--Selected Ability
        [SerializeField] private GameObject abilityUIElementsObjects;       // the parent object that all elemnts are bellow
        [SerializeField] private Image timerFilledImage;
        [Tooltip("A list of icons attached to the selected ability gameobject. Only the currently selected icon will be visible")]
        [SerializeField] private List<GameObject> selectedIcons;
        [SerializeField] private List<GameObject> usesLeftIndicators;
        [SerializeField] private List<AbilityToSelectUI> abilitiesToSelect;

        public GameObject AbilityUIElementsObjects {
            get { return abilityUIElementsObjects; }
        }
        public Image TimerFilledImage {
            get { return timerFilledImage; }
            //set { timerFilledImage = value; }
        }

        public List<GameObject> SelectedIcons {
            get { return selectedIcons; }
            //set { selectedIcons = value; }
        }

        public List<GameObject> UsesLeftIndicators {
            get { return usesLeftIndicators; }
            //set { usesLeftIndicators = value; }
        }
        public List<AbilityToSelectUI> AbilitiesToSelect {
            get { return abilitiesToSelect; }
            //set { abilitiesToSelect = value; }
        }
    }

    [Serializable]
    public struct AbilityToSelectUI {
        [SerializeField] RectTransform rectTransform;
        [SerializeField] GameObject selectedIndicator;


        public RectTransform RectTransform {
            get { return rectTransform; }
            //set { rectTransform = value; }
        }
        public GameObject SelectedIndicator {
            get { return selectedIndicator; }
            //set { selectedIndicator = value; }
        }

    }
}
