using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI
{
    [Serializable]
    public struct ScoreboardUIElements
    {
        [SerializeField] private CanvasGroup scoreBoardGroup; 
        [SerializeField] private List<PlayerRowElements> playerRowElements;
        [SerializeField] private Button nextRoundButton;
        [SerializeField] private Button mainMenuButton;


        public CanvasGroup ScoreBoardGroup {
            get { return scoreBoardGroup; }
            set { scoreBoardGroup = value; }
        }
        public List<PlayerRowElements> PlayerRowElements {
            get { return playerRowElements; }
            set { playerRowElements = value; }
        }
        public Button NextRoundButton {
            get { return nextRoundButton; }
            set { nextRoundButton = value; }
        }
        public Button MainMenuButton {
            get { return mainMenuButton; }
            set { mainMenuButton = value; }
        }

    }
    [Serializable]
    public struct PlayerRowElements {
        [SerializeField] private GameObject playerPositionObject;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerRoundWinsText;

        public GameObject PlayerPositionObject {
            get { return playerPositionObject; }
            set { playerPositionObject = value; }
        }
        public TextMeshProUGUI PlayerNameText {
            get { return playerNameText; }
            set { playerNameText = value; }
        }
        public TextMeshProUGUI PlayerRoundWinsText {
            get { return playerRoundWinsText; }
            set { playerRoundWinsText = value; }
        }

    }
}
