using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarTag.MainMenu
{
    public class AB_TestButtons : MonoBehaviour
    {
        
        [SerializeField] Button aTest;
        [SerializeField] Button bTest;

        [SerializeField] Sprite selectedImage;
        [SerializeField] Sprite unSelectedImage;
        


        //public string SelectedTest { get; set; }
        public string SelectedTest;


        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
            SelectedTest = "A Test";
        }

        public void ATestSelected() {
            SelectedTest = "A Test";
            aTest.image.sprite = selectedImage;
            bTest.image.sprite = unSelectedImage;


        }
        public void BTestSelected() {
            SelectedTest = "B Test";
            bTest.image.sprite = selectedImage;
            aTest.image.sprite = unSelectedImage;
        }

    }
}
