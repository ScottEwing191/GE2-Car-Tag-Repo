using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class GameEvents : MonoSingleton<GameEvents>
    {
        /// <summary>
        /// Players Have Swapped Roles
        /// Subscribers:
        ///     RoleSwapEarly: PlayerManager.ControlPlayerRoleSwap, 
        ///     RoleSwap: UIManager.RoleSwapReset
        ///     RoleSwapNullary Subscribers: SlowTimeAbility.TryDisableSloMo, RoadManager.ResetRoad, 
        /// </summary>
        public static Action<Player, Player> onRoleSwapEarly = delegate { };
        public static Action<Player, Player> onRoleSwap = delegate { };
        public static Action onRoleSwapNullary = delegate { };                  //identical to role swap but takes no arguments

        public static void RoleSwap(Player newRunner, Player newChaser) {
            if (onRoleSwapEarly != null) onRoleSwapEarly(newRunner, newChaser);
            if (onRoleSwap != null) onRoleSwap(newRunner, newChaser);
            if (onRoleSwapNullary != null) onRoleSwapNullary();
        }
        
        /// <summary>
        /// Players Have Swapped Roles
        /// Subscribers:
        ///     RoundResetEarly: PlayerManager., 
        ///     RoundSwap: UIManager.RoleSwapReset
        ///     RoundResetNullary Subscribers: SlowTimeAbility.TryDisableSloMo, RoadManager.ResetRoad, 
        /// </summary>
        public static Action onRoundResetEarly = delegate { };
        public static Action<Player> onRoundReset = delegate { };     
        public static Action onRoundResetNullary = delegate { };
        public static void RoundReset(Player newRunner) {
            if (onRoundResetEarly != null) onRoundResetEarly();
            if (onRoundReset != null) onRoundReset(newRunner);
            if (onRoundResetNullary != null) onRoleSwapNullary();
        }

    }
}
