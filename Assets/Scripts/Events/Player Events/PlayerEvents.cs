using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Events
{
    public class PlayerEvents 
    {
        public CheckpointEvents CheckpointEvents { get; set; }
        public AbilityEvents AbilityEvents { get; set; }

        public PlayerEvents() {
            AbilityEvents = new AbilityEvents();
        }
    }
}
