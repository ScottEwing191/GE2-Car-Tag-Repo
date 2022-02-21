using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.MainMenu {
    public enum ControlType { Any, Gamepad, KeyboardMouse }
    [Serializable]
    public class ControlOptionsButtons {
        [SerializeField] private Button gamepad;
        [SerializeField] private Button mouseKeyboard;
        [SerializeField] private TextMeshProUGUI gamepadText;
        [SerializeField] private TextMeshProUGUI mouseKeyboardText;


        public Button Gamepad { get { return gamepad; } }
        public Button MouseKeyboard { get { return mouseKeyboard; } }
        public TextMeshProUGUI GamepadText { get { return gamepadText; } }
        public TextMeshProUGUI MouseKeyboardText { get { return mouseKeyboardText; } }




    }
    public class SetPlayerControllers : MonoBehaviour {
        [SerializeField] List<GameObject> playerControlsObject = new List<GameObject>();
        //[SerializeField] List<ToggleButton> playerControls = new List<ToggleButton>();
        private MainMenuData mainMenuData;

        [SerializeField] List<ControlOptionsButtons> controlOptionsButtons = new List<ControlOptionsButtons>();
        [SerializeField] Sprite selectedImage;
        [SerializeField] Sprite unselectedImage;
        [SerializeField] Color selectedTextColor;
        [SerializeField] Color unselectedTextColor;


        private void Awake() {
            mainMenuData = FindObjectOfType<MainMenuData>();
        }
        public void SetUpControlOptionPage(int numberOfPlayer) {
            mainMenuData.PlayerControlTypes.Clear();
            ControlType newControlType = ControlType.Any;
            for (int i = 0; i < playerControlsObject.Count; i++) {
                //--Enable objects for the correct player count
                if (i < numberOfPlayer) {
                    playerControlsObject[i].SetActive(true);
                    
                    //--If this is player one then set the control type to mouse and keyboard otherwise set it to gamepad
                    newControlType = ControlType.Gamepad;
                    if (i==0) {
                        newControlType = ControlType.KeyboardMouse;
                    }
                    mainMenuData.PlayerControlTypes.Add(newControlType);

                }
                //--Disable any other Game objects
                else {
                playerControlsObject[i].SetActive(false);
                }
            }
        }



        //-- Called From the Gamepad or Mouse & Keyboard Buttons
        public void SetPlayer1ControlType(int controlType) {
            int player = 0;
            SetControlType(controlType, player);
            SetButtonImage(controlOptionsButtons[player], controlType);
        }
        public void SetPlayer2ControlType(int controlType) {
            int player = 1;
            SetControlType(controlType, player);
            SetButtonImage(controlOptionsButtons[player], controlType);
        }
        public void SetPlayer3ControlType(int controlType) {
            int player = 2;
            SetControlType(controlType, player);
            SetButtonImage(controlOptionsButtons[player], controlType);
        }
        public void SetPlayer4ControlType(int controlType) {
            int player = 3;
            SetControlType(controlType, player);
            SetButtonImage(controlOptionsButtons[player], controlType);
        }

        private void SetControlType(int controlType, int player) {
            if (Enum.IsDefined(typeof(ControlType), controlType))
                mainMenuData.PlayerControlTypes[player] = (ControlType)controlType;
            else
                Debug.LogError("Invalid Number passed in on button click");
        }

        private void SetButtonImage(ControlOptionsButtons options, int controlType) {
            switch ((ControlType)controlType) {
                case ControlType.Any:
                    break;
                case ControlType.Gamepad:
                    SetButtonImageAndTextColour(options, selectedImage, unselectedImage, selectedTextColor, unselectedTextColor);
                    break;
                case ControlType.KeyboardMouse:
                    SetButtonImageAndTextColour(options, unselectedImage, selectedImage, unselectedTextColor, selectedTextColor);
                    DeselectMouseForOtherPlayers(options);
                    break;
                default:
                    break;
            }
        }

        //--Chenges the button image and text colour of the Gamepad and Mouse and Keyboard buttons
        private void SetButtonImageAndTextColour(ControlOptionsButtons options, Sprite gSprite, Sprite mSprite, Color gColor, Color mColor) {
            options.Gamepad.image.sprite = gSprite;
            options.MouseKeyboard.image.sprite = mSprite;
            options.GamepadText.color = gColor;
            options.MouseKeyboardText.color = mColor;
        }

        //--will make sure that only one player can have Mouse and Keyboard Selected at once
        private void DeselectMouseForOtherPlayers(ControlOptionsButtons dontChangeButtons) {
            for (int i = 0; i < mainMenuData.PlayerControlTypes.Count; i++) {       // loop for number of players selected
                if (controlOptionsButtons[i] == dontChangeButtons) { continue; }

                if (mainMenuData.PlayerControlTypes[i] == ControlType.KeyboardMouse) {
                    SetButtonImageAndTextColour(controlOptionsButtons[i], selectedImage, unselectedImage, selectedTextColor, unselectedTextColor);
                    //mainMenuData.PlayerControlTypes[i] = ControlType.Gamepad;
                    SetControlType((int)ControlType.Gamepad, i);
                }
            }
        }


    }
}
