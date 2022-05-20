using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.PlayerSpace
{
    public class SetPlayerLayer : MonoBehaviour
    {
        private void Update() {
            Player thisPlayer = GetComponentInParent<Player>();
            //if (thisPlayer.gameObject.name == Player 1)
            string layerName = "";
            switch (thisPlayer.PlayerListIndex) {
                case 0:
                    layerName = "Player 1";
                    break;
                case 1:
                    layerName = "Player 2";
                    break;
                case 2:
                    layerName = "Player 3";
                    break;
                case 3:
                    layerName = "Player 4";
                    break;
                default:
                    break;
            }
            gameObject.layer = LayerMask.NameToLayer(layerName);
            foreach (Transform child in transform) {
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }
}
