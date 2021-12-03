using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.MainMenu
{
    public class MainMenuUIManager : MonoBehaviour
    {
        private void Awake() {
            Time.timeScale = 1;
        }
        public void ExitGame() {
            Application.Quit();
        }
    }
}
