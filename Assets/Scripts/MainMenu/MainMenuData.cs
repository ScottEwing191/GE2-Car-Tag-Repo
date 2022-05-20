using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.MainMenu
{
    public class MainMenuData : MonoBehaviour
    {
        [SerializeField] List<ControlType> playerControlTypes = new List<ControlType>();
        //public List<ControlType> PlayerControlTypes { get; set; }

        public List<ControlType> PlayerControlTypes {
            get {
                return playerControlTypes;
            }

        }

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
            //PlayerControlTypes = new List<ControlType>();
        }

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Y)) {
                print(PlayerControlTypes);
            }
        }
    }
}
