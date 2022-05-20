using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System;

namespace CarTag.Rounds
{
    public class PlayerRoundController : MonoBehaviour
    {
        public Player ThisPlayer { get; set; }
        public ForfeitRound ForfeitRound { get; set; }

        private void Awake() {
            ThisPlayer = GetComponentInParent<Player>();
            ForfeitRound = GetComponentInChildren<ForfeitRound>();
        }

        internal void InitialSetup() {
        
        }

        
    }
}
