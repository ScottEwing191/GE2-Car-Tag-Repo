using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class AbilityEvents
    {
        /// <summary>
        /// The current ability is changed
        /// </summary>
        public Action<int, int> currentAbilityChanged = delegate {  };
        public void OnCurrentAbilityChanged(int usesLeft, int selectedIndex) {
            currentAbilityChanged?.Invoke(usesLeft, selectedIndex);
        }
        
        /// <summary>
        /// Abilities have been Reset
        /// </summary>
        public Action<int, bool,int> resetAbilities = delegate {  };
        public void OnResetAbilities(int usesLeft, bool isRunner, int selectedAbilityIndex) {
            resetAbilities?.Invoke(usesLeft, isRunner, selectedAbilityIndex);
        }
        
        /// <summary>
        /// An ability has been used
        /// </summary>
        public Action<float, int> abilityUsed = delegate {  };
        public void OnAbilityUsed(float time, int usesLeft) {
            abilityUsed?.Invoke(time, usesLeft);
        }
        
        /// <summary>
        /// Ability has started which will remain active for a period of time
        /// </summary>
        public Action<string, float> abilityStarted = delegate {  };
        public void OnAbilityStarted(string message, float duration) {
            abilityStarted?.Invoke(message, duration);
        }
        
        /// <summary>
        /// Ability which was active for a period of time has stopped
        /// </summary>
        public Action abilityStopped = delegate {  };
        public void OnAbilityStopped() {
            abilityStopped?.Invoke();
        }
    }
}
