using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CarTag.MainMenu {
    [System.Serializable]
    class ToggleButton {
        [SerializeField] private Button button1;
        [SerializeField] private Button button2;
        [SerializeField] Sprite selectedImage;
        [SerializeField] Sprite unSelectedImage;

        public void Button1Selected() {
            button1.image.sprite = selectedImage;
            button2.image.sprite = unSelectedImage;


        }
        public void Button2Selected() {
            button2.image.sprite = selectedImage;
            button1.image.sprite = unSelectedImage;
        }

    }
}
