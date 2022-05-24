using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag{
    public class AbilityEvents{
        /// <summary>
        /// The current ability is changed
        /// Subscribers: AbilityUI.ChangeAbilityUI;
        /// </summary>
        public Action<int, int> onCurrentAbilityChanged = delegate { };
        public void CurrentAbilityChanged(int usesLeft, int selectedIndex) {
            if (onCurrentAbilityChanged != null) onCurrentAbilityChanged(usesLeft, selectedIndex);
        }

        /// <summary>
        /// Abilities have been Reset
        /// /// Subscribers: AbilityUI.ResetAbilityUI
        /// </summary>
        public Action<int, bool, int> onResetAbilities = delegate { };

        public void ResetAbilities(int usesLeft, bool isRunner, int selectedAbilityIndex) {
            if (onResetAbilities != null) onResetAbilities(usesLeft, isRunner, selectedAbilityIndex);
        }

        /// <summary>
        /// An ability has been used
        /// </summary>
        public Action<float, int> onAbilityUsed = delegate { };

        public void AbilityUsed(float time, int usesLeft) {
            if (onAbilityUsed != null) onAbilityUsed(time, usesLeft);
        }

        /// <summary>
        /// Ability has started which will remain active for a period of time
        /// Subscribers: AbilityActiveTimerUI.StartTimerUI
        /// </summary>
        public Action<string, float> onAbilityStarted = delegate { };

        public void AbilityStarted(string message, float duration) {
            if (onAbilityStarted != null) onAbilityStarted(message, duration);
        }

        /// <summary>
        /// Ability which was active for a period of time has stopped
        /// Subscribers: AbilityActiveTimerUI.StopTimerUI
        /// </summary>
        public Action onAbilityStopped = delegate { };

        public void AbilityStopped() {
            if (onAbilityStopped != null) onAbilityStopped();
        }
    }
}